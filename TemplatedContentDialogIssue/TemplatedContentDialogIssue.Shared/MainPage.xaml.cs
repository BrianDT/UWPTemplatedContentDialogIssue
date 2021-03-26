// <copyright file="MainPage.xaml.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020 - 2021 All rights reserved</copyright>

namespace TemplatedContentDialogIssue
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using TemplatedContentDialogIssue.Shared;
    using Vssl.Samples.Framework;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;

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

        /// <summary>
        /// Using a DependencyProperty as the backing store for  UndoCommand.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty UndoCommandProperty =
            DependencyProperty.Register("UndoCommand", typeof(ICommand), typeof(MainPage), new PropertyMetadata(0));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.UndoCommand = new DelegateCommandAsync(this.Undo, (o) => true);

            this.Loaded += (s, e) =>
            {
                if (Windows.UI.Xaml.Window.Current != null)
                {
                    DispatchHelper.Initialise();
                }

                this.progressDialog.DialogSizeChanged += (d) =>
                {
                    this.dialogWidth = d;
                    this.UpdateDialogOffset();
                };
            };
        }

        public ICommand UndoCommand
        {
            get { return (ICommand)GetValue(UndoCommandProperty); }
            set { SetValue(UndoCommandProperty, value); }
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
        /// Event handler for the show progress button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private async void Show_Progress(object sender, RoutedEventArgs e)
        {
            this.dialogContainer.Visibility = Visibility.Visible;
            await this.progressDialog.ShowAsync(ContentDialogPlacement.InPlace);
        }

        /// <summary>
        /// Event handler for the show progress button with a timer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void Show_Progress_With_Time_Limit(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                DispatchHelper.InvokeOnUIThread(async () =>
                {
                    this.dialogContainer.Visibility = Visibility.Visible;
                    await this.progressDialog.ShowAsync(ContentDialogPlacement.InPlace);
                });
            });

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(4));
                DispatchHelper.InvokeOnUIThread(() =>
                {
                    this.progressDialog.Hide();
                    this.dialogContainer.Visibility = Visibility.Collapsed;
                });
            });
        }

        /// <summary>
        /// Command handler for the UNDO button.
        /// </summary>
        /// <param name="parameters">Any optional parameters.</param>
        private async Task Undo(object parameters)
        {
            this.progressDialog.Hide();
            this.dialogContainer.Visibility = Visibility.Collapsed;
            await Task.CompletedTask;
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
                this.progressDialog.Margin = new Thickness(offset, progressDialog.Margin.Top, progressDialog.Margin.Right, progressDialog.Margin.Bottom);
            }
        }
    }
}
