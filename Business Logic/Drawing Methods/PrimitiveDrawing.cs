using AI_Graphs.FacadePattern;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Template_Method_Pattern;

namespace AI_Graphs.BusinessLogic.DecoratorPattern
{
	public class PrimitiveDrawing : IDrawingTemplate
	{
		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<AI_Graphs.Graphs.Node>> AdjList;
		private List<int> Paths;
		ICanvas canvas;
		public PrimitiveDrawing(ref ICanvas canvas)
		{
			this.canvas = canvas;
			Radius = DataCollection.GetInstance().Radius;
			Amount = DataCollection.GetInstance().Amount;
			AdjList = DataCollection.GetInstance().AdjList;
			ColorsList = DataCollection.GetInstance().ColorsList;
			Paths = DataCollection.GetInstance().Paths;
			NodesLocations = DataCollection.GetInstance().NodesLocations;
		}

		protected override void DrawLines()
		{
			int colorI = 0;
			canvas.StrokeSize = 6;
			for (int i = 0; i < AdjList.Count; i++)
			{
				canvas.StrokeColor = ColorsList[colorI];
				MyPoint start = NodesLocations[i];
				for (int j = 0; j < AdjList[i].Count; j++)
				{
					MyPoint end = NodesLocations[AdjList[i][j].country];
					canvas.DrawLine(start.X, start.Y, end.X, end.Y);
				}
				colorI++;
			}
		}
		protected override void DrawNodes()
		{
			canvas.StrokeSize = 6;
			int colorI = 0;
			for (int i = 0; i < Amount; i++)
			{
				// draw node
				canvas.FillColor = ColorsList[colorI];
				float X = NodesLocations[i].X;
				float Y = NodesLocations[i].Y;
				canvas.FillCircle(X, Y, Radius);

				canvas.FontColor = (ColorsList[colorI].GetLuminosity() > 0.4f) ? Colors.Black : Colors.Yellow;

				// draw text (mark nodes)
				canvas.FontSize = 25;
				canvas.DrawString(i.ToString(), X - Radius / 2, Y - Radius / 2, Radius, Radius, HorizontalAlignment.Center, VerticalAlignment.Center);

				colorI++;
			}
		}
		protected override void DrawSpecific()
		{
			canvas.StrokeSize = 12;
			canvas.StrokeColor = Colors.White;
			if (Paths is not null)
			{
				for (int d = 0; d < Paths.Count - 1; d++)
				{
					canvas.DrawLine(NodesLocations[Paths[d]].X, NodesLocations[Paths[d]].Y, NodesLocations[Paths[d + 1]].X, NodesLocations[Paths[d + 1]].Y);
				}
			}
		}
	}
}
