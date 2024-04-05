using AI_Graphs.Graphs;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;

#pragma warning disable

namespace AI_Graphs.DrawingMethods
{
    // concrete product
    class SimpleDraw : IDrawingMethod
	{
		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<Node>> AdjList;
		private List<int> Paths;
		public SimpleDraw()
		{
			DataCollection.GetInstance().NodesLocations.Clear();
			Radius = DataCollection.GetInstance().Radius;
			Amount = DataCollection.GetInstance().Amount;
			AdjList = DataCollection.GetInstance().AdjList;
			ColorsList = DataCollection.GetInstance().ColorsList;
			Paths = DataCollection.GetInstance().Paths;
			NodesLocations = new();
		}

		public async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			NodesLocations = AI_Graphs.Utils.Utils.GeneratePoints((int)dirtyRect.Width, (int)dirtyRect.Height, Amount, (int)Radius);

			// draw lines
			int colorI = 0;
			if (ColorsList is null)
			{
				ColorsList = AI_Graphs.Utils.Utils.RandomColors(Amount);
			}
			List<Color> colors = ColorsList;

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

			DataCollection.GetInstance().ColorsList = ColorsList;
			DataCollection.GetInstance().NodesLocations = NodesLocations;
		}
	}
}