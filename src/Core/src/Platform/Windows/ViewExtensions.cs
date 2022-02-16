#nullable enable
using System;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using WFlowDirection = Microsoft.UI.Xaml.FlowDirection;
using WinPoint = Windows.Foundation.Point;
using Microsoft.Maui.Primitives;

namespace Microsoft.Maui.Platform
{
	public static partial class ViewExtensions
	{
		static Canvas? BackgroundLayer;

		public static void TryMoveFocus(this FrameworkElement nativeView, FocusNavigationDirection direction)
		{
			if (nativeView?.XamlRoot?.Content is UIElement elem)
				FocusManager.TryMoveFocus(direction, new FindNextElementOptions { SearchRoot = elem });
		}

		public static void UpdateIsEnabled(this FrameworkElement nativeView, IView view) =>
			(nativeView as Control)?.UpdateIsEnabled(view.IsEnabled);

		public static void UpdateInputTransparent(this FrameworkElement nativeView, IView view)
		{
			var panel = nativeView as Panel;

			if (NeedsBackgroundLayer(view))
			{
				nativeView.IsHitTestVisible = true;

				if (panel !=null)
					panel.AddBackgroundLayer(view);
			}
			else
			{
				if (panel != null)
					panel.RemoveBackgroundLayer(view);

				nativeView.IsHitTestVisible = view.IsEnabled && !view.InputTransparent;

				if (!nativeView.IsHitTestVisible)
				{
					return;
				}

				// If this Panel's background brush is null, the UWP considers it transparent to hit testing (even 
				// when IsHitTestVisible is true). So we have to explicitly set a background brush to make it show up
				// in hit testing. 
				if (view is ILayout && panel != null && panel.Background == null)
				{
					panel.Background = new SolidColorBrush(UI.Colors.Transparent);
				}
			}
		}

		internal static bool NeedsBackgroundLayer(IView view)
		{
			if (view is not ILayout layout)
			{
				return false;
			}

			if (layout.IsEnabled && layout.InputTransparent && !layout.CascadeInputTransparent)
			{
				return true;
			}

			return false;
		}

		internal static void AddBackgroundLayer(this Panel nativeControl, IView view)
		{
			if (BackgroundLayer != null)
			{
				return;
			}

			// In WinUI, once a control has hit testing disabled, all of its child controls
			// also have hit testing disabled. The exception is a Panel with its 
			// Background Brush set to `null`; the Panel will be invisible to hit testing, but its
			// children will work just fine. 

			// In order to handle the situation where we need the layout to be invisible to hit testing,
			// the child controls to be visible to hit testing, *and* we need to support non-null
			// background brushes, we insert another empty Panel which is invisible to hit testing; that
			// Panel will be our Background color

			BackgroundLayer = new Canvas { IsHitTestVisible = false };
			nativeControl.Children.Insert(0, BackgroundLayer);

			nativeControl.UpdateBackground(view);
		}

		internal static void RemoveBackgroundLayer(this Panel nativeControl, IView view)
		{
			if (BackgroundLayer == null)
			{
				return;
			}

			nativeControl.Children.Remove(BackgroundLayer);
			BackgroundLayer = null;

			nativeControl.UpdateBackground(view);
		}

		public static void UpdateVisibility(this FrameworkElement nativeView, IView view)
		{
			double opacity = view.Opacity;
			var wasCollapsed = nativeView.Visibility == UI.Xaml.Visibility.Collapsed;

			switch (view.Visibility)
			{
				case Visibility.Visible:
					nativeView.Opacity = opacity;
					nativeView.Visibility = UI.Xaml.Visibility.Visible;
					break;
				case Visibility.Hidden:
					nativeView.Opacity = 0;
					nativeView.Visibility = UI.Xaml.Visibility.Visible;
					break;
				case Visibility.Collapsed:
					nativeView.Opacity = opacity;
					nativeView.Visibility = UI.Xaml.Visibility.Collapsed;
					break;
			}

			if (view.Visibility != Visibility.Collapsed && wasCollapsed)
			{
				// We may need to force the parent layout (if any) to re-layout to accomodate the new size
				(nativeView.Parent as FrameworkElement)?.InvalidateMeasure();
			}
		}

		public static void UpdateClip(this FrameworkElement nativeView, IView view)
		{
			if (nativeView is WrapperView wrapper)
			{
				wrapper.Clip = view.Clip;
			}
		}

		public static void UpdateShadow(this FrameworkElement nativeView, IView view)
		{
			if (nativeView is WrapperView wrapper)
			{
				wrapper.Shadow = view.Shadow;
			}
		}

