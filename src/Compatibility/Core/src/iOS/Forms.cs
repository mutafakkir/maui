﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Animations;
using Microsoft.Extensions.DependencyInjection;

#if __MOBILE__
using ObjCRuntime;
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using TNativeView = UIKit.UIView;
#else
using AppKit;
using Microsoft.Maui.Controls.Compatibility.Platform.MacOS;
using TNativeView = AppKit.NSView;
#endif

namespace Microsoft.Maui.Controls.Compatibility
{
	public struct InitializationOptions
	{
		public InitializationFlags Flags;
	}

	public static class Forms
	{
		internal static IMauiContext MauiContext { get; private set; }

		public static bool IsInitialized { get; private set; }

#if __MOBILE__
		static bool? s_isiOS11OrNewer;
		static bool? s_isiOS12OrNewer;
		static bool? s_isiOS13OrNewer;
		static bool? s_isiOS14OrNewer;
		static bool? s_isiOS15OrNewer;
		static bool? s_respondsTosetNeedsUpdateOfHomeIndicatorAutoHidden;

		internal static bool IsiOS11OrNewer
		{
			get
			{
				if (!s_isiOS11OrNewer.HasValue)
					s_isiOS11OrNewer = OperatingSystem.IsIOSVersionAtLeast(11, 0);
				return s_isiOS11OrNewer.Value;
			}
		}

		internal static bool IsiOS12OrNewer
		{
			get
			{
				if (!s_isiOS12OrNewer.HasValue)
					s_isiOS12OrNewer = OperatingSystem.IsIOSVersionAtLeast(12, 0);
				return s_isiOS12OrNewer.Value;
			}
		}

		internal static bool IsiOS13OrNewer
		{
			get
			{
				if (!s_isiOS13OrNewer.HasValue)
					s_isiOS13OrNewer = OperatingSystem.IsIOSVersionAtLeast(13, 0);
				return s_isiOS13OrNewer.Value;
			}
		}

		internal static bool IsiOS14OrNewer
		{
			get
			{
				if (!s_isiOS14OrNewer.HasValue)
					s_isiOS14OrNewer = OperatingSystem.IsIOSVersionAtLeast(14, 0);
				return s_isiOS14OrNewer.Value;
			}
		}

		internal static bool IsiOS15OrNewer
		{
			get
			{
				if (!s_isiOS15OrNewer.HasValue)
					s_isiOS15OrNewer = OperatingSystem.IsIOSVersionAtLeast(15, 0);
				return s_isiOS15OrNewer.Value;
			}
		}

		// Once we get essentials/cg converted to using startup.cs
		// we will delete all the renderer code inside this file
		internal static void RenderersRegistered()
		{
			IsInitializedRenderers = true;
		}

		internal static bool RespondsToSetNeedsUpdateOfHomeIndicatorAutoHidden
		{
			get
			{
				if (!s_respondsTosetNeedsUpdateOfHomeIndicatorAutoHidden.HasValue)
					s_respondsTosetNeedsUpdateOfHomeIndicatorAutoHidden = new UIViewController().RespondsToSelector(new ObjCRuntime.Selector("setNeedsUpdateOfHomeIndicatorAutoHidden"));
				return s_respondsTosetNeedsUpdateOfHomeIndicatorAutoHidden.Value;
			}
		}
#else
		static bool? s_isSierraOrNewer;

		internal static bool IsSierraOrNewer
		{
			get
			{
				if (!s_isSierraOrNewer.HasValue)
					s_isSierraOrNewer = NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(10, 12, 0));
				return s_isSierraOrNewer.Value;
			}
		}

		static bool? s_isHighSierraOrNewer;

		internal static bool IsHighSierraOrNewer
		{
			get
			{
				if (!s_isHighSierraOrNewer.HasValue)
					s_isHighSierraOrNewer = NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(10, 13, 0));
				return s_isHighSierraOrNewer.Value;
			}
		}

		static bool? s_isMojaveOrNewer;

		internal static bool IsMojaveOrNewer
		{
			get
			{
				if (!s_isMojaveOrNewer.HasValue)
					s_isMojaveOrNewer = NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(10, 14, 0));
				return s_isMojaveOrNewer.Value;
			}
		}

