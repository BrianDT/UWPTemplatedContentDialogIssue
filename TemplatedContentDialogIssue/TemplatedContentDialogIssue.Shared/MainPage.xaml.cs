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
    using TemplatedContentDialogIssue.Controls;
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

#if __UNO__
        /// <summary>
        /// The progress dialog control
        /// </summary>
        private AltProgressDialog altProgressDialog;
#else

        /// <summary>
        /// The width if the dialog
        /// </summary>
        private double dialogWidth;

        /// <summary>
        /// If true do manual based on the control widths.
        /// </summary>
        private bool manualRepositioning;
#endif

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

#if __UNO__
                this.altProgressDialog = new AltProgressDialog();
                this.altProgressDialog.UndoCommand = this.UndoCommand;
                this.altProgressDialog.Message = "This is where messages and and UNDO button would normally be";
#else
                this.progressDialog.DialogSizeChanged += (d) =>
                {
                    this.dialogWidth = d;
                    this.UpdateDialogOffset();
                };
#endif
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
#if !__UNO__
            this.manualRepositioning = true;
            this.UpdateDialogOffset();
#endif
        }

        /// <summary>
        /// Event handler for the show progress button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private async void Show_Progress(object sender, RoutedEventArgs e)
        {
#if __UNO__
            await this.altProgressDialog.ShowAsync();
#else
            this.dialogContainer.Visibility = Visibility.Visible;
            await this.progressDialog.ShowAsync(ContentDialogPlacement.InPlace);
#endif
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
#if __UNO__
                    await this.altProgressDialog.ShowAsync();
#else
                    this.dialogContainer.Visibility = Visibility.Visible;
                    await this.progressDialog.ShowAsync(ContentDialogPlacement.InPlace);
#endif
                });
            });

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(4));
                DispatchHelper.InvokeOnUIThread(() =>
                {
#if __UNO__
                    this.altProgressDialog.Hide();
#else
                    this.progressDialog.Hide();
                    this.dialogContainer.Visibility = Visibility.Collapsed;
#endif
                });
            });
        }

        /// <summary>
        /// Command handler for the UNDO button.
        /// </summary>
        /// <param name="parameters">Any optional parameters.</param>
        private async Task Undo(object parameters)
        {
#if __UNO__
            this.altProgressDialog.Hide();
#else
            this.progressDialog.Hide();
            this.dialogContainer.Visibility = Visibility.Collapsed;
#endif
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
#if !__UNO__
            if (this.manualRepositioning && this.dialogWidth > 0 && this.containerWidth > 0 && this.containerWidth > this.dialogWidth)
            {
                var offset = -this.dialogWidth;
                this.progressDialog.Margin = new Thickness(offset, progressDialog.Margin.Top, progressDialog.Margin.Right, progressDialog.Margin.Bottom);
            }
#endif
        }
    }
}
