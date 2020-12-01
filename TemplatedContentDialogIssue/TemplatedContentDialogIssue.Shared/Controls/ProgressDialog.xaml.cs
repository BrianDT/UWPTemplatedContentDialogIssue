// <copyright file="ProgressDialog.xaml.cs" company="Visual Software Systems Ltd.">Copyright (c) 2020 All rights reserved</copyright>

namespace TemplatedContentDialogIssue.Shared.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Windows.Input;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;
    public sealed partial class ProgressDialog : ContentDialog
    {


        public ICommand UndoCommand 
        {
            get { return (ICommand)GetValue(UndoCommandProperty); }
            set { SetValue(UndoCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for  UndoCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UndoCommandProperty =
            DependencyProperty.Register("UndoCommand", typeof(ICommand), typeof(ProgressDialog), new PropertyMetadata(0));


        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class
        /// </summary>
        public ProgressDialog()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.UndoCommand != null)
            {
                this.UndoCommand.Execute(null);
            }
        }
    }
}
