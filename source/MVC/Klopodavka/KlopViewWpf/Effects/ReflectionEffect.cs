#region Usings

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

#endregion

namespace KlopViewWpf.Effects
{
   /// <summary>An effect that applies a radial blur to the input.</summary>
   public class ReflectionEffect : ShaderEffect
   {
      #region Fields and Constants

      public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof (ReflectionEffect), 0);

      public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof (double), typeof (ReflectionEffect),
                                                                                              new PropertyMetadata(((0.5D)), PixelShaderConstantCallback(1)));

      public static readonly DependencyProperty ReflectionHeightProperty = DependencyProperty.Register("ReflectionHeight", typeof (double),
                                                                                                       typeof (ReflectionEffect),
                                                                                                       new PropertyMetadata(((0.5D)),
                                                                                                                            PixelShaderConstantCallback(0)));

      public static readonly DependencyProperty SpacingProperty =
         DependencyProperty.Register("Spacing", typeof (double), typeof (ReflectionEffect), new PropertyMetadata(0D, OnShadowSpacingChanged));

      public static readonly DependencyProperty SourceElementProperty =
         DependencyProperty.Register("SourceElement", typeof (FrameworkElement), typeof (ReflectionEffect), new PropertyMetadata(null, OnSourceElementChanged));

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="ReflectionEffect"/> class.
      /// </summary>
      public ReflectionEffect()
      {
         var pixelShader = new PixelShader
                              {
                                 UriSource = new Uri("/Klopodavka;component/Effects/reflection.ps", UriKind.Relative)
                              };
         PixelShader = pixelShader;

         UpdateShaderValue(InputProperty);
         UpdateShaderValue(ReflectionHeightProperty);
         UpdateShaderValue(OpacityProperty);
      }

      #endregion

      #region Public properties and indexers

      public double Spacing
      {
         get { return (double) GetValue(SpacingProperty); }
         set { SetValue(SpacingProperty, value); }
      }

      public FrameworkElement SourceElement
      {
         get { return (FrameworkElement) GetValue(SourceElementProperty); }
         set { SetValue(SourceElementProperty, value); }
      }

      public Brush Input
      {
         get { return ((Brush) (GetValue(InputProperty))); }
         set { SetValue(InputProperty, value); }
      }

      /// <summary>The height of the reflection.</summary>
      public double ReflectionHeight
      {
         get { return ((double) (GetValue(ReflectionHeightProperty))); }
         set { SetValue(ReflectionHeightProperty, value); }
      }

      /// <summary>Effect opacity.</summary>
      public double Opacity
      {
         get { return ((double) (GetValue(OpacityProperty))); }
         set { SetValue(OpacityProperty, value); }
      }

      #endregion

      #region Private and protected methods

      private static void OnShadowSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((ReflectionEffect) d).UpdatePadding();
      }

      private static void OnSourceElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((ReflectionEffect) d).UpdatePadding();
         var oldElement = e.OldValue as FrameworkElement;
         var newElement = e.NewValue as FrameworkElement;

         if (oldElement != null)
         {
            oldElement.SizeChanged -= ((ReflectionEffect) d).SourceElement_SizeChanged;
         }

         if (newElement != null)
         {
            newElement.SizeChanged += ((ReflectionEffect) d).SourceElement_SizeChanged;
         }
      }

      private void UpdatePadding()
      {
         PaddingBottom = Spacing + (SourceElement == null ? 0 : SourceElement.ActualHeight);
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the SizeChanged event of the SourceElement control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
      private void SourceElement_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         UpdatePadding();
      }

      #endregion
   }
}