		public static void UpdateBorder(this FrameworkElement nativeView, IView view)
		{
			var border = (view as IBorder)?.Border;
			if (nativeView is WrapperView wrapperView)
				wrapperView.Border = border;
		}

		public static void UpdateOpacity(this FrameworkElement nativeView, IView view)
		{
			nativeView.Opacity = view.Visibility == Visibility.Hidden ? 0 : view.Opacity;
		}

		public static void UpdateBackground(this ContentPanel nativeView, IBorderStroke border) 
		{
			var hasBorder = border.Shape != null && border.Stroke != null;

			if (hasBorder)
			{
				nativeView?.UpdateBorderBackground(border);
			}
			else if(border is IView v)
			{
				nativeView?.UpdateNativeViewBackground(v);
			}
		}

		public static void UpdateBackground(this FrameworkElement nativeView, IView view)
		{
			nativeView?.UpdateNativeViewBackground(view);
		}

		public static WFlowDirection ToNative(this FlowDirection flowDirection)
		{
			if (flowDirection == FlowDirection.RightToLeft)
				return WFlowDirection.RightToLeft;
			else if (flowDirection == FlowDirection.LeftToRight)
				return WFlowDirection.LeftToRight;

			throw new InvalidOperationException($"Invalid FlowDirection: {flowDirection}");
		}

		public static void UpdateFlowDirection(this FrameworkElement nativeView, IView view)
		{
			var flowDirection = view.FlowDirection;

			if (flowDirection == FlowDirection.MatchParent ||
				view.FlowDirection == FlowDirection.MatchParent)
			{
				flowDirection = view?.Handler?.MauiContext?.GetFlowDirection()
					?? FlowDirection.LeftToRight;
			}
			if (flowDirection == FlowDirection.MatchParent)
			{
				flowDirection = FlowDirection.LeftToRight;
			}

			nativeView.FlowDirection = flowDirection.ToNative();
		}

		public static void UpdateAutomationId(this FrameworkElement nativeView, IView view) =>
			AutomationProperties.SetAutomationId(nativeView, view.AutomationId);

		public static void UpdateSemantics(this FrameworkElement nativeView, IView view)
		{
			var semantics = view.Semantics;

			if (semantics == null)
				return;

			AutomationProperties.SetName(nativeView, semantics.Description);
			AutomationProperties.SetHelpText(nativeView, semantics.Hint);
			AutomationProperties.SetHeadingLevel(nativeView, (UI.Xaml.Automation.Peers.AutomationHeadingLevel)((int)semantics.HeadingLevel));
		}

		internal static void UpdateProperty(this FrameworkElement nativeControl, DependencyProperty property, Color color)
		{
			if (color.IsDefault())
				nativeControl.ClearValue(property);
			else
				nativeControl.SetValue(property, color.ToNative());
		}

		internal static void UpdateProperty(this FrameworkElement nativeControl, DependencyProperty property, object? value)
		{
			if (value == null)
				nativeControl.ClearValue(property);
			else
				nativeControl.SetValue(property, value);
		}

		public static void InvalidateMeasure(this FrameworkElement nativeView, IView view)
		{
			nativeView.InvalidateMeasure();
		}

		public static void UpdateWidth(this FrameworkElement nativeView, IView view)
		{
			// WinUI uses NaN for "unspecified", so as long as we're using NaN for unspecified on the xplat side, 
			// we can just propagate the value straight through
			nativeView.Width = view.Width;
		}

		public static void UpdateHeight(this FrameworkElement nativeView, IView view)
		{
			// WinUI uses NaN for "unspecified", so as long as we're using NaN for unspecified on the xplat side, 
			// we can just propagate the value straight through
			nativeView.Height = view.Height;
		}

		public static void UpdateMinimumHeight(this FrameworkElement nativeView, IView view)
		{
			var minHeight = view.MinimumHeight;

			if (Dimension.IsMinimumSet(minHeight))
			{
				// We only use the minimum value if it's been explicitly set; otherwise, leave it alone
				// because the platform/theme may have a minimum height for this control
				nativeView.MinHeight = minHeight;
			}
		}

		public static void UpdateMinimumWidth(this FrameworkElement nativeView, IView view)
		{
			var minWidth = view.MinimumWidth;

			if (Dimension.IsMinimumSet(minWidth))
			{
				// We only use the minimum value if it's been explicitly set; otherwise, leave it alone
				// because the platform/theme may have a minimum width for this control
				nativeView.MinWidth = minWidth;
			}
		}

		public static void UpdateMaximumHeight(this FrameworkElement nativeView, IView view)
		{
			nativeView.MaxHeight = view.MaximumHeight;
		}

