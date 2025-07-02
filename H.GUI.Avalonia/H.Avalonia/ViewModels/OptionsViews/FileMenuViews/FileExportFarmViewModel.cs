using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using H.Avalonia.Models;
using H.Core.Models;
using H.Core.Services.StorageService;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using H.Core.Services;
using SharpKml.Engine;
using Avalonia.Controls.Notifications;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileExportFarmViewModel : FarmOpenExistingViewmodel
    {
        #region Fields
        private IList<H.Core.Models.Farm> _selectedFarms = new List<H.Core.Models.Farm>();
        private bool _canExportExecute = false;
        #endregion

        #region Constructors
        public FileExportFarmViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            ExportFarms = new DelegateCommand<IStorageFile>(OnExport);
        }
        #endregion

        #region Public Methods
        public async void OnExport(IStorageFile file)
        {
            try
            {
                // Open writing stream from the file.
                await using var stream = await file.OpenWriteAsync();
                using var streamWriter = new StreamWriter(stream);

                JsonSerializer serializer = new()
                {
                    // Serializer and de-serializer must both have this set to Auto
                    TypeNameHandling = TypeNameHandling.Auto,
                };
                serializer.Serialize(streamWriter, SelectedFarms, typeof(H.Core.Models.Farm));

                NotificationManager?.Show(new Notification("Success",
                    "Farm(s) has successfully exported",
                    type: NotificationType.Success,
                    expiration: TimeSpan.FromSeconds(10))
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error exporting farms: {ex.Message}");
                NotificationManager?.Show(new Notification("Error",
                    ex.Message,
                    type: NotificationType.Error,
                    expiration: TimeSpan.FromSeconds(10))
                );
            }

        }
        #endregion

        #region Properties
        public DelegateCommand<IStorageFile> ExportFarms { get; }
        public IList<H.Core.Models.Farm> SelectedFarms
        {
            get => _selectedFarms;
            set
            {
                SetProperty(ref _selectedFarms, value);
                if(SelectedFarms.Count != 0)
                {
                    CanExportExecute = true;
                }
                else
                {
                    CanExportExecute = false;
                }
            }
        }
        public bool CanExportExecute
        {
            get => _canExportExecute;
            set
            {
                SetProperty(ref _canExportExecute, value);
            }
        }
        #endregion

    }
}
