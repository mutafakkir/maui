#if IOS || MACCATALYST
using PlatformView = UIKit.UIView;
#elif ANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif NETSTANDARD
using PlatformView = System.Object;
#endif
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Handlers;
using System.Threading.Tasks;

namespace Microsoft.Maui
{
	static class ElementHandlerExtensions
	{
		public static PlatformView ToPlatform(this IElementHandler elementHandler) =>
			(elementHandler.VirtualView?.ToPlatform() as PlatformView) ??
				throw new InvalidOperationException($"Unable to convert {elementHandler} to {typeof(PlatformView)}");

		public static IServiceProvider GetServiceProvider(this IElementHandler handler)
		{
			var context = handler.MauiContext ??
				throw new InvalidOperationException($"Unable to find the context. The {nameof(ElementHandler.MauiContext)} property should have been set by the host.");

			var services = context?.Services ??
				throw new InvalidOperationException($"Unable to find the service provider. The {nameof(ElementHandler.MauiContext)} property should have been set by the host.");

			return services;
		}

		public static T? GetService<T>(this IElementHandler handler, Type type)
		{
			var services = handler.GetServiceProvider();

			var service = services.GetService(type);

			return (T?)service;
		}

		public static T? GetService<T>(this IElementHandler handler)
		{
			var services = handler.GetServiceProvider();

			var service = services.GetService<T>();

			return service;
		}

		public static T GetRequiredService<T>(this IElementHandler handler, Type type)
			where T : notnull
		{
			var services = handler.GetServiceProvider();

			var service = services.GetRequiredService(type);

			return (T)service;
		}

		public static T GetRequiredService<T>(this IElementHandler handler)
			where T : notnull
		{
			var services = handler.GetServiceProvider();

			var service = services.GetRequiredService<T>();

			return service;
		}

		public static async Task<T> InvokeAsync<T>(this IElementHandler handler, string commandName,
			TaskCompletionSource<T> args)
		{
			handler?.Invoke(commandName, args);
			return await args.Task;
		}
	}
}