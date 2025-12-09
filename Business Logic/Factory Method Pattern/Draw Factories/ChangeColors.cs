using AI_Graphs.BusinessLogic.DecoratorPattern;
using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.FacadePattern;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;
using SkiaSharp;

namespace AI_Graphs.DrawingMethods
{
	// Concrete Product
	public class ChangeColors : IDrawingMethod
	{
		private PrimitiveDrawing primitiveDrawing;

		private SKBitmap bmp;
		public event EventHandler<ByteArrayEventArgs> SendBytes;

		public override async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
            var facade = new GraphGenerationFacade();
			

            List<Color> colors = ColorsList = DataCollection.GetInstance().ColorsList = facade.GenerateColorScheme();
			primitiveDrawing = new(ref canvas);
			primitiveDrawing.Draw();

			// image drawing
			bmp = new((int)dirtyRect.Width, (int)dirtyRect.Height);
			SKCanvas sKCanvas = new(bmp);
			//sKCanvas.Clear(SKColor.Parse("#003366"));
			// draw lines
			int colorI = 0;
			for (int i = 0; i < DataCollection.GetInstance().AdjList.Count; i++)
			{
				MyPoint start = DataCollection.GetInstance().NodesLocations[i];
				for (int j = 0; j < DataCollection.GetInstance().AdjList[i].Count; j++)
				{
					MyPoint end = DataCollection.GetInstance().NodesLocations[DataCollection.GetInstance().AdjList[i][j].country];
					sKCanvas.DrawLine(new SKPoint(start.X, start.Y), new SKPoint(end.X, end.Y), new SKPaint() { ColorF = new(colors[colorI].Red, colors[colorI].Green, colors[colorI].Blue), StrokeWidth = 6 });
				}
				colorI++;
			}

			// draw path
			if (DataCollection.GetInstance().Paths is not null)
			{
				for (int d = 0; d < DataCollection.GetInstance().Paths.Count - 1; d++)
				{
					sKCanvas.DrawLine(new SKPoint(DataCollection.GetInstance().NodesLocations[DataCollection.GetInstance().Paths[d]].X, DataCollection.GetInstance().NodesLocations[Paths[d]].Y), new SKPoint(DataCollection.GetInstance().NodesLocations[DataCollection.GetInstance().Paths[d + 1]].X, DataCollection.GetInstance().NodesLocations[DataCollection.GetInstance().Paths[d + 1]].Y), new SKPaint() { Color = SKColors.White, StrokeWidth = 12 });
				}
			}

			colorI = 0;
			for (int i = 0; i < DataCollection.GetInstance().Amount; i++)
			{
				// draw node
				float X = DataCollection.GetInstance().NodesLocations[i].X;
				float Y = DataCollection.GetInstance().NodesLocations[i].Y;

				sKCanvas.DrawCircle(X, Y, DataCollection.GetInstance().Radius, new SKPaint() { ColorF = new(colors[colorI].Red, colors[colorI].Green, colors[colorI].Blue), StrokeWidth = 6 });
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
				bmp.Encode(memoryStream, SKEncodedImageFormat.Jpeg, 1000); 
				byte[] imageBytes = memoryStream.ToArray();
				//SendBytes?.Invoke(this, new(imageBytes));
				using (FileStream fileStream = File.OpenWrite($"{rootPath}\\Business Logic\\Input\\image.jpg"))
				{
					await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
					await fileStream.FlushAsync();
				}
			}

		}
	}
}