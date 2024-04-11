using AI_Graphs.BusinessLogic.DecoratorPattern;
using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.Graphs;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;
using SkiaSharp;

namespace AI_Graphs.DrawingMethods
{
	// concrete product
	public class ChangeColors : IDrawingMethod
	{
		private PrimitiveDrawing primitiveDrawing;

		private SKBitmap bmp;
		public event EventHandler<ByteArrayEventArgs> SendBytes;

		public ChangeColors()
		{
			var SingletonCopyData = DataCollection.GetInstance();
			Radius = SingletonCopyData.Radius;
			Amount = SingletonCopyData.Amount;
			NodesLocations = SingletonCopyData.NodesLocations;
			AdjList = SingletonCopyData.AdjList;
			Paths = SingletonCopyData.Paths;
		}

		public override async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			List<Color> colors = ColorsList = DataCollection.GetInstance().ColorsList = AI_Graphs.Utils.Utils.RandomColors(Amount);
			primitiveDrawing = new(ref canvas);
			primitiveDrawing.Draw();
			//primitiveDrawing.DrawLines();
			//primitiveDrawing.DrawPaths();
			//primitiveDrawing.DrawNodes();

			// image drawing
			bmp = new((int)dirtyRect.Width, (int)dirtyRect.Height);
			SKCanvas sKCanvas = new(bmp);
			//sKCanvas.Clear(SKColor.Parse("#003366"));
			// draw lines
			int colorI = 0;
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
				SendBytes?.Invoke(this, new(imageBytes));
				// Write the byte array to a file
				//using (FileStream fileStream = File.OpenWrite($"{rootPath}\\BusinessLogic\\Input\\copy.jpg"))
				//{
				//	await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
				//	await fileStream.FlushAsync();
				//}
				// Now you have the image data in a byte array
			}

			// update singleton data
			DataCollection.GetInstance().ColorsList = ColorsList;
			DataCollection.GetInstance().NodesLocations = NodesLocations;
		}
	}
}