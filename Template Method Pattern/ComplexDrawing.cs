using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;
using System.Numerics;

namespace AI_Graphs.Template_Method_Pattern
{
	public class ComplexDrawing : IDrawingTemplate
	{
		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<AI_Graphs.Graphs.Node>> AdjList;
		ICanvas canvas;
		private Vector2 NewPoint;
		private int FromIndex;
		public List<Dictionary<int, Vector2>> TraceableLines;
		public ComplexDrawing(ref ICanvas canvas)
		{
			var singleTonData = DataCollection.GetInstance();
			Radius = singleTonData.Radius;
			Amount = singleTonData.Amount;
			TraceableLines = singleTonData.TraceableLines;
			AdjList = singleTonData.AdjList;
			ColorsList = singleTonData.ColorsList;
			NodesLocations = singleTonData.NodesLocations;
			NewPoint = singleTonData.NewPosition;
			FromIndex = singleTonData.FromIndex;

			this.canvas = canvas;
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
			// draw big line/s
			canvas.StrokeSize = 12;
			canvas.StrokeColor = Colors.White;

			// separate old lines and new lines by drawing first old and then new...
			var dic1 = new Dictionary<int, Vector2>
				{
					{ FromIndex, NewPoint }
				};
			if (TraceableLines.Count > 1)
			{
				// draw old
				for (int b = 0; b < TraceableLines.Count && TraceableLines[b].Keys.First() != FromIndex; b++)
				{
					int key = TraceableLines[b].Keys.First();
					canvas.DrawLine(NodesLocations[key].X, NodesLocations[key].Y, TraceableLines[b].Values.First().X, TraceableLines[b].Values.First().Y);
				}
			}

			// draw new
			if (TraceableLines.Count > 0 && AI_Graphs.Utils.Utils.CheckIfKeyExists(TraceableLines, FromIndex))
			{
				TraceableLines[^1] = dic1;
			}
			else
			{
				TraceableLines.Add(dic1);
			}
			canvas.DrawLine(NodesLocations[FromIndex].X, NodesLocations[FromIndex].Y, NewPoint.X, NewPoint.Y);
		}
	}
}
