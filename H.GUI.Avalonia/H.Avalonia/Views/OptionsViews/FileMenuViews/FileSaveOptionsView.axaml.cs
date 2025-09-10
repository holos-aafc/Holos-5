using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using H.Avalonia.ViewModels.OptionsViews.FileMenuViews;

namespace H.Avalonia;

public partial class FileSaveOptionsView : UserControl
{
    public FileSaveOptionsView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Is used to attach the windows manager for displaying notifications.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        ViewModel.NotificationManager = new WindowNotificationManager(GetTopLevel());
    }

    /// <summary>
    /// Get the toplevel window of the current window.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    TopLevel GetTopLevel() => TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Invalid Owner");

    /// <summary>
    /// Get the viewmodel associated with the view.
    /// </summary>
    private FileSaveOptionsViewModel? ViewModel => DataContext as FileSaveOptionsViewModel;
}