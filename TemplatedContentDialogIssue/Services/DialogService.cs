// <copyright file="DialogService.cs" company="Visual Software Systems Ltd.">Copyright (c) 2025 All rights reserved</copyright>
namespace TemplatedContentDialogIssue.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ServiceInterfaces;
using TemplatedContentDialogIssue.Controls;
using ViewModels;
using Vssl.Samples.ViewModelInterfaces;

/// <summary>
/// Tthe dialog services
/// </summary>
public class DialogService : IDialogService
{
    private IDispatchOnUIThread uiDispatcher;
    private ProgressDialog? progressDialog;

    /// <summary>
    /// The progress dialog control
    /// </summary>
    private AltProgressDialog altProgressDialog;

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogService" /> class.
    /// </summary>
    /// <param name="uiDispatcher">The dispatcher used to marshal onto the UI thread</param>
    public DialogService(IDispatchOnUIThread uiDispatcher)
    {
        this.uiDispatcher = uiDispatcher;
        this.altProgressDialog = new AltProgressDialog();
        this.altProgressDialog.VerticalAlignment = VerticalAlignment.Center;
        this.altProgressDialog.HorizontalAlignment = HorizontalAlignment.Center;
        this.altProgressDialog.Margin = new Thickness(0, 12, 0, 12);
        this.altProgressDialog.Message = "This is where messages and and UNDO button would normally be";
        this.altProgressDialog.Closed += (s, e) =>
        {
            this.NotifyTemplateDialogClosed?.Invoke();
        };
    }

    /// <summary>
    /// An event that notifies a message from the service;
    /// </summary>
    public event Action<string>? NotifyNewMessage;

    /// <summary>
    /// An event that notifies that the remplate dialog has closed;
    /// </summary>
    public event Action? NotifyTemplateDialogClosed;

    /// <summary>
    /// Sets the inplace progress dialog reference.
    /// </summary>
    /// <param name="dialog">The inplace progress dialog control</param>
    /// <param name="dialogContainer">The parent container for templated dialogs</param>
    /// <param name="xamlRootObject">The page Xamlroot</param>
    public void SetInPlaceProgressDialog(object dialog, object dialogContainer, object? xamlRootObject)
    {
        this.progressDialog = (ProgressDialog)dialog;
        var xamlRoot = xamlRootObject as XamlRoot;
        this.altProgressDialog.XamlRoot = xamlRoot ?? AppStateHelper.GetMainWindow()?.Content?.XamlRoot;
        var parent = dialogContainer as Panel;
        if (parent != null)
        {
            parent.Children.Add(this.altProgressDialog);
        }
    }

    /// <summary>
    /// Defines the action command for the templated progress dialog.
    /// </summary>
    /// <param name="undoCommand">The undo command</param>
    public void SetTemplateDialogAction(ICommand undoCommand)
    {
        this.altProgressDialog.UndoCommand = undoCommand;
    }

    /// <summary>
    /// Displays the inplace progress dialog.
    /// </summary>
    /// <returns>An awaitable task</returns>
    public Task ShowInPlaceProgressDialog()
    {
        var showTask = Task.CompletedTask;
        if (this.progressDialog != null)
        {
            showTask = this.uiDispatcher.InvokeTask(async () =>
            {
                try
                {
                    await this.progressDialog.ShowAsync(ContentDialogPlacement.InPlace);
                    this.Notify("Dialog show returned");
                }
                catch (Exception ex)
                {
                    this.Notify(ex.ToString());
                }
            });
        }
        else
        {
            this.Notify("In place dialog object is unset");
        }

        return showTask;
    }

    /// <summary>
    /// Displays the templated progress dialog.
    /// </summary>
    /// <returns>An awaitable task</returns>
    public Task ShowTemplatedProgressDialog()
    {
        return this.uiDispatcher.InvokeTask(async () =>
        {
            try
            {
                await this.altProgressDialog.ShowAsync(ContentDialogPlacement.InPlace);
            }
            catch (Exception ex)
            {
                this.Notify(ex.ToString());
            }
        });
    }

    /// <summary>
    /// Closes the progress dialog.
    /// </summary>
    public void CloseProgressDialog()
    {
        this.uiDispatcher.Invoke(() =>
        {
            this.progressDialog?.Hide();
        });
    }

    /// <summary>
    /// Closes the templated progress dialog.
    /// </summary>
    public void CloseTemplatedProgressDialog()
    {
        this.uiDispatcher.Invoke(() =>
        {
            this.altProgressDialog.Hide();
        });
    }

    private void Notify(string message)
    {
        this.NotifyNewMessage?.Invoke(message);
    }
}
