// <copyright file="MainViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2025 All rights reserved</copyright>
namespace ViewModels;

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ServiceInterfaces;
using ViewModelInterfaces;
using Vssl.Samples.Framework;
using Vssl.Samples.ViewModelInterfaces;
using Vssl.Samples.ViewModels;

/// <summary>
/// The view model for the main page.
/// </summary>
public class MainViewModel : BaseViewModel, IMainViewModel
{
    private bool canShowDiaglogButtons;

    private IDialogService dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel" /> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher used to marshal onto the UI thread</param>
    /// <param name="dialogService">The dialog manager</param>
    public MainViewModel(IDispatchOnUIThread dispatcher, IDialogService dialogService)
        : base(dispatcher)
    {
        this.dialogService = dialogService;
        this.ShowInplaceProgressCommand = new DelegateCommandAsync(this.UIDispatcher, this.Show_Inplace_Progress, (o) => true);
        this.ShowInplaceProgressWithTimeLimitCommand = new DelegateCommandAsync(this.UIDispatcher, this.Show_Inplace_Progress_With_Time_Limit, (o) => true);
        this.ShowTemplatedProgressCommand = new DelegateCommandAsync(this.UIDispatcher, this.Show_Templated_Progress, (o) => true);
        this.ShowTemplatedProgressWithTimeLimitCommand = new DelegateCommandAsync(this.UIDispatcher, this.Show_Templated_Progress_With_Time_Limit, (o) => true);
        this.UndoTemplateCommand = new DelegateCommandAsync(this.UIDispatcher, this.Undo_Templated, (o) => true);
        this.UndoInplaceCommand = new DelegateCommandAsync(this.UIDispatcher, this.Undo_Inplace, (o) => true);

        this.dialogService.SetTemplateDialogAction(this.UndoTemplateCommand);
        this.dialogService.NotifyNewMessage += (m) =>
        {
            this.SetLastMessage(m);
        };
        this.dialogService.NotifyTemplateDialogClosed += () =>
        {
            this.CanShowDiaglogButtons = true;
        };
    }

    /// <summary>
    /// Gets the command that displays the in-place dialog.
    /// </summary>
    public ICommand ShowInplaceProgressCommand { get; private set; }

    /// <summary>
    /// Gets the command that displays the in-place dialog with an auto close.
    /// </summary>
    public ICommand ShowInplaceProgressWithTimeLimitCommand { get; private set; }

    /// <summary>
    /// Gets the command that displays the templated dialog.
    /// </summary>
    public ICommand ShowTemplatedProgressCommand { get; private set; }

    /// <summary>
    /// Gets the command that displays the templated dialog with an auto close.
    /// </summary>
    public ICommand ShowTemplatedProgressWithTimeLimitCommand { get; private set; }

    /// <summary>
    /// Gets the command that cancels the inplace dialog.
    /// </summary>
    public ICommand UndoInplaceCommand { get; private set; }

    /// <summary>
    /// Gets the undo command for the templated dialog.
    /// </summary>
    public ICommand UndoTemplateCommand { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the in-place dialog contaioner is to be displayed.
    /// </summary>
    public bool ShowInPlaceDialogContainer { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the templated dialog contaioner is to be displayed.
    /// </summary>
    public bool ShowTemplatedDialogContainer { get; private set; }

    /// <summary>
    /// Gets the last message generated.
    /// </summary>
    public string? LastMessage { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog buttons should be enabled.
    /// </summary>
    public bool CanShowDiaglogButtons
    {
        get => this.canShowDiaglogButtons;
        set
        {
            if (this.canShowDiaglogButtons != value)
            {
                this.canShowDiaglogButtons = value;
                this.OnPropertyChanged(nameof(this.CanShowDiaglogButtons));
            }
        }
    }

    /// <summary>
    /// sets the last message on generation.
    /// </summary>
    /// <param name="message">The message text</param>
    public void SetLastMessage(string message)
    {
        this.LastMessage = message;
        this.OnPropertyChanged(nameof(this.LastMessage));
    }

    /// <summary>
    /// Event handler for the show progress button.
    /// </summary>
    /// <param name="parameters">Any optional parameters.</param>
    private Task Show_Inplace_Progress(object? parameters)
    {
        this.SetLastMessage(string.Empty);
        this.CanShowDiaglogButtons = false;
        this.ShowInPlaceDialogContainer = true;
        this.OnPropertyChanged(nameof(this.ShowInPlaceDialogContainer));

        // Do not wait for the dialog show to return;
        this.dialogService.ShowInPlaceProgressDialog();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Event handler for the show progress button with a timer.
    /// </summary>
    /// <param name="parameters">Any optional parameters.</param>
    private async Task Show_Inplace_Progress_With_Time_Limit(object? parameters)
    {
        this.SetLastMessage(string.Empty);
        this.CanShowDiaglogButtons = false;
        this.ShowInPlaceDialogContainer = true;
        this.OnPropertyChanged(nameof(this.ShowInPlaceDialogContainer));

        var displayTask = this.dialogService.ShowInPlaceProgressDialog();

        await Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            await this.Undo_Inplace(null);
            await displayTask;
        });
    }

    /// <summary>
    /// Event handler for the show progress button.
    /// </summary>
    /// <param name="parameters">Any optional parameters.</param>
    private Task Show_Templated_Progress(object? parameters)
    {
        this.SetLastMessage(string.Empty);
        this.CanShowDiaglogButtons = false;
        this.ShowTemplatedDialogContainer = true;
        this.OnPropertyChanged(nameof(this.ShowTemplatedDialogContainer));

        // Do not wait for the dialog show to return;
        this.dialogService.ShowTemplatedProgressDialog();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Event handler for the show progress button with a timer.
    /// </summary>
    /// <param name="parameters">Any optional parameters.</param>
    private async Task Show_Templated_Progress_With_Time_Limit(object? parameters)
    {
        this.SetLastMessage(string.Empty);
        this.CanShowDiaglogButtons = false;
        this.ShowTemplatedDialogContainer = true;
        this.OnPropertyChanged(nameof(this.ShowTemplatedDialogContainer));

        var displayTask = this.dialogService.ShowTemplatedProgressDialog();

        await Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            await this.Undo_Templated(parameters);
            await displayTask;
        });
    }

    /// <summary>
    /// Command handler for the UNDO button.
    /// </summary>
    /// <param name="parameters">Any optional parameters.</param>
    private async Task Undo_Inplace(object? parameters)
    {
        this.dialogService.CloseProgressDialog();
        this.ShowInPlaceDialogContainer = false;
        this.OnPropertyChanged(nameof(this.ShowInPlaceDialogContainer));
        await Task.CompletedTask;
    }

    /// <summary>
    /// Command handler for the UNDO button.
    /// </summary>
    /// <param name="parameters">Any optional parameters.</param>
    private async Task Undo_Templated(object? parameters)
    {
        this.dialogService.CloseTemplatedProgressDialog();
        this.ShowTemplatedDialogContainer = false;
        this.OnPropertyChanged(nameof(this.ShowTemplatedDialogContainer));
        await Task.CompletedTask;
    }
}
