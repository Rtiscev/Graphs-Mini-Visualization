using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.Utils;

namespace AI_Graphs.DrawingMethods
{
	// IProduct
	public abstract class IDrawingMethod
	{
		protected float Radius;
		protected List<MyPoint> NodesLocations;
		protected List<Color> ColorsList;
		protected int Amount;
		protected List<List<AI_Graphs.Graphs.Node>> AdjList;
		protected List<int> Paths;

		public event EventHandler<ByteArrayEventArgs> SendBytes;
		public virtual Task Draw(ICanvas canvas, RectF dirtyRect) { return null; }
	}
}
