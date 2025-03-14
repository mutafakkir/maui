﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using WPanel = Microsoft.UI.Xaml.Controls.Panel;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
using WWindow = Microsoft.UI.Xaml.Window;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Handlers;
using WAppBarButton = Microsoft.UI.Xaml.Controls.AppBarButton;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.NavigationPage)]
	public partial class NavigationPageTests : HandlerTestBase
	{
		public bool IsBackButtonVisible(IElementHandler handler) =>
			IsBackButtonVisible(handler.MauiContext);

		public bool IsBackButtonVisible(IMauiContext mauiContext)
		{
			var navView = GetMauiNavigationView(mauiContext);
			return navView.IsBackButtonVisible == UI.Xaml.Controls.NavigationViewBackButtonVisible.Visible;
		}

		public bool IsNavigationBarVisible(IElementHandler handler) =>
			IsNavigationBarVisible(handler.MauiContext);

		public bool IsNavigationBarVisible(IMauiContext mauiContext)
		{
			var navView = GetMauiNavigationView(mauiContext);
			var header = navView.Header as WFrameworkElement;
			return header.Visibility == UI.Xaml.Visibility.Visible;
		}

		public bool ToolbarItemsMatch(
			IElementHandler handler,
			params ToolbarItem[] toolbarItems)
		{
			var navView = (RootNavigationView)GetMauiNavigationView(handler.MauiContext);
			MauiToolbar windowHeader = (MauiToolbar)navView.Header;

			Assert.Equal(toolbarItems.Length, windowHeader.CommandBar.PrimaryCommands.Count);
			for (var i = 0; i < toolbarItems.Length; i++)
			{
				ToolbarItem toolbarItem = toolbarItems[i];
				var primaryCommand = ((WAppBarButton)windowHeader.CommandBar.PrimaryCommands[i]);
				Assert.Equal(toolbarItem, primaryCommand.DataContext);
			}

			return true;
		}

		MauiToolbar GetPlatformToolbar(IElementHandler handler)
		{
			var navView = (RootNavigationView)GetMauiNavigationView(handler.MauiContext);
			MauiToolbar windowHeader = (MauiToolbar)navView.Header;
			return windowHeader;
		}

		string GetToolbarTitle(IElementHandler handler) =>
			GetPlatformToolbar(handler).Title;
	}
}
