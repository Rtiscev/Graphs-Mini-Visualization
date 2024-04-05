﻿using AI_Graphs.Graphs;
using System.Numerics;
using SkiaSharp;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;

namespace AI_Graphs.DrawingMethods
{
    // concrete product
    public class TraceLines : IDrawingMethod
	{
		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<Node>> AdjList;
		private Vector2 NewPoint;
		private int FromIndex;
		public List<Dictionary<int, Vector2>> TraceableLines;

		public TraceLines()
		{
			Radius = DataCollection.GetInstance().Radius;
			Amount = DataCollection.GetInstance().Amount;
			NodesLocations = DataCollection.GetInstance().NodesLocations;
			TraceableLines = DataCollection.GetInstance().TraceableLines;
			AdjList = DataCollection.GetInstance().AdjList;
			ColorsList = DataCollection.GetInstance().ColorsList;
			NewPoint = DataCollection.GetInstance().NewPosition;
			FromIndex = DataCollection.GetInstance().FromIndex;
		}
		public async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			int colorI = 0;
			// remember to check how this works
			ColorsList ??= AI_Graphs.Utils.Utils.RandomColors(Amount);
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
			if (TraceableLines.Count > 0 && AI_Graphs.Utils.Utils.CheckIfKeyExists(TraceableLines, FromIndex))
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

			DataCollection.GetInstance().ColorsList = ColorsList;
			DataCollection.GetInstance().NodesLocations = NodesLocations;
		}
	}
}