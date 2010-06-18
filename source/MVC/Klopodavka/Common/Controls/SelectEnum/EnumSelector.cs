using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Jnj.ThirdDimension.Workflow.WorkflowManagementControls.Controls;

namespace Common.Controls.SelectEnum
{
   /// <summary>
   /// Enum wrapper to allow multiple values selection.
   /// </summary>
   public class EnumSelector : INotifyPropertyChanged
   {
      #region Fields and Constants


      private readonly List<SelectableEnum> values;


      #endregion


      #region Constructors


      /// <summary>
      /// Initializes a new instance of the <see cref="EnumSelector&lt"/> class.
      /// </summary>
      /// <exception cref="ArgumentException">EnumSelector type argument must be Enum</exception>
      public EnumSelector(Type enumType)
      {
         if (!enumType.IsEnum)
         {
            throw new ArgumentException("EnumSelector type argument must be Enum");
         }

         values = Enum.GetValues(enumType).Cast<Enum>().Select(x => new SelectableEnum(this) {Value = x}).ToList();
         
         foreach (SelectableEnum value in Values)
         {
            value.PropertyChanged += ValuePropertyChanged;
         }
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
      /// Gets the values.
      /// </summary>
      /// <value>The values.</value>
      public List<SelectableEnum> Values
      {
         get { return values; }
      }


      /// <summary>
      /// Gets the selected values.
      /// </summary>
      /// <value>The selected values.</value>
      public IEnumerable<SelectableEnum> SelectedValues
      {
         get
         {
            return values.Where(x => x.IsSelected);
         }
      }


      /// <summary>
      /// Gets or sets the selected values string.
      /// </summary>
      /// <value>The selected values string.</value>
      public string SelectedValuesString { get; private set; }


      #endregion


      #region Public methods


      /// <summary>
      /// Returns a <see cref="System.String"/> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String"/> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         var stringBuilder = new StringBuilder();

         foreach (SelectableEnum value in Values)
         {
            if (value.IsSelected)
            {
               stringBuilder.Append(", ").Append(value.Value);
            }
         }

         if (stringBuilder.Length > 1)
         {
            stringBuilder.Remove(0, 2);
         }

         return stringBuilder.ToString();
      }


      #endregion


      #region Private and protected methods


      /// <summary>
      /// Called when [property changed].
      /// </summary>
      /// <param name="propertyName">Name of the property.</param>
      protected void OnPropertyChanged(string propertyName)
      {
         PropertyChangedEventHandler handler = PropertyChanged;
         if (handler != null)
         {
            handler(this, new PropertyChangedEventArgs(propertyName));
         }
      }


      #endregion


      #region Event handlers


      /// <summary>
      /// Handles the PropertyChanged event of the value control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
      private void ValuePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         SelectedValuesString = ToString();
         OnPropertyChanged("SelectedValuesString");
      }


      #endregion
   }
}