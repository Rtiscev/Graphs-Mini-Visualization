namespace AI_Graphs.DrawingMethods
{
	// product
	public interface IDrawingMethod
	{
		Task Draw(ICanvas canvas, RectF dirtyRect);
	}
}
