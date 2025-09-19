using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using H.Avalonia.Views.ComponentViews;
using H.Avalonia.Views.ComponentViews.LandManagement;
using H.Avalonia.Views.ComponentViews.LandManagement.Field;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Plants;
using H.Core.Services.LandManagement.Fields;
using H.Core.Services.StorageService;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.LandManagement.Field;

/// <summary>
/// The view model that is used with a <see cref="FieldComponentView"/>.
/// </summary>
public class FieldComponentViewModel : ViewModelBase
{
    #region Fields

    /// <summary>
    /// A collection of all <see cref="IFieldComponentDto"/> that represent all the <see cref="FieldSystemComponent"/>s
    /// </summary>
    private Dictionary<Guid, IFieldComponentDto> _fieldComponentDtos;

    /// <summary>
    /// The selected field
    /// </summary>
    private FieldSystemComponent _selectedFieldSystemComponent;

    /// <summary>
    /// The selected crop
    /// </summary>
    private CropViewItem _selectedCropViewItem;

    /// <summary>
    /// The field DTO that is bound to the view and is based on the values from the <see cref="_selectedFieldSystemComponent"/> model object
    /// </summary>
    private IFieldComponentDto _selectedFieldSystemComponentDto;

    /// <summary>
    /// The crop DTO that is bound to the view and is based on the values from the <see cref="_selectedCropViewItem"/>
    /// </summary>
    private ICropDto _selectedCropDto;

    /// <summary>
    /// A service class to perform domain/business logic on field and crop DTOs/objects
    /// </summary>
    private readonly IFieldComponentService _fieldComponentService;

    /// <summary>
    /// A helper class to convert user input from one units of measurement system to another
    /// </summary>
    private readonly IUnitsOfMeasurementCalculator _unitsOfMeasurementCalculator;

    private ILogger _logger;

    #endregion

    #region Constructors

    public FieldComponentViewModel()
    {
    }

    public FieldComponentViewModel(
        IRegionManager regionManager, 
        IEventAggregator eventAggregator, 
        IStorageService storageService,
        IFieldComponentService fieldComponentService,
        IUnitsOfMeasurementCalculator unitsOfMeasurementCalculator,
        ILogger logger) : base(regionManager, eventAggregator, storageService)
    {
        if (logger != null)
        {
            _logger = logger; 
        }
        else
        {
            throw new ArgumentNullException(nameof(logger));
        }

        if (unitsOfMeasurementCalculator != null)
        {
            _unitsOfMeasurementCalculator = unitsOfMeasurementCalculator;
        }
        else
        {
            throw new ArgumentNullException(nameof(unitsOfMeasurementCalculator));
        }

        if (fieldComponentService != null)
        {
            _fieldComponentService = fieldComponentService;
        }
        else
        {
            throw new ArgumentNullException(nameof(fieldComponentService));
        }

        this.AddCropCommand = new DelegateCommand<object>(OnAddCropExecute, AddCropCanExecute);
        this.RemoveCropCommand = new DelegateCommand<object>(OnRemoveCropExecute, RemoveCropCanExecute);

        _fieldComponentDtos = new Dictionary<Guid, IFieldComponentDto>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Responsible for handling the addition of new crops
    /// </summary>
    public DelegateCommand<object> AddCropCommand { get; set; }

    /// <summary>
    /// Responsible for handling the removal of crops
    /// </summary>
    public DelegateCommand<object> RemoveCropCommand { get; set; }

    /// <summary>
    /// The selected <see cref="SelectedFieldSystemComponentDto"/>
    /// </summary>
    public IFieldComponentDto SelectedFieldSystemComponentDto
    {
        get => _selectedFieldSystemComponentDto;
        set => SetProperty(ref _selectedFieldSystemComponentDto, value);
    }

    /// <summary>
    /// The selected <see cref="CropDto"/>
    /// </summary>
    public ICropDto SelectedCropDto
    {
        get => _selectedCropDto;
        set => SetProperty(ref _selectedCropDto, value);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// When the user navigates to a <see cref="FieldSystemComponent"/>, we must initialize the component and any DTOs
    /// that will be used with the view
    /// </summary>
    /// <param name="component">The <see cref="FieldSystemComponent"/> to display to the user</param>
    public override void InitializeViewModel(ComponentBase component)
    {
        _logger.LogDebug("Initializing " + component);

        if (component is FieldSystemComponent fieldSystemComponent)
        {
            // Keep a reference to the model/domain object
            _selectedFieldSystemComponent = fieldSystemComponent;

            if (_fieldComponentDtos.TryGetValue(component.Guid, out var componentDto))
            {
                this.SelectedFieldSystemComponentDto = componentDto;
            }
            else
            {
                // Build a DTO to represent the model/domain object
                var dto  = _fieldComponentService.TransferToFieldComponentDto(_selectedFieldSystemComponent);

                // Listen for changes on the DTO
                dto.PropertyChanged += FieldSystemComponentDtoOnPropertyChanged;

                this.SelectedFieldSystemComponentDto = dto;

                _fieldComponentDtos.Add(component.Guid, dto);
            }

            if (this.SelectedFieldSystemComponentDto.CropDtos.Any())
            {
                this.SelectedCropDto = this.SelectedFieldSystemComponentDto.CropDtos.First();
            }
            else
            {
                this.AddCropDto();
            }

            _selectedCropViewItem = _fieldComponentService.GetCropViewItemFromDto(this.SelectedCropDto, _selectedFieldSystemComponent);

            this.PropertyChanged += OnPropertyChanged;
        }
    }

    /// <summary>
    /// A first point of entry to this class (after the constructor is called). Get a reference to the <see cref="FieldSystemComponent"/> the
    /// user selected from the <see cref="MyComponentsView"/>.
    /// </summary>
    /// <param name="navigationContext">An object holding a reference to the selected <see cref="FieldSystemComponent"/></param>
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.ContainsKey(GuiConstants.ComponentKey))
        {
            var parameter = navigationContext.Parameters[GuiConstants.ComponentKey];
            if (parameter is FieldSystemComponent fieldSystemComponent)
            {
                this.InitializeViewModel(fieldSystemComponent);
            }
        }
    }