		public static void UpdateMaximumWidth(this FrameworkElement nativeView, IView view)
		{
			nativeView.MaxWidth = view.MaximumWidth;
		}

		internal static void UpdateBorderBackground(this FrameworkElement nativeView, IBorderStroke border)
		{

			if(border is IView v)
			(nativeView as ContentPanel)?.UpdateBackground(v.Background);

			if (nativeView is Control control)
				control.UpdateBackground((Paint?)null);
			else if (nativeView is Border b)
				b.UpdateBackground(null);
			else if (nativeView is Panel panel)
				panel.UpdateBackground(null);
		}

		internal static void UpdateNativeViewBackground(this FrameworkElement nativeView, IView view)
		{
			(nativeView as ContentPanel)?.UpdateBackground(null);

			if (nativeView is Control control)
				control.UpdateBackground(view.Background);
			else if (nativeView is Border border)
				border.UpdateBackground(view.Background);
			else if (nativeView is Panel panel)
				panel.UpdateBackground(view.Background);
		}

		public static async Task<byte[]?> RenderAsPNG(this IView view)
		{
			var nativeView = view?.ToPlatform();
			if (nativeView == null)
				return null;

			return await nativeView.RenderAsPNG();
		}

		public static async Task<byte[]?> RenderAsJPEG(this IView view)
		{
			var nativeView = view?.ToPlatform();
			if (nativeView == null)
				return null;

			return await nativeView.RenderAsJPEG();
		}

		public static Task<byte[]?> RenderAsPNG(this FrameworkElement view) =>  Task.FromResult<byte[]?>(null);

		public static Task<byte[]?> RenderAsJPEG(this FrameworkElement view) =>  Task.FromResult<byte[]?>(null);

		internal static Matrix4x4 GetViewTransform(this IView view)
		{
			var nativeView = view?.ToPlatform();
			if (nativeView == null)
				return new Matrix4x4();
			return GetViewTransform(nativeView);
		}

		internal static Matrix4x4 GetViewTransform(this FrameworkElement element)
		{
			var root = element?.Parent as UIElement;
			if (root == null)
				return new Matrix4x4();
			var offset = element?.TransformToVisual(root) as MatrixTransform;
			if (offset == null)
				return new Matrix4x4();
			Matrix matrix = offset.Matrix;
			return new Matrix4x4()
			{
				M11 = (float)matrix.M11,
				M12 = (float)matrix.M12,
				M21 = (float)matrix.M21,
				M22 = (float)matrix.M22,
				Translation = new Vector3((float)matrix.OffsetX, (float)matrix.OffsetY, 0)
			};
		}

		internal static Rectangle GetNativeViewBounds(this IView view)
		{
			var nativeView = view?.ToPlatform();
			if (nativeView != null)
				return nativeView.GetNativeViewBounds();
			return new Rectangle();
		}

		internal static Rectangle GetNativeViewBounds(this FrameworkElement nativeView)
		{
			if (nativeView == null)
				return new Rectangle();

			var root = nativeView.XamlRoot;
			var offset = nativeView.TransformToVisual(root.Content) as UI.Xaml.Media.MatrixTransform;
			if (offset != null)
				return new Rectangle(offset.Matrix.OffsetX, offset.Matrix.OffsetY, nativeView.ActualWidth, nativeView.ActualHeight);

			return new Rectangle();
		}

		internal static Graphics.Rectangle GetBoundingBox(this IView view) 
			=> view.ToPlatform().GetBoundingBox();

		internal static Graphics.Rectangle GetBoundingBox(this FrameworkElement? nativeView)
		{
			if (nativeView == null)
				return new Rectangle();

			var rootView = nativeView.XamlRoot.Content;
			if (nativeView == rootView)
			{
				if (rootView is not FrameworkElement el)
					return new Rectangle();

				return new Rectangle(0, 0, el.ActualWidth, el.ActualHeight);
			}


			var topLeft = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint());
			var topRight = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint(nativeView.ActualWidth, 0));
			var bottomLeft = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint(0, nativeView.ActualHeight));
			var bottomRight = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint(nativeView.ActualWidth, nativeView.ActualHeight));


			var x1 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Min();
			var x2 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Max();
			var y1 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Min();
			var y2 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Max();
			return new Rectangle(x1, y1, x2 - x1, y2 - y1);
		}

		internal static DependencyObject? GetParent(this FrameworkElement? view)
		{
			return view?.Parent;
		}

		internal static DependencyObject? GetParent(this DependencyObject? view)
		{
			if (view is FrameworkElement pv)
				return pv.Parent;

			return null;
		}
	}
}
