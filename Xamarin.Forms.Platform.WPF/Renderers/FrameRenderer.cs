﻿using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using WThickness = System.Windows.Thickness;

namespace Xamarin.Forms.Platform.WPF
{
	public class FrameRenderer : ViewRenderer<Frame, Border>
	{
		VisualElement _currentView;
		readonly Border _rounding;
		readonly VisualBrush _mask;

		public FrameRenderer()
		{
			_rounding = new Border();
			_rounding.Background = Color.White.ToBrush();
			_rounding.SnapsToDevicePixels = true;
			var wb = new System.Windows.Data.Binding(nameof(Border.ActualWidth));
			wb.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
			{
				AncestorType = typeof(Border)
			};
			_rounding.SetBinding(Border.WidthProperty, wb);
			var hb = new System.Windows.Data.Binding(nameof(Border.ActualHeight));
			hb.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
			{
				AncestorType = typeof(Border)
			};
			_rounding.SetBinding(Border.HeightProperty, hb);
			_mask = new VisualBrush(_rounding);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null) // construct and SetNativeControl and suscribe control event
				{
					SetNativeControl(new Border());
				}

				// Update control property 
				UpdateContent();
				UpdateBorder();
				UpdateCornerRadius();
				UpdatePadding();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Frame.ContentProperty.PropertyName)
				UpdateContent();
			else if (e.PropertyName == Frame.BorderColorProperty.PropertyName)
				UpdateBorder();
			else if (e.PropertyName == Frame.HasShadowProperty.PropertyName)
				UpdateShadow();
			else if (e.PropertyName == Frame.CornerRadiusProperty.PropertyName)
				UpdateCornerRadius();
			else if (e.PropertyName == Button.PaddingProperty.PropertyName)
				UpdatePadding();
		}

		protected override void UpdateBackground()
		{
			Control.UpdateDependencyColor(Border.BackgroundProperty, Element.BackgroundColor);
		}

		void UpdateContent()
		{
			if (_currentView != null)
			{
				_currentView.Cleanup(); // cleanup old view
			}

			_currentView = Element.Content;
			Control.OpacityMask = _mask;
			Control.Child = _currentView != null ? Platform.GetOrCreateRenderer(_currentView).GetNativeElement() : null;
		}

		void UpdateBorder()
		{
			if (Element.BorderColor != Color.Default)
			{
				Control.UpdateDependencyColor(Border.BorderBrushProperty, Element.BorderColor);
				Control.BorderThickness = new System.Windows.Thickness(1);
			}
			else
			{
				Control.UpdateDependencyColor(Border.BorderBrushProperty, Color.Transparent);
				Control.BorderThickness = new System.Windows.Thickness(0);
			}
		}

		protected virtual UpdateShadow()
		{ 
			if (Element.HasShadow)
			{
				Control.Effect = new DropShadowEffect()
				{
					Color = Colors.Gray,
					Direction = 320,
					Opacity = 0.5,
					BlurRadius = 6,
					ShadowDepth = 2
				};
			}
			else if(Control.Effect is DropShadowEffect)
			{
				Control.Effect = null;
			} 

			// Verify that the background is correctly set when the shadow is changed
			UpdateBackground();
		}

		protected override UpdateBackground(){ 
			// Enforce that a background color is set when the shadow is enabled
			// to ensure, that the shadow is visible
			if(Element.HasShadow && Element.BackgroundColor == Color.Transparent)
			{
				Control.UpdateDependencyColor(WControl.BackgroundProperty, Color.White);
			}
			else
			{
				base.UpdateBackground();
			}
		}

		void UpdateCornerRadius()
		{
			Control.CornerRadius = new System.Windows.CornerRadius(Element.CornerRadius >= 0 ? Element.CornerRadius : 0);
			_rounding.CornerRadius = Control.CornerRadius;
		}

		void UpdatePadding()
		{
			Control.Padding = new WThickness(
				Element.Padding.Left,
				Element.Padding.Top,
				Element.Padding.Right,
				Element.Padding.Bottom);
		}
	}
}
