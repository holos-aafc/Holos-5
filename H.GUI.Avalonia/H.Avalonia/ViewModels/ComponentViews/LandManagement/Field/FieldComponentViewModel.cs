using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoMapper;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Plants;
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
    private ObservableCollection<ICropDto> _cropDtoModels;
    private ObservableCollection<CropType> _cropTypes;

    private readonly IFieldComponentDtoFactory _fieldComponentDtoFactory;
    private readonly ICropDtoFactory _cropDtoFactory;

    #endregion

    #region Constructors

    public FieldComponentViewModel()
    {
    }

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
        this.CropTypes = new ObservableCollection<CropType>() { CropType.Wheat, CropType.Barley, CropType.Oats };

        this.AddCropCommand = new DelegateCommand<object>(OnAddCropExecute, AddCropCanExecute);
    }

    #endregion

    #region Properties

    public DelegateCommand<object> AddCropCommand { get; set; }

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

    public ObservableCollection<CropType> CropTypes
    {
        get => _cropTypes;
        set => SetProperty(ref _cropTypes, value);
    }

    #endregion

    #region Public Methods

    public override void InitializeViewModel(ComponentBase component)
    {
        if (component is FieldSystemComponent fieldSystemComponent)
        {
            _selectedFieldSystemComponent = fieldSystemComponent;

            this.SelectedFieldSystemComponentDto =  this.InitializeFieldComponentDto(_selectedFieldSystemComponent);

            this.SelectedFieldSystemComponentDto.PropertyChanged -= SelectedFieldSystemComponentDtoOnPropertyChanged;
            this.SelectedFieldSystemComponentDto.PropertyChanged += SelectedFieldSystemComponentDtoOnPropertyChanged;

            this.BuildCropDtoCollection(_selectedFieldSystemComponent);
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

    private IFieldComponentDto InitializeFieldComponentDto(FieldSystemComponent fieldSystemComponent)
    {
        IFieldComponentDto fieldDto;

        if (fieldSystemComponent.IsInitialized)
        {
            fieldDto = _fieldComponentDtoFactory.Create(template: fieldSystemComponent);
        }
        else
        {
            fieldDto = _fieldComponentDtoFactory.Create();
            fieldSystemComponent.IsInitialized = true;
        }

        return fieldDto;
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

    private bool AddCropCanExecute(object arg)
    {
        return true;
    }

    private void OnAddCropExecute(object obj)
    {
        var dto = _cropDtoFactory.Create();

        this.CropDtos.Add(dto);
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

    #endregion
}