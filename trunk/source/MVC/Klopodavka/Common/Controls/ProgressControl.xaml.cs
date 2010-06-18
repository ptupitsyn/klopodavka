using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jnj.ThirdDimension.WPFControls
{
   /// <summary>
   /// Interaction logic for ProgressControl.xaml
   /// </summary>
   public partial class ProgressControl : UserControl
   {

        #region Routed Events
        /// <summary>
        /// Denotes not started status
        /// </summary>
        public static RoutedEvent NotStartedEvent;
        /// <summary>
        /// Fired when a query is successfully executed
        /// </summary>
        public static RoutedEvent SuccessEvent;
        /// <summary>
        /// Fired when a query fails execution
        /// </summary>
        public static RoutedEvent FailureEvent;
        /// <summary>
        /// Fired when a query is canceled by the user
        /// </summary>
        public static RoutedEvent CancelEvent;
        /// <summary>
        /// Fired when a query is being executed
        /// </summary>
        public static RoutedEvent ExecutingEvent;
        #endregion

        #region ctors
        /// <summary>
        /// initializes routed events
        /// </summary>
        static ProgressControl()
        {
            NotStartedEvent = EventManager.RegisterRoutedEvent("NotStartedEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ProgressControl));
            SuccessEvent = EventManager.RegisterRoutedEvent("SuccessEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ProgressControl));
            FailureEvent = EventManager.RegisterRoutedEvent("FailureEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ProgressControl));
            CancelEvent = EventManager.RegisterRoutedEvent("CancelEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ProgressControl));
            ExecutingEvent = EventManager.RegisterRoutedEvent("ExecutingEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ProgressControl));
        }
        /// <summary>
        /// initializes the control
        /// </summary>
        public ProgressControl()
        {
            InitializeComponent();
            SetStartExecutionStatus();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Raises routed event denoting execution not started
        /// </summary>
        public void SetNotStartedStatus()
        {
            RaiseEvent(new RoutedEventArgs(NotStartedEvent));
        }
        /// <summary>
        /// Raises routed event denoting success
        /// </summary>
        public void SetSuccessStatus()
        {
            RaiseEvent(new RoutedEventArgs(SuccessEvent));
        }
        /// <summary>
        /// Raises routed event denoting user cancellation
        /// </summary>
        public void SetCancelStatus()
        {
            RaiseEvent(new RoutedEventArgs(CancelEvent));
        }
        /// <summary>
        /// Raises routed event denoting in-progress
        /// </summary>
        public void SetStartExecutionStatus()
        {
            RaiseEvent(new RoutedEventArgs(ExecutingEvent));
        }
        /// <summary>
        /// Raises routed event denoting error
        /// </summary>
        public void SetErrorStatus()
        {
            RaiseEvent(new RoutedEventArgs(FailureEvent));
        }
        #endregion

   }
}
