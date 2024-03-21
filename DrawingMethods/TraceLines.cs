﻿using AI_Graphs.Graphs;
using System.Linq;
using System.Numerics;
namespace AI_Graphs.DrawingMethods
{
	public class TraceLines : DrawingMethod
	{
		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<Node>> AdjList;
		private Vector2 NewPoint;
		private int FromIndex;
		public List<Dictionary<int, Vector2>> TraceableLines;

		public TraceLines(float radius, int amount, List<List<Node>> adjList, List<Color> colorsList, List<MyPoint> nodesLocations, int fromIndex, Vector2 newPoint, List<Dictionary<int, Vector2>> traceableLines)
		{
			Radius = radius;
			Amount = amount;
			NodesLocations = nodesLocations;
			TraceableLines = traceableLines;
			AdjList = adjList;
			ColorsList = colorsList;
			NewPoint = newPoint;
			FromIndex = fromIndex;
		}
		public override async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			int colorI = 0;
			// remember to check how this works
			ColorsList ??= Utils.RandomColors(Amount);
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
			if (TraceableLines.Count > 0 && Utils.CheckIfKeyExists(TraceableLines, FromIndex))
			{
				TraceableLines[^1] = dic1;
			}
			else
			{
				TraceableLines.Add(dic1);
			}
			canvas.DrawLine(NodesLocations[FromIndex].X, NodesLocations[FromIndex].Y, NewPoint.X, NewPoint.Y);

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