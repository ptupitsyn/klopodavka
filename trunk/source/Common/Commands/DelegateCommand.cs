using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Common.Commands
{
   /// <summary>
   ///     This class allows delegating the commanding logic to methods passed as parameters,
   ///     and enables a View to bind commands to objects that are not part of the element tree.
   /// </summary>
   public class DelegateCommand : ICommand
   {
      #region Constructors


      /// <summary>
      /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
      /// </summary>
      /// <param name="executeMethod">The execute method.</param>
      public DelegateCommand(Action executeMethod)
         : this(executeMethod, null, false)
      {
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
      /// </summary>
      /// <param name="executeMethod">The execute method.</param>
      /// <param name="canExecuteMethod">The can execute method.</param>
      public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
         : this(executeMethod, canExecuteMethod, false)
      {
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
      /// </summary>
      /// <param name="executeMethod">The execute method.</param>
      /// <param name="canExecuteMethod">The can execute method.</param>
      /// <param name="isAutomaticRequeryDisabled">if set to <c>true</c> [is automatic requery disabled].</param>
      /// <exception cref="ArgumentNullException"><c>executeMethod</c> is null.</exception>
      public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
      {
         if (executeMethod == null)
         {
            throw new ArgumentNullException("executeMethod");
         }

         this.executeMethod = executeMethod;
         this.canExecuteMethod = canExecuteMethod;
         this.isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
      }


      #endregion


      #region Public Methods


      #region Public properties and indexers


      /// <summary>
      ///     Property to enable or disable CommandManager's automatic requery on this command
      /// </summary>
      public bool IsAutomaticRequeryDisabled
      {
         get { return isAutomaticRequeryDisabled; }
         set
         {
            if (isAutomaticRequeryDisabled != value)
            {
               if (value)
               {
                  CommandManagerHelper.RemoveHandlersFromRequerySuggested(canExecuteChangedHandlers);
               }
               else
               {
                  CommandManagerHelper.AddHandlersToRequerySuggested(canExecuteChangedHandlers);
               }
               isAutomaticRequeryDisabled = value;
            }
         }
      }


      #endregion


      #region Public methods


      /// <summary>
      ///     Method to determine if the command can be executed
      /// </summary>
      public bool CanExecute()
      {
         if (canExecuteMethod != null)
         {
            return canExecuteMethod();
         }
         return true;
      }


      /// <summary>
      ///     Execution of the command
      /// </summary>
      public void Execute()
      {
         if (executeMethod != null)
         {
            executeMethod();
         }
      }


      /// <summary>
      ///     Raises the CanExecuteChaged event
      /// </summary>
      public void RaiseCanExecuteChanged()
      {
         OnCanExecuteChanged();
      }


      #endregion


      #region Private and protected methods


      /// <summary>
      ///     Protected virtual method to raise CanExecuteChanged event
      /// </summary>
      protected virtual void OnCanExecuteChanged()
      {
         CommandManagerHelper.CallWeakReferenceHandlers(canExecuteChangedHandlers);
      }


      #endregion


      #endregion


      #region ICommand Members


      /// <summary>
      ///     ICommand.CanExecuteChanged implementation
      /// </summary>
      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (!isAutomaticRequeryDisabled)
            {
               CommandManager.RequerySuggested += value;
            }
            CommandManagerHelper.AddWeakReferenceHandler(ref canExecuteChangedHandlers, value, 2);
         }
         remove
         {
            if (!isAutomaticRequeryDisabled)
            {
               CommandManager.RequerySuggested -= value;
            }
            CommandManagerHelper.RemoveWeakReferenceHandler(canExecuteChangedHandlers, value);
         }
      }


      /// <summary>
      /// Defines the method that determines whether the command can execute in its current state.
      /// </summary>
      /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
      /// <returns>
      /// true if this command can be executed; otherwise, false.
      /// </returns>
      bool ICommand.CanExecute(object parameter)
      {
         return CanExecute();
      }


      /// <summary>
      /// Defines the method to be called when the command is invoked.
      /// </summary>
      /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
      void ICommand.Execute(object parameter)
      {
         Execute();
      }


      #endregion


      #region Data


      private readonly Func<bool> canExecuteMethod;
      private readonly Action executeMethod;
      private List<WeakReference> canExecuteChangedHandlers;
      private bool isAutomaticRequeryDisabled;


      #endregion
   }


   /// <summary>
   ///     This class allows delegating the commanding logic to methods passed as parameters,
   ///     and enables a View to bind commands to objects that are not part of the element tree.
   /// </summary>
   /// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
   public class DelegateCommand<T> : ICommand
   {
      #region Constructors


      /// <summary>
      ///     Constructor
      /// </summary>
      public DelegateCommand(Action<T> executeMethod)
         : this(executeMethod, null, false)
      {
      }


      /// <summary>
      ///     Constructor
      /// </summary>
      public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
         : this(executeMethod, canExecuteMethod, false)
      {
      }


      /// <summary>
      ///     Constructor
      /// </summary>
      /// <exception cref="ArgumentNullException"><c>executeMethod</c> is null.</exception>
      public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
      {
         if (executeMethod == null)
         {
            throw new ArgumentNullException("executeMethod");
         }

         this.executeMethod = executeMethod;
         this.canExecuteMethod = canExecuteMethod;
         this.isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
      }


      #endregion


      #region Public Methods


      #region Public properties and indexers


      /// <summary>
      ///     Property to enable or disable CommandManager's automatic requery on this command
      /// </summary>
      public bool IsAutomaticRequeryDisabled
      {
         get { return isAutomaticRequeryDisabled; }
         set
         {
            if (isAutomaticRequeryDisabled != value)
            {
               if (value)
               {
                  CommandManagerHelper.RemoveHandlersFromRequerySuggested(canExecuteChangedHandlers);
               }
               else
               {
                  CommandManagerHelper.AddHandlersToRequerySuggested(canExecuteChangedHandlers);
               }
               isAutomaticRequeryDisabled = value;
            }
         }
      }


      #endregion


      #region Public methods


      /// <summary>
      ///     Method to determine if the command can be executed
      /// </summary>
      public bool CanExecute(T parameter)
      {
         if (canExecuteMethod != null)
         {
            return canExecuteMethod(parameter);
         }
         return true;
      }


      /// <summary>
      ///     Execution of the command
      /// </summary>
      public void Execute(T parameter)
      {
         if (executeMethod != null)
         {
            executeMethod(parameter);
         }
      }


      /// <summary>
      ///     Raises the CanExecuteChaged event
      /// </summary>
      public void RaiseCanExecuteChanged()
      {
         OnCanExecuteChanged();
      }


      #endregion


      #region Private and protected methods


      /// <summary>
      ///     Protected virtual method to raise CanExecuteChanged event
      /// </summary>
      protected virtual void OnCanExecuteChanged()
      {
         CommandManagerHelper.CallWeakReferenceHandlers(canExecuteChangedHandlers);
      }


      #endregion


      #endregion


      #region ICommand Members


      /// <summary>
      ///     ICommand.CanExecuteChanged implementation
      /// </summary>
      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (!isAutomaticRequeryDisabled)
            {
               CommandManager.RequerySuggested += value;
            }
            CommandManagerHelper.AddWeakReferenceHandler(ref canExecuteChangedHandlers, value, 2);
         }
         remove
         {
            if (!isAutomaticRequeryDisabled)
            {
               CommandManager.RequerySuggested -= value;
            }
            CommandManagerHelper.RemoveWeakReferenceHandler(canExecuteChangedHandlers, value);
         }
      }


      /// <summary>
      /// Defines the method that determines whether the command can execute in its current state.
      /// </summary>
      /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
      /// <returns>
      /// true if this command can be executed; otherwise, false.
      /// </returns>
      bool ICommand.CanExecute(object parameter)
      {
         // if T is of value type and the parameter is not
         // set yet, then return false if CanExecute delegate
         // exists, else return true
         if (canExecuteMethod == null) return true;
         if (parameter == null && typeof (T).IsValueType)
         {
            return (false);
         }
         return CanExecute((T)parameter);
      }


      /// <summary>
      /// Defines the method to be called when the command is invoked.
      /// </summary>
      /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
      void ICommand.Execute(object parameter)
      {
         Execute((T) parameter);
      }


      #endregion


      #region Data


      private readonly Func<T, bool> canExecuteMethod;
      private readonly Action<T> executeMethod;
      private List<WeakReference> canExecuteChangedHandlers;
      private bool isAutomaticRequeryDisabled;


      #endregion
   }


   /// <summary>
   ///     This class contains methods for the CommandManager that help avoid memory leaks by
   ///     using weak references.
   /// </summary>
   internal class CommandManagerHelper
   {
      /// <summary>
      /// Calls the weak reference handlers.
      /// </summary>
      /// <param name="handlers">The handlers.</param>
      internal static void CallWeakReferenceHandlers(List<WeakReference> handlers)
      {
         if (handlers != null)
         {
            // Take a snapshot of the handlers before we call out to them since the handlers
            // could cause the array to me modified while we are reading it.

            var callees = new EventHandler[handlers.Count];
            int count = 0;

            for (int i = handlers.Count - 1; i >= 0; i--)
            {
               WeakReference reference = handlers[i];
               var handler = reference.Target as EventHandler;
               if (handler == null)
               {
                  // Clean up old handlers that have been collected
                  handlers.RemoveAt(i);
               }
               else
               {
                  callees[count] = handler;
                  count++;
               }
            }

            // Call the handlers that we snapshotted
            for (int i = 0; i < count; i++)
            {
               EventHandler handler = callees[i];
               handler(null, EventArgs.Empty);
            }
         }
      }


      /// <summary>
      /// Adds the handlers to requery suggested.
      /// </summary>
      /// <param name="handlers">The handlers.</param>
      internal static void AddHandlersToRequerySuggested(List<WeakReference> handlers)
      {
         if (handlers != null)
         {
            foreach (WeakReference handlerRef in handlers)
            {
               var handler = handlerRef.Target as EventHandler;
               if (handler != null)
               {
                  CommandManager.RequerySuggested += handler;
               }
            }
         }
      }


      /// <summary>
      /// Removes the handlers from requery suggested.
      /// </summary>
      /// <param name="handlers">The handlers.</param>
      internal static void RemoveHandlersFromRequerySuggested(List<WeakReference> handlers)
      {
         if (handlers != null)
         {
            foreach (WeakReference handlerRef in handlers)
            {
               var handler = handlerRef.Target as EventHandler;
               if (handler != null)
               {
                  CommandManager.RequerySuggested -= handler;
               }
            }
         }
      }


      /// <summary>
      /// Adds the weak reference handler.
      /// </summary>
      /// <param name="handlers">The handlers.</param>
      /// <param name="handler">The handler.</param>
      internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
      {
         AddWeakReferenceHandler(ref handlers, handler, -1);
      }


      /// <summary>
      /// Adds the weak reference handler.
      /// </summary>
      /// <param name="handlers">The handlers.</param>
      /// <param name="handler">The handler.</param>
      /// <param name="defaultListSize">Default size of the list.</param>
      internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
      {
         if (handlers == null)
         {
            handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
         }

         handlers.Add(new WeakReference(handler));
      }


      /// <summary>
      /// Removes the weak reference handler.
      /// </summary>
      /// <param name="handlers">The handlers.</param>
      /// <param name="handler">The handler.</param>
      internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
      {
         if (handlers != null)
         {
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
               WeakReference reference = handlers[i];
               var existingHandler = reference.Target as EventHandler;
               if ((existingHandler == null) || (existingHandler == handler))
               {
                  // Clean up old handlers that have been collected
                  // in addition to the handler that is to be removed.
                  handlers.RemoveAt(i);
               }
            }
         }
      }
   }
}