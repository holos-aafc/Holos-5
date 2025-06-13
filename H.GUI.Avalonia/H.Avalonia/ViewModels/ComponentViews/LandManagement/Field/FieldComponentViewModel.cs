using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
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

public class FieldComponentViewModel : ViewModelBase
{
    #region Fields

    private FieldSystemComponent _selectedFieldSystemComponent;

    private IFieldComponentDto _selectedFieldSystemComponentDto;
    private ICropDto _selectedCropDto;

    private readonly IFieldComponentService _fieldComponentService;

    #endregion

    #region Constructors

    public FieldComponentViewModel()
    {
    }

    public FieldComponentViewModel(
        IRegionManager regionManager, 
        IEventAggregator eventAggregator, 
        IStorageService storageService,

        IFieldComponentService fieldComponentService) : base(regionManager, eventAggregator, storageService)
    {
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

    public DelegateCommand<object> AddCropCommand { get; set; }
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

    public override void InitializeViewModel(ComponentBase component)
    {
        if (component is FieldSystemComponent fieldSystemComponent)
        {
            _selectedFieldSystemComponent = fieldSystemComponent;

            this.SelectedFieldSystemComponentDto = _fieldComponentService.Create(_selectedFieldSystemComponent);

            this.SelectedFieldSystemComponentDto.PropertyChanged += SelectedFieldSystemComponentDtoOnPropertyChanged;
        }
    }

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

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
        base.OnNavigatedFrom(navigationContext);

        this.SelectedFieldSystemComponentDto.PropertyChanged -= SelectedFieldSystemComponentDtoOnPropertyChanged;
    }

    #endregion

    #region Private Methods

    private bool AddCropCanExecute(object arg)
    {
        return true;
    }

    private void OnAddCropExecute(object obj)
    {
        var dto = _fieldComponentService.CreateCropDto();
        _fieldComponentService.InitializeCropDto(this.SelectedFieldSystemComponentDto, dto);
        this.SelectedCropDto = dto;

        this.RemoveCropCommand.RaiseCanExecuteChanged();
    }

    private void SelectedFieldSystemComponentDtoOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is FieldSystemComponentDto fieldSystemComponentDto)
        {
            if (e.PropertyName.Equals(nameof(FieldSystemComponentDto.Name)))
            {
                if (!fieldSystemComponentDto.HasErrors)
                {
                    _selectedFieldSystemComponent.Name = fieldSystemComponentDto.Name;
                }
            }
        }
    }

    private bool RemoveCropCanExecute(object arg)
    {
        return this.SelectedFieldSystemComponentDto != null && this.SelectedFieldSystemComponentDto.CropDtos.Any();
    }

    private void OnRemoveCropExecute(object obj)
    {
        if (this.SelectedCropDto != null)
        {
            this.SelectedFieldSystemComponentDto.CropDtos.Remove(this.SelectedCropDto);

            _fieldComponentService.ResetAllYears(this.SelectedFieldSystemComponentDto.CropDtos);

            this.RemoveCropCommand.RaiseCanExecuteChanged();
        }
    }

    #endregion
}