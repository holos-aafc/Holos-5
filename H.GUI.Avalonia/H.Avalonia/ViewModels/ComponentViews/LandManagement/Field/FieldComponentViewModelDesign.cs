using System.Collections.ObjectModel;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Services.StorageService;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.ViewModels.ComponentViews.LandManagement.Field;

public class FieldComponentViewModelDesign : FieldComponentViewModel
{
    public FieldComponentViewModelDesign()
    {
        base.SelectedFieldSystemComponentDto = new FieldSystemComponentDto();
        base.SelectedFieldSystemComponentDto.Name = "A Field";

        base.CropDtos = new ObservableCollection<ICropDto>()
        {
            new CropDto() { CropType = CropType.Wheat },
        };
    }

    public FieldComponentViewModelDesign(IRegionManager regionManager, IEventAggregator eventAggregator, IStorageService storageService, IFieldComponentDtoFactory fieldComponentDtoFactory, ICropDtoFactory cropDtoFactory) : base(regionManager, eventAggregator, storageService, fieldComponentDtoFactory, cropDtoFactory)
    {
    }
}