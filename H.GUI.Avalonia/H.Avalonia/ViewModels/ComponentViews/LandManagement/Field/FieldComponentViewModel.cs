using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using H.Avalonia.Views.ComponentViews;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Plants;
using H.Core.Services.LandManagement.Fields;
using H.Core.Services.StorageService;
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

    private FieldSystemComponent _selectedFieldSystemComponent;

    private IFieldComponentDto _selectedFieldSystemComponentDto;
    private ICropDto _selectedCropDto;

    private readonly IFieldComponentService _fieldComponentService;
    private readonly IUnitsOfMeasurementCalculator _unitsOfMeasurementCalculator;

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
        IUnitsOfMeasurementCalculator unitsOfMeasurementCalculator) : base(regionManager, eventAggregator, storageService)
    {
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
        if (component is FieldSystemComponent fieldSystemComponent)
        {
            // Keep a reference to the model/domain object
            _selectedFieldSystemComponent = fieldSystemComponent;

            // Build a DTO to represent the model/domain object
            this.SelectedFieldSystemComponentDto = _fieldComponentService.TransferToFieldComponentDto(_selectedFieldSystemComponent);

            // Listen for changes on the DTO
            this.SelectedFieldSystemComponentDto.PropertyChanged += SelectedFieldSystemComponentDtoOnPropertyChanged;
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

        // Release any property change handlers
        this.SelectedFieldSystemComponentDto.PropertyChanged -= SelectedFieldSystemComponentDtoOnPropertyChanged;
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
        var dto = _fieldComponentService.CreateCropDto();
        _fieldComponentService.InitializeCropDto(this.SelectedFieldSystemComponentDto, dto);

        // Use this as the new selected instance
        this.SelectedCropDto = dto;

        // If disabled before, enable this command now so that the user can remove a DTO
        this.RemoveCropCommand.RaiseCanExecuteChanged();
    }
    
    /// <summary>
    /// Some property on the <see cref="SelectedFieldSystemComponentDto"/> has changed. Check if we need to validate any user
    /// input before assigning the value on to the associated <see cref="FieldSystemComponent"/>
    /// </summary>
    private void SelectedFieldSystemComponentDtoOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is FieldSystemComponentDto fieldSystemComponentDto)
        {
            // Before assign values from the bound DTOs, check for any validation errors
            if (!fieldSystemComponentDto.HasErrors)
            {
                // A property on the DTO has been changed by the user, assign the new value to the system object after any unit conversion (if necessary)
                _fieldComponentService.TransferFieldDtoToSystem(fieldSystemComponentDto, _selectedFieldSystemComponent);
            }
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
        }
    }

    #endregion
}