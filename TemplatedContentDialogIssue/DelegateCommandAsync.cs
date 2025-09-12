// <copyright file="DelegateCommandAsync.cs" company="Visual Software Systems Ltd.">Copyright (c) 2014, 2025 All rights reserved</copyright>

namespace Vssl.Samples.Framework;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TemplatedContentDialogIssue;

/// <summary>
/// A helper class the assembles the methods for command execution
/// </summary>
public class DelegateCommandAsync : ICommand
{
    #region [ Private fields ]

    /// <summary>
    /// The can execute function
    /// </summary>
    private Func<object, bool> canExecute;

    /// <summary>
    /// The execute method
    /// </summary>
    private Func<object, Task> executeAction;

    /// <summary>
    /// The cached value of CanExecute
    /// </summary>
    private bool canExecuteCache;

    /// <summary>
    /// Indicates that the async action is executing
    /// </summary>
    private bool isExecuting;

    private IDispatchOnUIThread uiDispatcher;

    #endregion

    #region [ Constructors ]

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateCommandAsync"/> class
    /// </summary>
    /// <param name="executeAction">The execute method</param>
    public DelegateCommandAsync(IDispatchOnUIThread uiDispatcher, Func<object, Task> executeAction)
    {
        this.uiDispatcher = uiDispatcher;
        this.executeAction = executeAction;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateCommandAsync"/> class
    /// </summary>
    /// <param name="executeAction">The execute method</param>
    /// <param name="canExecute">The can execute function</param>
    public DelegateCommandAsync(IDispatchOnUIThread uiDispatcher, Func<object, Task> executeAction, Func<object, bool> canExecute)
        : this(uiDispatcher, executeAction)
    {
        this.canExecute = canExecute;
    }

    #endregion

    #region [ ICommand Members ]

    /// <summary>
    /// The CanExecuteChanged event
    /// </summary>
    public event EventHandler CanExecuteChanged;

    /// <summary>
    /// Checks to see if the command can be executed
    /// </summary>
    /// <param name="parameter">The optional command parameter</param>
    /// <returns>True if the command can be executed</returns>
    public bool CanExecute(object parameter)
    {
        bool temp = (this.canExecute == null || this.canExecute(parameter)) && !this.isExecuting;

        if (this.canExecuteCache != temp)
        {
            this.canExecuteCache = temp;
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, new EventArgs());
            }
        }

        return this.canExecuteCache;
    }

    /// <summary>
    /// Executes the command action
    /// </summary>
    /// <param name="parameter">The optional command parameter</param>
    public async void Execute(object parameter)
    {
        this.SetIsExecuting(true);
        try
        {
            bool useUIThread = false;
            if (parameter != null && parameter is string)
            {
                useUIThread = (parameter as string) == "UIThread";
            }

            await this.executeAction(parameter).ConfigureAwait(continueOnCapturedContext: useUIThread);
        }
        finally
        {
            this.SetIsExecuting(false);
        }
    }

    /// <summary>
    /// Executes the command action asynchronously
    /// </summary>
    /// <param name="parameter">The optional command parameter</param>
    /// <returns>The task that can be awaited</returns>
    public async Task ExecuteAsync(object parameter)
    {
        this.SetIsExecuting(true);
        try
        {
            await this.executeAction(parameter).ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            this.SetIsExecuting(false);
        }
    }

    #endregion

    #region [ Other public methods ]

    /// <summary>
    /// Forces the raising of the CanExecute Changed event
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        this.uiDispatcher.Invoke(() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty));
    }

    #endregion

    #region [ Private methods ]

    /// <summary>
    /// Flags the state as executing and raises the Can Execute Changed event
    /// </summary>
    /// <param name="newValue">The new value</param>
    private void SetIsExecuting(bool newValue)
    {
        if (newValue != this.isExecuting)
        {
            this.isExecuting = newValue;
            this.RaiseCanExecuteChanged();
        }
    }

    #endregion
}
