// <copyright file="AltProgressDialog.xaml.cs" company="Visual Software Systems Ltd.">Copyright (c) 2021, 2025 All rights reserved</copyright>
namespace TemplatedContentDialogIssue.Controls;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

public sealed partial class AltProgressDialog : ContentDialog
{
    /// <summary>
    /// Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register("Message", typeof(string), typeof(AltProgressDialog), new PropertyMetadata(null));

    /// <summary>
    /// Using a DependencyProperty as the backing store for  UndoCommand.  This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty UndoCommandProperty =
        DependencyProperty.Register("UndoCommand", typeof(ICommand), typeof(AltProgressDialog), new PropertyMetadata(0));

    /// <summary>
    /// Initializes a new instance of the <see cref="AltProgressDialog"/> class
    /// </summary>
    public AltProgressDialog()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the message to display
    /// </summary>
    public string Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    /// <summary>
    /// Gets or sets the command executed when the undo button is pressed
    /// </summary>
    public ICommand UndoCommand
    {
        get { return (ICommand)GetValue(UndoCommandProperty); }
        set { SetValue(UndoCommandProperty, value); }
    }

    /*
    /// <summary>
    /// Event handler for the button click
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The event args</param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (this.UndoCommand != null)
        {
            this.UndoCommand.Execute(null);
        }
    }
    */
}
