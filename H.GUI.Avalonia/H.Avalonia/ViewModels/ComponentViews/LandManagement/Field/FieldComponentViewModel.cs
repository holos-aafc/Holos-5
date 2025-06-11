using System;
using System.Collections.ObjectModel;
using AutoMapper;
using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Plants;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.LandManagement.Field;

public class FieldComponentViewModel : ViewModelBase
{
    #region Fields

    private FieldSystemComponent _selectedFieldSystemComponent;

    private IFieldComponentDto _selectedFieldSystemComponentDto;
    private ICropDto _selectedCropDto;
    private ObservableCollection<ICropDto> _cropDtoModels;

    private IFieldComponentDtoFactory _fieldComponentDtoFactory;
    private readonly ICropDtoFactory _cropDtoFactory;

    #endregion

    #region Constructors

    public FieldComponentViewModel(
        IRegionManager regionManager, 
        IEventAggregator eventAggregator, 
        IStorageService storageService,
        IFieldComponentDtoFactory fieldComponentDtoFactory,
        ICropDtoFactory cropDtoFactory) : base(regionManager, eventAggregator, storageService)
    {
        if (cropDtoFactory != null)
        {
            _cropDtoFactory = cropDtoFactory; 
        }
        else
        {
            throw new ArgumentNullException(nameof(cropDtoFactory));
        }

        if (fieldComponentDtoFactory != null)
        {
            _fieldComponentDtoFactory = fieldComponentDtoFactory;
        }
        else
        {
            throw new ArgumentNullException(nameof(fieldComponentDtoFactory));
        }
        
        this.CropDtos = new ObservableCollection<ICropDto>();
    }

    #endregion

    #region Properties

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

    public ObservableCollection<ICropDto> CropDtos
    {
        get => _cropDtoModels;
        set => SetProperty(ref _cropDtoModels, value);
    }

    #endregion

    #region Public Methods

    public override void InitializeViewModel(ComponentBase component)
    {
        if (component is FieldSystemComponent fieldSystemComponent)
        {
            _selectedFieldSystemComponent = fieldSystemComponent;

            this.BuildCropDtoCollection(_selectedFieldSystemComponent);
            this.BuildFieldComponentDto(_selectedFieldSystemComponent);
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

    #endregion

    #region Private Methods

    private void BuildFieldComponentDto(FieldSystemComponent fieldSystemComponent)
    {
        var fieldDto = _fieldComponentDtoFactory.Create(template: fieldSystemComponent);

        this.SelectedFieldSystemComponentDto = fieldDto;
    }

    private void BuildCropDtoCollection(FieldSystemComponent fieldSystemComponent)
    {
        this.CropDtos.Clear();

        foreach (var cropViewItem in fieldSystemComponent.CropViewItems)
        {
            var dto = _cropDtoFactory.Create(template: cropViewItem);

            this.CropDtos.Add(dto);
        }
    }

    #endregion
}