    /// <summary>
    /// Use to perform any cleanup when the user selects another/different component or when leaving the <see cref="MyComponentsView"/>.
    /// </summary>
    /// <param name="navigationContext">Passed up to base implementation</param>
    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        base.OnNavigatedFrom(navigationContext);
        
        this.PropertyChanged -= OnPropertyChanged;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// A user can add a crop under any condition
    /// </summary>
    private bool AddCropCanExecute(object arg)
    {
        return true;
    }

    /// <summary>
    /// Adds a new <see cref="CropDto"/> to the <see cref="SelectedFieldSystemComponentDto"/> property
    /// </summary>
    private void OnAddCropExecute(object obj)
    {
        this.AddCropDto();
    }

    /// <summary>
    /// Some property on the <see cref="SelectedFieldSystemComponentDto"/> has changed. Check if we need to validate any user
    /// input before assigning the value on to the associated <see cref="FieldSystemComponent"/>
    /// </summary>
    private void FieldSystemComponentDtoOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is FieldSystemComponentDto fieldSystemComponentDto)
        {
            /*
             * Before assigning values from the bound DTOs, check for any validation errors. If there are any validation errors
             * we should not proceed with the transfer of user input from the DTO to the model until the validation errors are fixed
             */

            if (!fieldSystemComponentDto.HasErrors)
            {
                // A property on the DTO has been changed by the user, assign the new value to the system object after any unit conversion (if necessary)
                _fieldComponentService.TransferFieldDtoToSystem(fieldSystemComponentDto, _selectedFieldSystemComponent);
            }
        }
    }

    private void CropDtoOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is CropDto cropDto)
        {
            // Persist the changes to the system
            _fieldComponentService.TransferCropDtoToSystem(cropDto, _selectedCropViewItem);
        }
    }

    /// <summary>
    /// Used to indicate to the GUI if the command button should be enabled or disabled
    /// </summary>
    private bool RemoveCropCanExecute(object arg)
    {
        return this.SelectedFieldSystemComponentDto != null && this.SelectedFieldSystemComponentDto.CropDtos.Any();
    }

    private void OnRemoveCropExecute(object obj)
    {
        if (this.SelectedCropDto != null)
        {
            this.SelectedFieldSystemComponentDto.CropDtos.Remove(this.SelectedCropDto);

            // Ensure consecutive ordering (by year) of all crops now that one has been removed
            _fieldComponentService.ResetAllYears(this.SelectedFieldSystemComponentDto.CropDtos);

            this.RemoveCropCommand.RaiseCanExecuteChanged();

            _fieldComponentService.RemoveCropFromSystem(_selectedFieldSystemComponent, this.SelectedCropDto);
        }
    }



    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {

    }

    /// <summary>
    /// Adds a new <see cref="CropDto"/> to the <see cref="SelectedFieldSystemComponentDto"/> property
    /// Used in both the <see cref="OnAddCropExecute(object)"/> and in the <see cref="InitializeViewModel(ComponentBase)"/> methods
    /// </summary>
    private void AddCropDto()
    {
        var dto = _fieldComponentService.CreateCropDto(base.ActiveFarm);
        _fieldComponentService.InitializeCropDto(this.SelectedFieldSystemComponentDto, dto);

        // Use this as the new selected instance
        this.SelectedCropDto = dto;


        // If disabled before, enable this command now so that the user can remove a DTO
        this.RemoveCropCommand.RaiseCanExecuteChanged();

        _fieldComponentService.AddCropDtoToSystem(_selectedFieldSystemComponent, dto);
        _selectedCropViewItem = _fieldComponentService.GetCropViewItemFromDto(dto, _selectedFieldSystemComponent);

        dto.PropertyChanged += CropDtoOnPropertyChanged;
    }

    #endregion
}