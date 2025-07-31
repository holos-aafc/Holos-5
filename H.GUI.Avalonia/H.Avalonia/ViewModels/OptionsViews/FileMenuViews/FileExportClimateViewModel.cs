using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using H.Core.Providers.Climate;
using H.Core.Providers;
using H.Core.Services.StorageService;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using static H.Avalonia.FileExportClimateView;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileExportClimateViewModel : FileExportFarmViewModel
    {
        #region Fields

        #endregion

        public FileExportClimateViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            this.ExportClimate = new DelegateCommand<object>(OnExport);
        }

        #region Properties

        public DelegateCommand<object> ExportClimate { get; }
        public async Task ExportAsync(H.Core.Models.Farm farm, IStorageFile file)
        {
            try
            {
                const string Extension = ".csv";
                ClimateProvider climateProvider = new ClimateProvider(new SlcClimateDataProvider());
  
                await Task.Run(() =>
                {
                    climateProvider.OutputDailyClimateData(farm, file.Path.LocalPath);
                    var indexOfExtension = file.Path.LocalPath.IndexOf(Extension, StringComparison.OrdinalIgnoreCase);
                    var monthlyFilename = file.Path.LocalPath.Insert(indexOfExtension, "_monthly");
                    climateProvider.OutputMonthlyClimateData(farm, monthlyFilename);
                });

                NotificationManager?.Show(new Notification(H.Core.Properties.Resources.LabelSuccess,
                    "Climate data has been successfully exported for"+" "+$"{file.Name}",
                    type: NotificationType.Success,
                    expiration: TimeSpan.FromSeconds(10))
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error exporting farms: {ex.Message}");
                NotificationManager?.Show(new Notification(H.Core.Properties.Resources.ErrorError,
                    ex.Message,
                    type: NotificationType.Error,
                    expiration: TimeSpan.FromSeconds(10))
                );
            }
        }

        private async void OnExport(object obj)
        {
            if (obj is ExportClimateData data)
            {
               if(data.Farm is H.Core.Models.Farm farm && data.File is IStorageFile file)
               {
                    await this.ExportAsync(farm, file);
               }
                
            }
            else
            {
                NotificationManager?.Show(new Notification(H.Core.Properties.Resources.ErrorError,
                   H.Core.Properties.Resources.ErrorNoDataForExport,
                   type: NotificationType.Error,
                   expiration: TimeSpan.FromSeconds(10))
                );
                return;
            }
        }
        #endregion

        
    }
}
