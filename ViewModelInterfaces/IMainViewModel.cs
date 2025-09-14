// <copyright file="IMainViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2025 All rights reserved</copyright>
namespace ViewModelInterfaces;

using System.ComponentModel;
using System.Windows.Input;
using Vssl.Samples.ViewModelInterfaces;

/// <summary>
/// The view model for the main page.
/// </summary>
public interface IMainViewModel : IBaseViewModel
{
    /// <summary>
    /// Gets the command that displays the in-place dialog.
    /// </summary>
    ICommand ShowInplaceProgressCommand { get; }

    /// <summary>
    /// Gets the command that displays the in-place dialog with an auto close.
    /// </summary>
    ICommand ShowInplaceProgressWithTimeLimitCommand { get; }

    /// <summary>
    /// Gets the command that displays the templated dialog.
    /// </summary>
    ICommand ShowTemplatedProgressCommand { get; }

    /// <summary>
    /// Gets the command that displays the templated dialog with an auto close.
    /// </summary>
    ICommand ShowTemplatedProgressWithTimeLimitCommand { get; }

    /// <summary>
    /// Gets the command that cancels the inplace dialog.
    /// </summary>
    ICommand UndoInplaceCommand { get; }

    /// <summary>
    /// Gets the undo command for the templated dialog.
    /// </summary>
    ICommand UndoTemplateCommand { get; }

    /// <summary>
    /// Gets the last message generated.
    /// </summary>
    string? LastMessage { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog buttons should be enabled.
    /// </summary>
    bool CanShowDiaglogButtons { get; set; }

    /// <summary>
    /// Gets a value indicating whether the in-place dialog contaioner is to be displayed.
    /// </summary>
    bool ShowInPlaceDialogContainer { get; }

    /// <summary>
    /// Gets a value indicating whether the templated dialog contaioner is to be displayed.
    /// </summary>
    bool ShowTemplatedDialogContainer { get; }

    /// <summary>
    /// sets the last message on generation.
    /// </summary>
    /// <param name="message">The message text</param>
    void SetLastMessage(string message);
}
