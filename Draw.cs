using AI_Graphs.DrawingMethods;
using AI_Graphs.Graphs;
using System.Numerics;

namespace AI_Graphs
{
	public class GraphicsDrawable : IDrawable
	{
		public List<MyPoint> NodesLocations;
		public List<Color> ColorsList;
		public bool IsRandom = true;
		public bool IsTraceable = false;
		public int Amount;
		public bool IsInitialized = false;
		public List<List<Node>> AdjList;
		public Vector2 NewPosition = Vector2.Zero;
		public List<Dictionary<int, Vector2>> TraceableLines;
		public int FromIndex;
		public List<int> Paths;


		float radius;
		public GraphicsDrawable()
		{
			radius = 30;
			NodesLocations = new();
			TraceableLines = new();
			Paths = null;
		}

		public void ChangeRadius(float _radius)
		{
			radius = _radius;
		}
		public async void Draw(ICanvas canvas, RectF dirtyRect)
		{
			if (IsInitialized)
			{
				DrawingMethod drawingMethod;
				if (IsRandom)
				{
					NodesLocations.Clear();
					drawingMethod = new SimpleDraw(radius, Amount, AdjList, ColorsList, Paths);
					await drawingMethod.Draw(canvas, dirtyRect);
					NodesLocations = drawingMethod.ReturnNodesList();
					ColorsList = drawingMethod.ReturnColors();
					IsRandom = false;
				}
				else if (IsTraceable)
				{
					drawingMethod = new TraceLines(radius, Amount, AdjList, ColorsList, NodesLocations, FromIndex, NewPosition, TraceableLines);
					await drawingMethod.Draw(canvas, dirtyRect);

					IsTraceable = false;
				}
				else
				{
					drawingMethod = new ChangeColors(radius, Amount, AdjList, NodesLocations, Paths);
					await drawingMethod.Draw(canvas, dirtyRect);
					NodesLocations = drawingMethod.ReturnNodesList();
					ColorsList = drawingMethod.ReturnColors();
					IsRandom = false;
				}
			}
		}
	}
}