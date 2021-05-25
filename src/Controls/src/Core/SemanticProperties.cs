using System;
using System.ComponentModel;

namespace Microsoft.Maui.Controls
{
	public class SemanticProperties
	{
		public static readonly BindableProperty DescriptionProperty = BindableProperty.CreateAttached("Description", typeof(string), typeof(SemanticProperties), default(string), propertyChanged: OnDescriptionPropertyChanged);

		public static readonly BindableProperty HintProperty = BindableProperty.CreateAttached("Hint", typeof(string), typeof(SemanticProperties), default(string), propertyChanged: OnHintPropertyChanged);

		public static readonly BindableProperty HeadingLevelProperty = BindableProperty.CreateAttached("HeadingLevel", typeof(SemanticHeadingLevel), typeof(SemanticProperties), SemanticHeadingLevel.None, propertyChanged: OnHeadingLevelPropertyChanged);

		public static readonly BindableProperty LabeledByProperty = BindableProperty.CreateAttached("LabeledBy", typeof(IView), typeof(SemanticProperties), default(IView), propertyChanged: OnLabeledByPropertyChanged);

		static void OnDescriptionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			string value = null;
			if (newValue != null)
				value = newValue.ToString();

			UpdateSemanticsProperty(bindable,
				(semantics) => semantics.Description = value);
		}

		static void OnHintPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			string value = null;
			if (newValue != null)
				value = newValue.ToString();

			UpdateSemanticsProperty(bindable,
				(semantics) => semantics.Hint = value);
		}

		static void OnHeadingLevelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			UpdateSemanticsProperty(bindable,
				(semantics) => semantics.HeadingLevel = (SemanticHeadingLevel)newValue);
		}

		static void OnLabeledByPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			IView value = null;
			if (newValue != null)
				value = newValue as IView;

			UpdateSemanticsProperty(bindable,
				(semantics) => semantics.LabeledBy = value);
		}

		static void UpdateSemanticsProperty(BindableObject bindable, Action<Semantics> action)
		{
			action.Invoke(((VisualElement)bindable).SetupSemantics());
			if (bindable is IFrameworkElement fe)
				fe.Handler?.UpdateValue(nameof(IView.Semantics));
		}

		public static string GetDescription(BindableObject bindable)
		{
			return (string)bindable.GetValue(DescriptionProperty);
		}

		public static void SetDescription(BindableObject bindable, string value)
		{
			bindable.SetValue(DescriptionProperty, value);
		}

		public static string GetHint(BindableObject bindable)
		{
			return (string)bindable.GetValue(HintProperty);
		}

		public static void SetHint(BindableObject bindable, string value)
		{
			bindable.SetValue(HintProperty, value);
		}

		public static SemanticHeadingLevel GetHeadingLevel(BindableObject bindable)
		{
			return (SemanticHeadingLevel)bindable.GetValue(HeadingLevelProperty);
		}

		public static void SetHeadingLevel(BindableObject bindable, SemanticHeadingLevel value)
		{
			bindable.SetValue(HeadingLevelProperty, value);
		}

		public static VisualElement GetLabeledBy(BindableObject bindable)
		{
			return (VisualElement)bindable.GetValue(LabeledByProperty);
		}

		public static void SetLabeledBy(BindableObject bindable, IView value)
		{
			bindable.SetValue(LabeledByProperty, value);
		}
	}
}
