using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using H.Avalonia.Models;
using H.Avalonia.ViewModels.OptionsViews.FileMenuViews;
using H.Core.Models;

namespace H.Avalonia;

public partial class FileExportFarmView : UserControl
{

    public FileExportFarmView()
    {
        InitializeComponent();
    }

    private void FarmsDataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is FileExportFarmViewModel vm)
        {
            vm.SelectedFarms = FarmsDataGrid.SelectedItems.Cast<H.Core.Models.Farm>().ToList();
        }
    }

    private async void SaveFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        
        if(DataContext is FileExportFarmViewModel vm)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel != null)
            {
                vm.NotificationManager = new WindowNotificationManager(topLevel);
                vm.SelectedFarms = vm.SelectedFarms.OrderBy(obj => obj.Name).ToList();
                string fileName = string.Join(",", vm.SelectedFarms.Select(farm => farm.Name));
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = H.Core.Properties.Resources.TitleExportFarm,
                    SuggestedStartLocation = await topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Documents),
                    FileTypeChoices = new FilePickerFileType[]
                    {
                        new("Holos export files (*.json)")
                        {
                            Patterns = new[] { "*.json" }
                        }
                    },
                    SuggestedFileName = fileName,
                });

                if (file != null)
                {
                    vm.ExportFarms.Execute(file);
                }
            }
        }
    }
}