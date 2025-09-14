// <copyright file="MainPage.xaml.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020, 2021, 2025 All rights reserved</copyright>

namespace TemplatedContentDialogIssue;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ServiceInterfaces;
using TemplatedContentDialogIssue.Controls;
using TemplatedContentDialogIssue.Services;
using ViewModelInterfaces;
using ViewModels;
using Vssl.Samples.Framework;
using Vssl.Samples.ViewModelInterfaces;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
    /// <summary>
    /// The width if the dialog container
    /// </summary>
    private double containerWidth;

    /// <summary>
    /// The width if the dialog
    /// </summary>
    private double dialogWidth;

    /// <summary>
    /// If true do manual based on the control widths.
    /// </summary>
    private bool manualRepositioning;

    private IDispatchOnUIThread uiDispatcher;

    private IDialogService dialogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class
    /// </summary>
    public MainPage()
    {
        this.InitializeComponent();

        this.uiDispatcher = new UIDispatcher();
        this.dialogService = new DialogService(this.uiDispatcher);
        this.DataContext = new MainViewModel(this.uiDispatcher, this.dialogService);

        this.Loaded += (s, e) =>
        {
            this.uiDispatcher.Initialise();
            this.dialogService.SetInPlaceProgressDialog(this.progressDialog, this.altDialogContainer, this.XamlRoot);

            this.progressDialog.DialogSizeChanged += (d) =>
            {
                this.dialogWidth = d;
                this.UpdateDialogOffset();
            };

            this.progressDialog.Closed += (s, e) =>
            {
                this.VM.CanShowDiaglogButtons = true;
            };

            this.VM.CanShowDiaglogButtons = true;
        };
    }

    /// <summary>
    /// Gets the current view model
    /// </summary>
    public IMainViewModel VM
    {
        get
        {
#pragma warning disable CS8603 // Possible null reference return.
            return this.DataContext as IMainViewModel;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    /// <summary>
    /// Event handler for the enable manual adjustment button.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Enable_Adjustment(object sender, RoutedEventArgs e)
    {
        this.manualRepositioning = true;
        this.UpdateDialogOffset();
    }

    /// <summary>
    /// Event handler for dialog container size change
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void Container_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (this.containerWidth != e.NewSize.Width)
        {
            this.containerWidth = e.NewSize.Width;
            this.UpdateDialogOffset();
        }
    }

    private void UpdateDialogOffset()
    {
        if (this.manualRepositioning && this.dialogWidth > 0 && this.containerWidth > 0 && this.containerWidth > this.dialogWidth)
        {
            var offset = -this.dialogWidth;
            this.progressDialog.Margin = new Thickness(offset, this.progressDialog.Margin.Top, this.progressDialog.Margin.Right, this.progressDialog.Margin.Bottom);
        }
    }
}
