
﻿using System.Collections.ObjectModel;
using H.Core.Enumerations;
using H.Core.Services.StorageService;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class OptionSoilViewModel : ViewModelBase
    {
        #region Constructors
        public OptionSoilViewModel() { }
        public OptionSoilViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            ManageData();
            Data = new SoilDisplayViewModel(storageService);
        }
        #endregion
        #region Properties
        public SoilDisplayViewModel Data { get; set; }
        public ObservableCollection<SoilTexture> SoilTextures { get; set; }
        #endregion
        #region Methods
        public void ManageData()
        {
            SoilTextures = new ObservableCollection<SoilTexture>();
            ActiveFarm = base.StorageService.GetActiveFarm();
            var soilTextures = SoilTexture.GetValues(typeof(SoilTexture));
            foreach (SoilTexture soilTexture in soilTextures)
            {
                if (!SoilTextures.Contains(soilTexture))
                {
                    SoilTextures.Add(soilTexture);
                }
            }
        }
        #endregion
    }
}
