﻿using AI_Graphs.Template_Method_Pattern;

namespace AI_Graphs.DrawingMethods
{
	// concrete product
	public class TraceLines : IDrawingMethod
	{
		private ComplexDrawing complexDrawing;

		public override async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			complexDrawing = new(ref canvas);
			complexDrawing.Draw();
		}
	}
}