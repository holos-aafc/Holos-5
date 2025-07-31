using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using H.Core.Services.StorageService;
using H.Infrastructure;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileImportFarmViewModel : ViewModelBase
    {
        #region Fields
        private bool _showGrid;
        private bool _isFarmImported;
        private IList<H.Core.Models.Farm> _selectedFarms = new List<H.Core.Models.Farm>();
        private bool _canImport = false;
        private const string exportedFileExtension = ".json";
        #endregion

        #region Constructors
        public FileImportFarmViewModel(IRegionManager regionManager, IStorageService storageService) : base(regionManager, storageService)
        {
            ImportFarms = new DelegateCommand(OnImport);
            this.Farms = new ObservableCollection<H.Core.Models.Farm>();
            this.ShowGrid = false;
            this.IsFarmImported = false;
        }
        #endregion

        #region Properties
        public DelegateCommand ImportFarms { get; }
        public ObservableCollection<H.Core.Models.Farm> Farms { get; set; }
        public bool ShowGrid
        {
            get => _showGrid;
            set => SetProperty(ref _showGrid, value);
        }
        public bool IsFarmImported
        {
            get => _isFarmImported;
            set
            {
                SetProperty(ref _isFarmImported, value);
            }
        }
        public IList<H.Core.Models.Farm> SelectedFarms
        {
            get => _selectedFarms;
            set
            {
                SetProperty(ref _selectedFarms, value);
                if(SelectedFarms.Count != 0)
                {
                    CanImport = true;
                }
                else
                {
                    CanImport = false;
                }
            }
        }
        public bool CanImport
        {
            get => _canImport;
            set => SetProperty(ref _canImport, value);
        }
        #endregion

        #region Event Handlers
        private void OnImport()
        {
            try
            {
                if (this.StorageService != null)
                {
                    foreach (var farm in SelectedFarms)
                    {
                        this.StorageService.AddFarm(farm);
                    }
                }

                NotificationManager?.Show(new Notification(H.Core.Properties.Resources.LabelSuccess,
                    H.Core.Properties.Resources.LabelFarmImportSuccess,
                    type: NotificationType.Success,
                    expiration: TimeSpan.FromSeconds(10))
                );
                IsFarmImported = true;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error importing farms: {ex.Message}");
                NotificationManager?.Show(new Notification(H.Core.Properties.Resources.ErrorError,
                    ex.Message,
                    type: NotificationType.Error,
                    expiration: TimeSpan.FromSeconds(10))
                );

            }
        }

        public async Task<IEnumerable<H.Core.Models.Farm>> GetFarmsFromExportFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Enumerable.Empty<H.Core.Models.Farm>();
            }

            try
            {
                var farms = await Task.Run(() =>
                {
                    // Open reading stream from the file.
                    using var streamReader = new StreamReader(filePath);
                    using JsonReader jsonReader = new JsonTextReader(streamReader);

                    JsonSerializer serializer = new()
                    {
                        // Serializer and de-serializer must both have this set to Auto
                        TypeNameHandling = TypeNameHandling.Auto,
                    };
                    return serializer.Deserialize<List<H.Core.Models.Farm>>(jsonReader);
                });
                return farms ?? Enumerable.Empty<H.Core.Models.Farm>();
            }
            catch (Exception e)
            {
                Trace.TraceError($"{e.Message}");
                if (e.InnerException != null)
                {
                    Trace.TraceError($"{e.InnerException.ToString()}");
                }
                return Enumerable.Empty<H.Core.Models.Farm>();
            }
        }

        public async Task<IEnumerable<H.Core.Models.Farm>> GetExportedFarmsFromDirectoryRecursivelyAsync(string path)
        {
            var result = new List<H.Core.Models.Farm>();

            var stringCollection = new StringCollection();
            var files = FileSystemHelper.ListAllFiles(stringCollection, path, $"*{exportedFileExtension}", isRecursiveScan: true);
            if (files == null)
            {
                return result;
            }

            var farmNumber = 1;
            var totalFarms = files.Count;
            foreach (var filePath in files)
            {
                if(filePath != null && File.Exists(filePath))
                {
                    var farmsFromFile = await GetFarmsFromExportFileAsync(filePath);
                    result.AddRange(farmsFromFile);
                    farmNumber++;
                }
                
            }

            return result;
        }

        #endregion
    }
}
