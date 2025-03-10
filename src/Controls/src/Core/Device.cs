using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Essentials;

namespace Microsoft.Maui.Controls
{
	/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="Type[@FullName='Microsoft.Maui.Controls.Device']/Docs" />
	public static class Device
	{
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='iOS']/Docs" />
		public const string iOS = "iOS";
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='Android']/Docs" />
		public const string Android = "Android";
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='UWP']/Docs" />
		public const string UWP = "UWP";
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='macOS']/Docs" />
		public const string macOS = "macOS";
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='GTK']/Docs" />
		public const string GTK = "GTK";
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='Tizen']/Docs" />
		public const string Tizen = "Tizen";
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='WPF']/Docs" />
		public const string WPF = "WPF";
		public const string MacCatalyst = "MacCatalyst";
		public const string tvOS = "tvOS";

		static IPlatformServices s_platformServices;

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='Idiom']/Docs" />
		public static TargetIdiom Idiom
		{
			get
			{
				var idiom = DeviceInfo.Idiom;
				if (idiom == DeviceIdiom.Tablet)
					return TargetIdiom.Tablet;
				if (idiom == DeviceIdiom.Phone)
					return TargetIdiom.Phone;
				if (idiom == DeviceIdiom.Desktop)
					return TargetIdiom.Desktop;
				if (idiom == DeviceIdiom.TV)
					return TargetIdiom.TV;
				if (idiom == DeviceIdiom.Watch)
					return TargetIdiom.Watch;
				return TargetIdiom.Unsupported;
			}
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='RuntimePlatform']/Docs" />
		public static string RuntimePlatform => DeviceInfo.Platform.ToString();

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='SetFlowDirection']/Docs" />
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetFlowDirection(FlowDirection value) => FlowDirection = value;
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='FlowDirection']/Docs" />
		public static FlowDirection FlowDirection { get; internal set; }

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='PlatformServices']/Docs" />
		internal static IPlatformServices PlatformServices
		{
			get
			{
				if (s_platformServices == null)
					throw new InvalidOperationException($"You must call Microsoft.Maui.Controls.Compatibility.Forms.Init(); prior to using this property ({nameof(PlatformServices)}).");
				return s_platformServices;
			}
			set
			{
				s_platformServices = value;
				if (s_platformServices != null)
					Application.Current?.PlatformServicesSet();
			}
		}

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='IsInvokeRequired']/Docs" />
		public static bool IsInvokeRequired =>
			Application.Current.FindDispatcher().IsDispatchRequired;

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='BeginInvokeOnMainThread']/Docs" />
		public static void BeginInvokeOnMainThread(Action action) =>
			Application.Current.FindDispatcher().Dispatch(action);

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='InvokeOnMainThreadAsync']/Docs" />
		public static Task<T> InvokeOnMainThreadAsync<T>(Func<T> func) =>
			Application.Current.FindDispatcher().DispatchAsync(func);

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='InvokeOnMainThreadAsync'][0]/Docs" />
		public static Task InvokeOnMainThreadAsync(Action action) =>
			Application.Current.FindDispatcher().DispatchAsync(action);

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='InvokeOnMainThreadAsync']/Docs" />
		public static Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask) =>
			Application.Current.FindDispatcher().DispatchAsync(funcTask);

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='InvokeOnMainThreadAsync']/Docs" />
		public static Task InvokeOnMainThreadAsync(Func<Task> funcTask) =>
			Application.Current.FindDispatcher().DispatchAsync(funcTask);

		//[Obsolete("Use BindableObject.Dispatcher instead.")]
		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='GetMainThreadSynchronizationContextAsync']/Docs" />
		public static Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync() =>
			Application.Current.FindDispatcher().GetSynchronizationContextAsync();

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='GetNamedSize'][1]/Docs" />
		public static double GetNamedSize(NamedSize size, Element targetElement)
		{
			return GetNamedSize(size, targetElement.GetType());
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='GetNamedSize'][0]/Docs" />
		public static double GetNamedSize(NamedSize size, Type targetElementType)
		{
			return GetNamedSize(size, targetElementType, false);
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='StartTimer']/Docs" />
		public static void StartTimer(TimeSpan interval, Func<bool> callback)
		{
			PlatformServices.StartTimer(interval, callback);
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/Device.xml" path="//Member[@MemberName='GetNamedSize'][2]/Docs" />
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes) =>
			DependencyService.Get<IFontNamedSizeService>()?.GetNamedSize(size, targetElementType, useOldSizes) ??
			throw new NotImplementedException("The current platform does not implement the IFontNamedSizeService dependency service.");

		public static class Styles
		{
			public static readonly string TitleStyleKey = "TitleStyle";

			public static readonly string SubtitleStyleKey = "SubtitleStyle";

			public static readonly string BodyStyleKey = "BodyStyle";

			public static readonly string ListItemTextStyleKey = "ListItemTextStyle";

			public static readonly string ListItemDetailTextStyleKey = "ListItemDetailTextStyle";

			public static readonly string CaptionStyleKey = "CaptionStyle";

			public static readonly Style TitleStyle = new Style(typeof(Label)) { BaseResourceKey = TitleStyleKey };

			public static readonly Style SubtitleStyle = new Style(typeof(Label)) { BaseResourceKey = SubtitleStyleKey };

			public static readonly Style BodyStyle = new Style(typeof(Label)) { BaseResourceKey = BodyStyleKey };

			public static readonly Style ListItemTextStyle = new Style(typeof(Label)) { BaseResourceKey = ListItemTextStyleKey };

			public static readonly Style ListItemDetailTextStyle = new Style(typeof(Label)) { BaseResourceKey = ListItemDetailTextStyleKey };

			public static readonly Style CaptionStyle = new Style(typeof(Label)) { BaseResourceKey = CaptionStyleKey };
		}
	}
}
