using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using H.Core.Models;
using H.Core.Services;
using H.Core.Services.StorageService;
using Prism.Commands;

namespace H.Avalonia.ViewModels.OptionsViews.FileMenuViews
{
    public class FileSaveAsViewModel : ViewModelBase
    {
        #region Fields

        private string _newFarmName;
        private IFarmResultsService_NEW _farmResultsService;

        #endregion

        #region Constructors

        public FileSaveAsViewModel(IStorageService storageService, IFarmResultsService_NEW farmResultsService) : base(storageService) 
        {
            if (farmResultsService != null)
            {
                _farmResultsService = farmResultsService;
            }
            else
            {
                throw new ArgumentNullException(nameof(farmResultsService));
            }

            this.SaveAsCommand = new DelegateCommand(OnSaveAsExecute);
        }

        #endregion

        #region Properties

        public string NewFarmName
        {
            get => _newFarmName;
            set
            {
                if (SetProperty(ref _newFarmName, value))
                {
                    ValidateNewFarmName(nameof(NewFarmName), value);
                }
            }
        }

        public ICommand SaveAsCommand { get; set; }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods
        
        private void ValidateNewFarmName(string propertyName, string farmName)
        {
            base.RemoveError(propertyName);

            if (string.IsNullOrEmpty(farmName))
            {
                base.AddError(propertyName, "Name cannot be empty.");
            }
            else if (base.StorageService.Storage.ApplicationData.Farms.Any(x => x.Name == farmName))
            {
                base.AddError(propertyName, "Cannot have a farm with the same name as an existing one.");
            }
        }

        #endregion

        #region Event Handlers

        private void OnSaveAsExecute()
        {
            if (base.HasErrors)
            {
                return;
            }

            var replicatedFarm = _farmResultsService.ReplicateFarm(base.ActiveFarm);
            replicatedFarm.Name = this.NewFarmName;
            base.StorageService.Storage.ApplicationData.Farms.Add(replicatedFarm);
            NotificationManager?.Show(new Notification(
                title: "Save Success",
                message: "Holos saved successfully.",
                type: NotificationType.Success,
                expiration: TimeSpan.FromSeconds(10))
            );
        }

        #endregion
    }
}
