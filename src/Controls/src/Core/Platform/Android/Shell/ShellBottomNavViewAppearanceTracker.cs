﻿using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Util;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.View;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Shape;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using AColor = Android.Graphics.Color;
using R = Android.Resource;

namespace Microsoft.Maui.Controls.Platform
{
	public class ShellBottomNavViewAppearanceTracker : IShellBottomNavViewAppearanceTracker
	{
		IShellContext _shellContext;
		ShellItem _shellItem;
		static ColorStateList _defaultListLight;
		static ColorStateList _defaultListDark;

		bool _disposed;
		ColorStateList _colorStateList;

		public ShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem)
		{
			_shellItem = shellItem;
			_shellContext = shellContext;
		}

		static ColorStateList GetDefaultTabColorList(Context context) =>
			ShellView.IsDarkTheme ?
			_defaultListDark ??= MakeDefaultColorStateList(context)
			: _defaultListLight ??= MakeDefaultColorStateList(context);

		public virtual void ResetAppearance(BottomNavigationView bottomView)
		{
			bottomView.ItemIconTintList = GetDefaultTabColorList(_shellContext.AndroidContext);
			bottomView.ItemTextColor = GetDefaultTabColorList(_shellContext.AndroidContext);
			SetBackgroundColor(bottomView, null);
		}

		public virtual void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
		{
			IShellAppearanceElement controller = appearance;
			var backgroundColor = controller.EffectiveTabBarBackgroundColor;
			var foregroundColor = controller.EffectiveTabBarForegroundColor; // currently unused
			var disabledColor = controller.EffectiveTabBarDisabledColor;
			var unselectedColor = controller.EffectiveTabBarUnselectedColor;
			var titleColor = controller.EffectiveTabBarTitleColor;

			_colorStateList = MakeColorStateList(titleColor, disabledColor, unselectedColor);
			bottomView.ItemTextColor = _colorStateList;
			bottomView.ItemIconTintList = _colorStateList;

			SetBackgroundColor(bottomView, backgroundColor);
		}

		protected virtual void SetBackgroundColor(BottomNavigationView bottomView, Color color)
		{
			var menuView = bottomView.GetChildAt(0) as BottomNavigationMenuView;
			var oldBackground = bottomView.Background;
			var colorDrawable = oldBackground as ColorDrawable;
			var colorChangeRevealDrawable = oldBackground as ColorChangeRevealDrawable;
			AColor lastColor = colorChangeRevealDrawable?.EndColor ?? colorDrawable?.Color ?? Colors.Transparent.ToPlatform();
			AColor newColor;

			if (color == null)
				newColor = ShellView.DefaultBottomNavigationViewBackgroundColor.ToPlatform();
			else
				newColor = color.ToPlatform();

			if (menuView == null)
			{
				if (colorDrawable != null && lastColor == newColor)
					return;

				if (lastColor != newColor || colorDrawable == null)
				{
					// taken from android source code
					var backgroundColor = new MaterialShapeDrawable();
					backgroundColor.FillColor = ColorStateList.ValueOf(newColor);
					backgroundColor.InitializeElevationOverlay(bottomView.Context);

					ViewCompat.SetBackground(bottomView, backgroundColor);
				}
			}
			else
			{
				if (colorChangeRevealDrawable != null && lastColor == newColor)
					return;

				var index = ((IShellItemController)_shellItem).GetItems().IndexOf(_shellItem.CurrentItem);
				var menu = bottomView.Menu;
				index = Math.Min(index, menu.Size() - 1);

				var child = menuView.GetChildAt(index);
				if (child == null)
					return;

				var touchPoint = new Point(child.Left + (child.Right - child.Left) / 2, child.Top + (child.Bottom - child.Top) / 2);

				ViewCompat.SetBackground(bottomView, new ColorChangeRevealDrawable(lastColor, newColor, touchPoint));
			}
		}

		static ColorStateList MakeDefaultColorStateList(Context context)
		{
			TypedValue mTypedValue = new TypedValue();
			if (context.Theme?.ResolveAttribute(R.Attribute.TextColorSecondary, mTypedValue, true) == false)
				return null;

			var baseCSL = AppCompatResources.GetColorStateList(context, mTypedValue.ResourceId);
			var colorPrimary = (ShellView.IsDarkTheme) ? AColor.White : ShellView.DefaultBackgroundColor.ToPlatform();
			int defaultColor = baseCSL.DefaultColor;
			var disabledcolor = baseCSL.GetColorForState(new[] { -R.Attribute.StateEnabled }, AColor.Gray);

			return MakeColorStateList(colorPrimary, disabledcolor, defaultColor);
		}

		ColorStateList MakeColorStateList(Color titleColor, Color disabledColor, Color unselectedColor)
		{
			var defaultList = GetDefaultTabColorList(_shellContext.AndroidContext);

			var disabledInt = disabledColor == null ?
				defaultList.GetColorForState(new[] { -R.Attribute.StateEnabled }, AColor.Gray) :
				disabledColor.ToPlatform().ToArgb();

			var checkedInt = titleColor == null ?
				defaultList.GetColorForState(new[] { R.Attribute.StateChecked }, AColor.Black) :
				titleColor.ToPlatform().ToArgb();

			var defaultColor = unselectedColor == null ?
				defaultList.DefaultColor :
				unselectedColor.ToPlatform().ToArgb();

			return MakeColorStateList(checkedInt, disabledInt, defaultColor);
		}

		static ColorStateList MakeColorStateList(int titleColorInt, int disabledColorInt, int defaultColor)
		{
			var states = new int[][] {
				new int[] { -R.Attribute.StateEnabled },
				new int[] {R.Attribute.StateChecked },
				new int[] { }
			};

			var colors = new[] { disabledColorInt, titleColorInt, defaultColor };

			return new ColorStateList(states, colors);
		}

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if (disposing)
			{
				_colorStateList?.Dispose();

				_shellItem = null;
				_shellContext = null;
				_colorStateList = null;
			}
		}

		#endregion IDisposable
	}
}
