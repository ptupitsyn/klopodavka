using System;
using System.Windows;
using System.Windows.Input;

namespace Common.Commands
{
   /// <summary>
   /// This class facilitates associating a key binding in XAML markup to a command
   /// defined in a View Model by exposing a Command dependency property.
   /// The class derives from Freezable to work around a limitation in WPF when data-binding from XAML.
   /// </summary>
   public class CommandReference : Freezable, ICommand
   {
      #region Fields and Constants


      /// <summary>
      /// 
      /// </summary>
      public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof (ICommand), typeof (CommandReference),
                                                                                              new PropertyMetadata(
                                                                                                 new PropertyChangedCallback(OnCommandChanged)));


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets the command.
      /// </summary>
      /// <value>The command.</value>
      public ICommand Command
      {
         get { return (ICommand) GetValue(CommandProperty); }
         set { SetValue(CommandProperty, value); }
      }


      #endregion


      #region ICommand Members


      /// <summary>
      /// Defines the method that determines whether the command can execute in its current state.
      /// </summary>
      /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
      /// <returns>
      /// true if this command can be executed; otherwise, false.
      /// </returns>
      public bool CanExecute(object parameter)
      {
         if (Command != null)
            return Command.CanExecute(parameter);
         return false;
      }


      /// <summary>
      /// Defines the method to be called when the command is invoked.
      /// </summary>
      /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
      public void Execute(object parameter)
      {
         Command.Execute(parameter);
      }


      /// <summary>
      /// Occurs when changes occur that affect whether or not the command should execute.
      /// </summary>
      public event EventHandler CanExecuteChanged;


      #region Private and protected methods


      /// <summary>
      /// Called when [command changed].
      /// </summary>
      /// <param name="d">The d.</param>
      /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
      private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var commandReference = d as CommandReference;
         var oldCommand = e.OldValue as ICommand;
         var newCommand = e.NewValue as ICommand;

         if (oldCommand != null)
         {
            oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
         }
         if (newCommand != null)
         {
            newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
         }
      }


      #endregion


      #endregion


      #region Freezable


      /// <summary>
      /// When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable"/> derived class.
      /// </summary>
      /// <returns>The new instance.</returns>
      protected override Freezable CreateInstanceCore()
      {
         throw new NotImplementedException();
      }


      #endregion
   }
}