using System;
using Microsoft.UI.Xaml.Controls;
using WThickness = Microsoft.UI.Xaml.Thickness;

namespace Microsoft.Maui.Handlers
{
	public partial class WindowHandler : ElementHandler<IWindow, UI.Xaml.Window>
	{
		RootPanel? _rootPanel = null;

		protected override void ConnectHandler(UI.Xaml.Window platformView)
		{
			base.ConnectHandler(platformView);

			if (_rootPanel == null)
			{
				// TODO WINUI should this be some other known constant or via some mechanism? Or done differently?
				MauiWinUIApplication.Current.Resources.TryGetValue("MauiRootContainerStyle", out object? style);

				_rootPanel = new RootPanel
				{
					Style = style as UI.Xaml.Style
				};
			}

			platformView.Content = _rootPanel;
		}

		protected override void DisconnectHandler(UI.Xaml.Window platformView)
		{			
			MauiContext
				?.GetNavigationRootManager()
				?.Disconnect();			

			_rootPanel?.Children?.Clear();
			platformView.Content = null;

			base.DisconnectHandler(platformView);
		}

		public static void MapTitle(IWindowHandler handler, IWindow window) =>
			handler.PlatformView?.UpdateTitle(window);

		public static void MapContent(IWindowHandler handler, IWindow window)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			var windowManager = handler.MauiContext.GetNavigationRootManager();
			windowManager.Connect(handler.VirtualView.Content);
			var rootPanel = handler.PlatformView.Content as Panel;

			if (rootPanel == null)
				return;

			rootPanel.Children.Clear();
			rootPanel.Children.Add(windowManager.RootView);

			if (window.VisualDiagnosticsOverlay != null)
				window.VisualDiagnosticsOverlay.Initialize();
		}

		public static void MapToolbar(IWindowHandler handler, IWindow view)
		{
			if (view is IToolbarElement tb)
				ViewHandler.MapToolbar(handler, tb);
		}
	}
}