namespace AI_Graphs.DrawingMethods
{
	public abstract class DrawingMethod
	{
		abstract public Task Draw(ICanvas canvas, RectF dirtyRect);
		abstract public List<MyPoint> ReturnNodesList();
		abstract public List<Color> ReturnColors();
	}
}
