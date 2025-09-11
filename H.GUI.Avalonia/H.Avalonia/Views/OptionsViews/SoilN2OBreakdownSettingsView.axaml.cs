using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using H.Avalonia.ViewModels.OptionsViews;

namespace H.Avalonia.Views.OptionsViews;

public partial class OptionSoilN2OBreakdownView : UserControl
{
    public OptionSoilN2OBreakdownView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Get the viewmodel associated with the view.
    /// </summary>
    private SoilN2OBreakdownSettingsViewModel? ViewModel => DataContext as SoilN2OBreakdownSettingsViewModel;

    /// <summary>
    /// Get the toplevel window of the current window.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    TopLevel GetTopLevel() => TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");

    /// <summary>
    /// Is used to attach the windows manager for displaying notifications.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        ViewModel.NotificationManager = new WindowNotificationManager(GetTopLevel());
    }
}