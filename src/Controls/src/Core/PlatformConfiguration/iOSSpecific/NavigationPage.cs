
namespace Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific
{
	using FormsElement = Maui.Controls.NavigationPage;

	/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="Type[@FullName='Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.NavigationPage']/Docs" />
	public static class NavigationPage
	{
		#region Translucent
		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='IsNavigationBarTranslucentProperty']/Docs" />
		public static readonly BindableProperty IsNavigationBarTranslucentProperty =
			BindableProperty.Create("IsNavigationBarTranslucent", typeof(bool),
			typeof(NavigationPage), false);

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='GetIsNavigationBarTranslucent']/Docs" />
		public static bool GetIsNavigationBarTranslucent(BindableObject element)
		{
			return (bool)element.GetValue(IsNavigationBarTranslucentProperty);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetIsNavigationBarTranslucent'][0]/Docs" />
		public static void SetIsNavigationBarTranslucent(BindableObject element, bool value)
		{
			element.SetValue(IsNavigationBarTranslucentProperty, value);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='IsNavigationBarTranslucent']/Docs" />
		public static bool IsNavigationBarTranslucent(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			return GetIsNavigationBarTranslucent(config.Element);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetIsNavigationBarTranslucent']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> SetIsNavigationBarTranslucent(this IPlatformElementConfiguration<iOS, FormsElement> config, bool value)
		{
			SetIsNavigationBarTranslucent(config.Element, value);
			return config;
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='EnableTranslucentNavigationBar']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> EnableTranslucentNavigationBar(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			SetIsNavigationBarTranslucent(config.Element, true);
			return config;
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='DisableTranslucentNavigationBar']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> DisableTranslucentNavigationBar(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			SetIsNavigationBarTranslucent(config.Element, false);
			return config;
		}
		#endregion


		#region StatusBarTextColorMode
		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='StatusBarTextColorModeProperty']/Docs" />
		public static readonly BindableProperty StatusBarTextColorModeProperty =
			BindableProperty.Create("StatusBarColorTextMode", typeof(StatusBarTextColorMode),
			typeof(NavigationPage), StatusBarTextColorMode.MatchNavigationBarTextLuminosity);

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='GetStatusBarTextColorMode'][0]/Docs" />
		public static StatusBarTextColorMode GetStatusBarTextColorMode(BindableObject element)
		{
			return (StatusBarTextColorMode)element.GetValue(StatusBarTextColorModeProperty);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetStatusBarTextColorMode'][0]/Docs" />
		public static void SetStatusBarTextColorMode(BindableObject element, StatusBarTextColorMode value)
		{
			element.SetValue(StatusBarTextColorModeProperty, value);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='GetStatusBarTextColorMode']/Docs" />
		public static StatusBarTextColorMode GetStatusBarTextColorMode(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			return GetStatusBarTextColorMode(config.Element);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetStatusBarTextColorMode']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> SetStatusBarTextColorMode(this IPlatformElementConfiguration<iOS, FormsElement> config, StatusBarTextColorMode value)
		{
			SetStatusBarTextColorMode(config.Element, value);
			return config;
		}
		#endregion

		#region PrefersLargeTitles
		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='PrefersLargeTitlesProperty']/Docs" />
		public static readonly BindableProperty PrefersLargeTitlesProperty = BindableProperty.Create(nameof(PrefersLargeTitles), typeof(bool), typeof(Page), false);

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='GetPrefersLargeTitles']/Docs" />
		public static bool GetPrefersLargeTitles(BindableObject element)
		{
			return (bool)element.GetValue(PrefersLargeTitlesProperty);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetPrefersLargeTitles'][0]/Docs" />
		public static void SetPrefersLargeTitles(BindableObject element, bool value)
		{
			element.SetValue(PrefersLargeTitlesProperty, value);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetPrefersLargeTitles']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> SetPrefersLargeTitles(this IPlatformElementConfiguration<iOS, FormsElement> config, bool value)
		{
			SetPrefersLargeTitles(config.Element, value);
			return config;
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='PrefersLargeTitles']/Docs" />
		public static bool PrefersLargeTitles(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			return GetPrefersLargeTitles(config.Element);
		}
		#endregion

		#region HideNavigationBarSeparator
		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='HideNavigationBarSeparatorProperty']/Docs" />
		public static readonly BindableProperty HideNavigationBarSeparatorProperty = BindableProperty.Create(nameof(HideNavigationBarSeparator), typeof(bool), typeof(Page), false);

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='GetHideNavigationBarSeparator']/Docs" />
		public static bool GetHideNavigationBarSeparator(BindableObject element)
		{
			return (bool)element.GetValue(HideNavigationBarSeparatorProperty);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetHideNavigationBarSeparator'][0]/Docs" />
		public static void SetHideNavigationBarSeparator(BindableObject element, bool value)
		{
			element.SetValue(HideNavigationBarSeparatorProperty, value);
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='SetHideNavigationBarSeparator']/Docs" />
		public static IPlatformElementConfiguration<iOS, FormsElement> SetHideNavigationBarSeparator(this IPlatformElementConfiguration<iOS, FormsElement> config, bool value)
		{
			SetHideNavigationBarSeparator(config.Element, value);
			return config;
		}

		/// <include file="../../../../docs/Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific/NavigationPage.xml" path="//Member[@MemberName='HideNavigationBarSeparator']/Docs" />
		public static bool HideNavigationBarSeparator(this IPlatformElementConfiguration<iOS, FormsElement> config)
		{
			return GetHideNavigationBarSeparator(config.Element);
		}
		#endregion
	}
}
