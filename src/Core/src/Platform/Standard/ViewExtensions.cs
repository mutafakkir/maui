namespace Microsoft.Maui.Platform
{
	public static partial class ViewExtensions
	{
		public static void UpdateIsEnabled(this object platformView, IView view) { }

		public static void UpdateVisibility(this object platformView, IView view) { }

		public static void UpdateBackground(this object platformView, IView view) { }

		public static void UpdateClipsToBounds(this object platformView, IView view) { }

		public static void UpdateAutomationId(this object platformView, IView view) { }

		public static void UpdateClip(this object platformView, IView view) { }

		public static void UpdateShadow(this object platformView, IView view) { }
		public static void UpdateBorder(this object platformView, IView view) { }

		public static void UpdateOpacity(this object platformView, IView view) { }

		public static void UpdateSemantics(this object platformView, IView view) { }

		public static void UpdateFlowDirection(this object platformView, IView view) { }

		public static void UpdateTranslationX(this object platformView, IView view) { }

		public static void UpdateTranslationY(this object platformView, IView view) { }

		public static void UpdateScale(this object platformView, IView view) { }

		public static void UpdateRotation(this object platformView, IView view) { }

		public static void UpdateRotationX(this object platformView, IView view) { }

		public static void UpdateRotationY(this object platformView, IView view) { }

		public static void UpdateAnchorX(this object platformView, IView view) { }

		public static void UpdateAnchorY(this object platformView, IView view) { }

		public static void InvalidateMeasure(this object platformView, IView view) { }

		public static void UpdateWidth(this object platformView, IView view) { }

		public static void UpdateHeight(this object platformView, IView view) { }

		public static void UpdateMinimumHeight(this object platformView, IView view) { }

		public static void UpdateMaximumHeight(this object platformView, IView view) { }

		public static void UpdateMinimumWidth(this object platformView, IView view) { }

		public static void UpdateMaximumWidth(this object platformView, IView view) { }

		public static System.Threading.Tasks.Task<byte[]?> RenderAsPNG(this IView view)
			=> System.Threading.Tasks.Task.FromResult<byte[]?>(null);

		public static System.Threading.Tasks.Task<byte[]?> RenderAsJPEG(this IView view)
			=> System.Threading.Tasks.Task.FromResult<byte[]?>(null);
		internal static Graphics.Rectangle GetPlatformViewBounds(this IView view) => view.Frame;

		internal static System.Numerics.Matrix4x4 GetViewTransform(this IView view) => new System.Numerics.Matrix4x4();

		internal static Graphics.Rectangle GetBoundingBox(this IView view) => view.Frame;

		internal static object? GetParent(this object? view)
		{
			return null;
		}

		internal static IWindow? GetHostedWindow(this IView? view)
			=> null;
	}
}
