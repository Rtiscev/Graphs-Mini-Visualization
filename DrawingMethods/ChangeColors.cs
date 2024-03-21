using AI_Graphs.Graphs;

namespace AI_Graphs.DrawingMethods
{
	public class ChangeColors : DrawingMethod
	{
		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<Node>> AdjList;
		private List<int> Paths;
		public ChangeColors(float radius, int amount, List<List<Node>> adjList, List<MyPoint> nodesLocations, List<int> paths)
		{
			Radius = radius;
			Amount = amount;
			NodesLocations = nodesLocations;
			AdjList = adjList;
			Paths = paths;
		}

		public override async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			// draw lines
			int colorI = 0;
			List<Color> colors = ColorsList = Utils.RandomColors(Amount);
			canvas.StrokeSize = 6;
			for (int i = 0; i < AdjList.Count; i++)
			{
				canvas.StrokeColor = colors[colorI];
				MyPoint start = NodesLocations[i];
				for (int j = 0; j < AdjList[i].Count; j++)
				{
					MyPoint end = NodesLocations[AdjList[i][j].country];
					canvas.DrawLine(start.X, start.Y, end.X, end.Y);
				}
				colorI++;
			}

			// draw path
			canvas.StrokeSize = 12;
			canvas.StrokeColor = Colors.White;
			if (Paths is not null)
			{
				for (int d = 0; d < Paths.Count - 1; d++)
				{
					canvas.DrawLine(NodesLocations[Paths[d]].X, NodesLocations[Paths[d]].Y, NodesLocations[Paths[d + 1]].X, NodesLocations[Paths[d + 1]].Y);
				}
			}

			canvas.StrokeSize = 6;
			colorI = 0;
			for (int i = 0; i < Amount; i++)
			{
				// draw node
				canvas.FillColor = colors[colorI];
				float X = NodesLocations[i].X;
				float Y = NodesLocations[i].Y;
				canvas.FillCircle(X, Y, Radius);

				canvas.FontColor = (colors[colorI].GetLuminosity() > 0.4f) ? Colors.Black : Colors.Yellow;

				// draw text (mark nodes)
				canvas.FontSize = 25;
				canvas.DrawString(i.ToString(), X - Radius / 2, Y - Radius / 2, Radius, Radius, HorizontalAlignment.Center, VerticalAlignment.Center);

				colorI++;
			}
		}

		public override List<Color> ReturnColors()
		{
			return ColorsList;
		}

		public override List<MyPoint> ReturnNodesList()
		{
			return NodesLocations;
		}

	}
}