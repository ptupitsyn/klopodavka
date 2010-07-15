using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace KlopModel
{
   public class ModelBase
   {
      #region Fields and Constants


      private static readonly ObservableCollection<Exception> exceptions = new ObservableCollection<Exception>();
      private static readonly Dispatcher uiDispatcher;
      private int busyCount;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes a new instance of the <see cref="ModelBase"/> class.
      /// </summary>
      static ModelBase()
      {
         uiDispatcher = Dispatcher.CurrentDispatcher;
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="ModelBase"/> class.
      /// </summary>
      public ModelBase()
      {
         exceptions.CollectionChanged += (a, e) => OnPropertyChanged("Exceptions");
      }


      #endregion


      #region INotifyPropertyChanged implementation


      /// <summary>
      /// Occurs when a property value changes.
      /// </summary>
      public event PropertyChangedEventHandler PropertyChanged;


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets a value indicating whether this instance is busy.
      /// </summary>
      /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
      [XmlIgnore]
      public virtual bool IsBusy
      {
         get { return busyCount > 0; }
         set
         {
            busyCount += value ? 1 : -1;
            OnPropertyChanged("IsBusy");
         }
      }

      /// <summary>
      /// Gets the busy indicator.
      /// </summary>
      /// <value>The busy indicator.</value>
      public DisposableHelper BusyIndicator
      {
         get { return new DisposableHelper(() => IsBusy = true, () => IsBusy = false); }
      }

      /// <summary>
      /// Gets the exceptions.
      /// </summary>
      /// <value>The exceptions.</value>
      [XmlIgnore]
      public ObservableCollection<Exception> Exceptions
      {
         get { return exceptions; }
      }


      #endregion


      #region Private and protected methods


      /// <summary>
      /// Called when [property changed].
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void OnPropertyChanged(string propertyName)
      {
         DeferOnPropertyChanged(propertyName, DispatcherPriority.Background); 
         //PropertyChangedEventHandler handler = PropertyChanged;
         //if (handler != null)
         //{
         //   if (uiDispatcher.CheckAccess())
         //   {
         //      handler(this, new PropertyChangedEventArgs(propertyName));
         //   }
         //   else
         //   {
         //      UiDispatcherInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
         //   }
         //}
      }


      /// <summary>
      /// Raises deferred PropertyChanged event.
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      /// <param name="priority">The priority.</param>
      protected void DeferOnPropertyChanged(string propertyName, DispatcherPriority priority)
      {
         var handler = PropertyChanged;
         if (handler != null)
         {
            UiDispatcherBeginInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)), priority);
         }
      }


      /// <summary>
      /// Invokes specified action on ui dispatcher thread.
      /// </summary>
      /// <param name="action">The action.</param>
      protected void UiDispatcherInvoke(Action action)
      {
         uiDispatcher.Invoke(action);
      }


      /// <summary>
      /// Invokes specified action on ui dispatcher thread.
      /// </summary>
      /// <param name="action">The action.</param>
      protected void UiDispatcherBeginInvoke(Action action)
      {
         uiDispatcher.BeginInvoke(action);
      }


      /// <summary>
      /// Invokes specified action on ui dispatcher thread.
      /// </summary>
      /// <param name="action">The action.</param>
      /// <param name="priority">The priority.</param>
      protected void UiDispatcherBeginInvoke(Action action, DispatcherPriority priority)
      {
         uiDispatcher.BeginInvoke(action, priority);
      }


      /// <summary>
      /// Reports the exception.
      /// </summary>
      /// <param name="exception">The exception.</param>
      protected void ReportException(Exception exception)
      {
         ReportException(exception, false);
      }


      /// <summary>
      /// Reports the exception.
      /// </summary>
      /// <param name="exception">The exception.</param>
      /// <param name="silent">if set to <c>true</c> [silent].</param>
      protected void ReportException(Exception exception, bool silent)
      {
         UiDispatcherInvoke(() => Exceptions.Add(exception));
         if (silent) return;
         UiDispatcherInvoke(() => { throw new Exception(exception.Message, exception); });
      }


      #endregion
   }
}
