﻿using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<IShapeView, MauiShapeView>
	{
		protected override MauiShapeView CreatePlatformView()
		{
			return new MauiShapeView();
		}

		public static void MapShape(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.UpdateShape(shapeView);
		}

		public static void MapAspect(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapFill(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStroke(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeThickness(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeDashPattern(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeLineCap(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeLineJoin(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeMiterLimit(ShapeViewHandler handler, IShapeView shapeView)
		{
			handler.PlatformView?.InvalidateShape(shapeView);
		}
	}
}