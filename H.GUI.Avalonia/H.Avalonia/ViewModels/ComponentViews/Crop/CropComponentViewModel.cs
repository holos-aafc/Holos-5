using System.ComponentModel;

namespace H.Avalonia.ViewModels.ComponentViews.Crop

{
    public class CropComponentViewModel : INotifyPropertyChanged
    {
        private bool _showAdditionalInformation;
        public bool ShowAdditionalInformation
        {
            get => _showAdditionalInformation;
            set
            {
                if (_showAdditionalInformation != value)
                {
                    _showAdditionalInformation = value;
                    OnPropertyChanged(nameof(ShowAdditionalInformation));
                    OnPropertyChanged(nameof(HideAdditionalInformation));
                }
            }
        }

        public bool HideAdditionalInformation
        {
            get => !ShowAdditionalInformation;
            set => ShowAdditionalInformation = !value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}