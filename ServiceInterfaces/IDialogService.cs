// <copyright file="IDialogService.cs" company="Visual Software Systems Ltd.">Copyright (c) 2025 All rights reserved</copyright>
namespace ServiceInterfaces;

using System;
using System.Threading.Tasks;
using System.Windows.Input;

/// <summary>
/// Interface to the dialog service
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// An event that notifies a message from the service;
    /// </summary>
    event Action<string>? NotifyNewMessage;

    /// <summary>
    /// An event that notifies that the remplate dialog has closed;
    /// </summary>
    event Action? NotifyTemplateDialogClosed;

    /// <summary>
    /// Sets the inplace progress dialog reference.
    /// </summary>
    /// <param name="dialog">The inplace progress dialog control</param>
    /// <param name="dialogContainer">The parent container for templated dialogs</param>
    /// <param name="xamlRootObject">The page Xamlroot</param>
    void SetInPlaceProgressDialog(object dialog, object dialogContainer, object? xamlRootObject);

    /// <summary>
    /// Defines the action command for the templated progress dialog.
    /// </summary>
    /// <param name="undoCommand">The undo command</param>
    void SetTemplateDialogAction(ICommand undoCommand);

    /// <summary>
    /// Displays the inplace progress dialog.
    /// </summary>
    /// <returns>An awaitable task</returns>
    Task ShowInPlaceProgressDialog();

    /// <summary>
    /// Displays the templated progress dialog.
    /// </summary>
    /// <returns>An awaitable task</returns>
    Task ShowTemplatedProgressDialog();

    /// <summary>
    /// Closes the progress dialog.
    /// </summary>
    void CloseProgressDialog();

    /// <summary>
    /// Closes the templated progress dialog.
    /// </summary>
    void CloseTemplatedProgressDialog();
}
