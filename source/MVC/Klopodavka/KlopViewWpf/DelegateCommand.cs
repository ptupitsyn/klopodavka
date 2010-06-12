#region Usings

using System;
using System.Windows.Input;

#endregion

namespace KlopViewWpf
{
   public class DelegateCommand<T> : ICommand
   {
      #region Fields and Constants

      private readonly Predicate<T> _canExecute;
      private readonly Action<T> _execute;

      #endregion

      #region Constructors

      public DelegateCommand(Action<T> execute)
         : this(execute, null)
      {
      }

      public DelegateCommand(Action<T> execute,
                             Predicate<T> canExecute)
      {
         _execute = execute;
         _canExecute = canExecute;
      }

      #endregion

      #region ICommand implementation

      public event EventHandler CanExecuteChanged;

      public bool CanExecute(object parameter)
      {
         if (_canExecute == null)
         {
            return true;
         }

         return _canExecute((T) parameter);
      }

      public void Execute(object parameter)
      {
         _execute((T) parameter);
      }

      #endregion

      #region Public methods

      public void RaiseCanExecuteChanged()
      {
         if (CanExecuteChanged != null)
         {
            CanExecuteChanged(this, EventArgs.Empty);
         }
      }

      #endregion
   }
}