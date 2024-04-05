using AI_Graphs.Graphs;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;
using SkiaSharp;

namespace AI_Graphs.DrawingMethods
{
    // concrete product
    public class ChangeColors : IDrawingMethod
	{
		private SKBitmap bmp;

		private float Radius;
		private List<MyPoint> NodesLocations;
		private List<Color> ColorsList;
		private int Amount;
		private List<List<Node>> AdjList;
		private List<int> Paths;
		public ChangeColors()
		{
			Radius = DataCollection.GetInstance().Radius;
			Amount = DataCollection.GetInstance().Amount;
			NodesLocations = DataCollection.GetInstance().NodesLocations;
			AdjList = DataCollection.GetInstance().AdjList;
			Paths = DataCollection.GetInstance().Paths;
		}

		public async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			// draw lines
			int colorI = 0;
			List<Color> colors = ColorsList = AI_Graphs.Utils.Utils.RandomColors(Amount);
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

			// image drawing
			bmp = new((int)dirtyRect.Width, (int)dirtyRect.Height);
			SKCanvas sKCanvas = new(bmp);
			//sKCanvas.Clear(SKColor.Parse("#003366"));
			// draw lines
			colorI = 0;
			for (int i = 0; i < AdjList.Count; i++)
			{
				MyPoint start = NodesLocations[i];
				for (int j = 0; j < AdjList[i].Count; j++)
				{
					MyPoint end = NodesLocations[AdjList[i][j].country];
					sKCanvas.DrawLine(new SKPoint(start.X, start.Y), new SKPoint(end.X, end.Y), new SKPaint() { ColorF = new(colors[colorI].Red, colors[colorI].Green, colors[colorI].Blue), StrokeWidth = 6 });
				}
				colorI++;
			}

			// draw path
			if (Paths is not null)
			{
				for (int d = 0; d < Paths.Count - 1; d++)
				{
					sKCanvas.DrawLine(new SKPoint(NodesLocations[Paths[d]].X, NodesLocations[Paths[d]].Y), new SKPoint(NodesLocations[Paths[d + 1]].X, NodesLocations[Paths[d + 1]].Y), new SKPaint() { Color = SKColors.White, StrokeWidth = 12 });
				}
			}

			colorI = 0;
			for (int i = 0; i < Amount; i++)
			{
				// draw node
				float X = NodesLocations[i].X;
				float Y = NodesLocations[i].Y;

				sKCanvas.DrawCircle(X, Y, Radius, new SKPaint() { ColorF = new(colors[colorI].Red, colors[colorI].Green, colors[colorI].Blue), StrokeWidth = 6 });

				var desireableColor = (colors[colorI].GetLuminosity() > 0.4f) ? SKColors.Black : SKColors.Yellow;

				sKCanvas.DrawText(i.ToString(), new SKPoint(X, Y), new SKPaint() { Color = desireableColor, TextAlign = SKTextAlign.Center, TextSize = 25 });

				colorI++;
			}

			// process file
			string rootPath = Environment.ProcessPath;
			for (int oo = 0; oo < 6; oo++)
			{
				rootPath = Directory.GetParent(rootPath).FullName;
			}

			// Using a MemoryStream for flexibility
			using (var memoryStream = new MemoryStream())
			{
				bmp.Encode(memoryStream, SKEncodedImageFormat.Jpeg, 1000); // Adjust quality as needed
				byte[] imageBytes = memoryStream.ToArray();
				// Write the byte array to a file
				using (FileStream fileStream = File.OpenWrite($"{rootPath}\\copy.jpg"))
				{
					await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
					await fileStream.FlushAsync();
				}
				// Now you have the image data in a byte array
			}

			// get colors and nodes
			DataCollection.GetInstance().ColorsList = ColorsList;
			DataCollection.GetInstance().NodesLocations = NodesLocations;
		}
	}
}