#endif

		public static bool IsInitializedRenderers { get; private set; }

		public static void Init(IActivationState activationState, InitializationOptions? options = null) =>
			SetupInit(activationState.Context, options);

		static void SetupInit(IMauiContext context, InitializationOptions? maybeOptions = null)
		{
			MauiContext = context;

			Microsoft.Maui.Controls.Internals.Registrar.RegisterRendererToHandlerShim(RendererToHandlerShim.CreateShim);

			Application.AccentColor = Color.FromRgba(50, 79, 133, 255);

#if __MOBILE__
			Device.SetFlowDirection(UIApplication.SharedApplication.UserInterfaceLayoutDirection.ToFlowDirection());
#else
			if (!IsInitialized)
			{
				// Only need to do this once
				// Subscribe to notifications in OS Theme changes
				NSDistributedNotificationCenter.GetDefaultCenter().AddObserver((NSString)"AppleInterfaceThemeChangedNotification", (n) =>
				{
					var interfaceStyle = NSUserDefaults.StandardUserDefaults.StringForKey("AppleInterfaceStyle");

					var aquaAppearance = NSAppearance.GetAppearance(interfaceStyle == "Dark" ? NSAppearance.NameDarkAqua : NSAppearance.NameAqua);
					NSApplication.SharedApplication.Appearance = aquaAppearance;

					Application.Current?.TriggerThemeChanged(new AppThemeChangedEventArgs(interfaceStyle == "Dark" ? OSAppTheme.Dark : OSAppTheme.Light));
				});
			}

			Device.SetFlowDirection(NSApplication.SharedApplication.UserInterfaceLayoutDirection.ToFlowDirection());

			if (IsMojaveOrNewer)
			{
				var interfaceStyle = NSUserDefaults.StandardUserDefaults.StringForKey("AppleInterfaceStyle");
				var aquaAppearance = NSAppearance.GetAppearance(interfaceStyle == "Dark" ? NSAppearance.NameDarkAqua : NSAppearance.NameAqua);
				NSApplication.SharedApplication.Appearance = aquaAppearance;
			}
#endif
			var platformServices = new IOSPlatformServices();

			Device.PlatformServices = platformServices;

			if (maybeOptions?.Flags.HasFlag(InitializationFlags.SkipRenderers) != true)
				RegisterCompatRenderers(context);

			ExpressionSearch.Default = new iOSExpressionSearch();

			IsInitialized = true;
		}

		internal static void RegisterCompatRenderers(IMauiContext context)
		{
			if (!IsInitializedRenderers)
			{
				IsInitializedRenderers = true;

				// Only need to do this once
				Controls.Internals.Registrar.RegisterAll(new[]
				{
					typeof(ExportRendererAttribute),
					typeof(ExportCellAttribute),
					typeof(ExportImageSourceHandlerAttribute),
					typeof(ExportFontAttribute)
				}, context?.Services?.GetService<IFontRegistrar>());
			}
		}

		public static event EventHandler<ViewInitializedEventArgs> ViewInitialized;

		internal static void SendViewInitialized(this VisualElement self, TNativeView nativeView)
		{
			ViewInitialized?.Invoke(self, new ViewInitializedEventArgs { View = self, NativeView = nativeView });
		}

		class iOSExpressionSearch : ExpressionVisitor, IExpressionSearch
		{
			List<object> _results;
			Type _targetType;

			public List<T> FindObjects<T>(Expression expression) where T : class
			{
				_results = new List<object>();
				_targetType = typeof(T);
				Visit(expression);
				return _results.Select(o => o as T).ToList();
			}

			protected override Expression VisitMember(MemberExpression node)
			{
				if (node.Expression is ConstantExpression && node.Member is FieldInfo)
				{
					var container = ((ConstantExpression)node.Expression).Value;
					var value = ((FieldInfo)node.Member).GetValue(container);

					if (_targetType.IsInstanceOfType(value))
						_results.Add(value);
				}
				return base.VisitMember(node);
			}
		}

		class IOSPlatformServices : IPlatformServices
		{
			public void StartTimer(TimeSpan interval, Func<bool> callback)
			{
				NSTimer timer = NSTimer.CreateRepeatingTimer(interval, t =>
				{
					if (!callback())
						t.Invalidate();
				});
				NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);
			}

			HttpClient GetHttpClient()
			{
				var proxy = CoreFoundation.CFNetwork.GetSystemProxySettings();
				var handler = new HttpClientHandler();
				if (!string.IsNullOrEmpty(proxy.HTTPProxy))
				{
					handler.Proxy = CoreFoundation.CFNetwork.GetDefaultProxy();
					handler.UseProxy = true;
				}
				return new HttpClient(handler);
			}

			public SizeRequest GetPlatformSize(VisualElement view, double widthConstraint, double heightConstraint)
			{
				return Platform.iOS.Platform.GetNativeSize(view, widthConstraint, heightConstraint);
			}

			public OSAppTheme RequestedTheme
			{
				get
				{
#if __IOS__ || __TVOS__
					if (!IsiOS13OrNewer)
						return OSAppTheme.Unspecified;
					var uiStyle = GetCurrentUIViewController()?.TraitCollection?.UserInterfaceStyle ??
						UITraitCollection.CurrentTraitCollection.UserInterfaceStyle;

					switch (uiStyle)
					{
						case UIUserInterfaceStyle.Light:
							return OSAppTheme.Light;
						case UIUserInterfaceStyle.Dark:
							return OSAppTheme.Dark;
						default:
							return OSAppTheme.Unspecified;
					};
#else
					return AppearanceIsDark() ? OSAppTheme.Dark : OSAppTheme.Light;
#endif
				}
			}

#if __MACOS__
			bool AppearanceIsDark()
			{
				if (IsMojaveOrNewer)
				{
					var appearance = NSApplication.SharedApplication.EffectiveAppearance;
					var matchedAppearance = appearance.FindBestMatch(new string[] { NSAppearance.NameAqua, NSAppearance.NameDarkAqua });

					return matchedAppearance == NSAppearance.NameDarkAqua;
				}
				else
				{
					return false;
				}
			}
#endif

#if __IOS__ || __TVOS__

			static UIViewController GetCurrentUIViewController() =>
				GetCurrentViewController(false);

			static UIViewController GetCurrentViewController(bool throwIfNull = true)
			{
				UIViewController viewController = null;

				var window = UIApplication.SharedApplication.GetKeyWindow();

				if (window != null && window.WindowLevel == UIWindowLevel.Normal)
					viewController = window.RootViewController;

				if (viewController == null)
				{
					window = UIApplication.SharedApplication
						.Windows
						.OrderByDescending(w => w.WindowLevel)
						.FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

					if (window == null && throwIfNull)
						throw new InvalidOperationException("Could not find current view controller.");
					else
						viewController = window?.RootViewController;
				}

				while (viewController?.PresentedViewController != null)
					viewController = viewController.PresentedViewController;

				if (throwIfNull && viewController == null)
					throw new InvalidOperationException("Could not find current view controller.");

				return viewController;
			}
#endif
		}
	}
}
