// <copyright file="BaseViewModel.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019, 2025 All rights reserved</copyright>

namespace Vssl.Samples.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Vssl.Samples.ViewModelInterfaces;

/// <summary>
/// A base class for all view models
/// </summary>
public abstract class BaseViewModel : IBaseViewModel
{
    /// <summary>
    /// The dispatcher used to marshal onto the UI thread
    /// </summary>
    private IDispatchOnUIThread dispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseViewModel" /> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher used to marshal onto the UI thread</param>
    public BaseViewModel(IDispatchOnUIThread dispatcher)
    {
        this.dispatcher = dispatcher;
    }

    #region [ INotifyPropertyChanged Events ]

    /// <summary>
    ///  Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    /// <summary>
    /// Gets the dispatcher used to marshal onto the UI thread
    /// </summary>
    protected IDispatchOnUIThread UIDispatcher => this.dispatcher;

    /// <summary>
    /// Called when a property value has changed
    /// </summary>
    /// <param name="propertyName">The name of the property that changed</param>
    protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
    {
        if (this.dispatcher != null)
        {
            this.dispatcher.Invoke(() =>
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }
        else
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
