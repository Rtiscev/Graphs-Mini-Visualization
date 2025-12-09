using AI_Graphs.BusinessLogic.DecoratorPattern;
using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.FacadePattern;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;
using SkiaSharp;

namespace AI_Graphs.DrawingMethods
{
    public class SaveGraphImage : IDrawingMethod
    {
        private SKBitmap bmp;

        public override async Task Draw(ICanvas canvas, RectF dirtyRect)
        {
            var nodesLocations = DataCollection.GetInstance().NodesLocations;
            var adjList = DataCollection.GetInstance().AdjList;
            var colors = DataCollection.GetInstance().ColorsList;
            var paths = DataCollection.GetInstance().Paths;
            int amount = DataCollection.GetInstance().Amount;
            float radius = DataCollection.GetInstance().Radius;

            if (nodesLocations == null || colors == null)
            {
                System.Diagnostics.Debug.WriteLine("Cannot save image: No graph data available");
                return;
            }

            // FIRST: Draw to canvas so screen isn't blank
            DrawToCanvas(canvas, nodesLocations, adjList, colors, paths, amount, radius);

            // THEN: Save to file
            await SaveToFile(dirtyRect, nodesLocations, adjList, colors, paths, amount, radius);
        }

        /// <summary>
        /// Draw current state to canvas (keeps screen visible)
        /// </summary>
        private void DrawToCanvas(
            ICanvas canvas,
            List<MyPoint> nodesLocations,
            List<List<AI_Graphs.Graphs.Node>> adjList,
            List<Color> colors,
            List<int> paths,
            int amount,
            float radius)
        {
            // Draw lines
            canvas.StrokeSize = 4;
            for (int i = 0; i < adjList.Count && i < nodesLocations.Count; i++)
            {
                if (i >= colors.Count) continue;

                canvas.StrokeColor = colors[i].WithAlpha(0.4f);
                MyPoint start = nodesLocations[i];

                foreach (var edge in adjList[i])
                {
                    int targetNode = edge.country;
                    if (targetNode < 0 || targetNode >= nodesLocations.Count) continue;

                    MyPoint end = nodesLocations[targetNode];
                    canvas.DrawLine(start.X, start.Y, end.X, end.Y);
                }
            }

            // Draw shadows
            canvas.FillColor = Colors.Black.WithAlpha(0.15f);
            for (int i = 0; i < amount && i < nodesLocations.Count; i++)
            {
                canvas.FillCircle(nodesLocations[i].X + 2, nodesLocations[i].Y + 2, radius);
            }

            // Draw nodes
            for (int i = 0; i < amount && i < nodesLocations.Count; i++)
            {
                if (i >= colors.Count) continue;

                float X = nodesLocations[i].X;
                float Y = nodesLocations[i].Y;

                canvas.FillColor = colors[i];
                canvas.FillCircle(X, Y, radius);

                canvas.StrokeColor = Colors.White;
                canvas.StrokeSize = 2;
                canvas.DrawCircle(X, Y, radius);

                canvas.FontColor = colors[i].GetLuminosity() > 0.5f ? Colors.Black : Colors.White;
                canvas.FontSize = radius * 0.7f;
                canvas.DrawString(i.ToString(), X - radius, Y - radius, radius * 2, radius * 2,
                    HorizontalAlignment.Center, VerticalAlignment.Center);
            }

            // Draw path
            if (paths != null && paths.Count > 1)
            {
                for (int d = 0; d < paths.Count - 1; d++)
                {
                    if (paths[d] < 0 || paths[d] >= nodesLocations.Count ||
                        paths[d + 1] < 0 || paths[d + 1] >= nodesLocations.Count)
                        continue;

                    float x1 = nodesLocations[paths[d]].X;
                    float y1 = nodesLocations[paths[d]].Y;
                    float x2 = nodesLocations[paths[d + 1]].X;
                    float y2 = nodesLocations[paths[d + 1]].Y;

                    canvas.StrokeSize = 14;
                    canvas.StrokeColor = Colors.Yellow.WithAlpha(0.3f);
                    canvas.DrawLine(x1, y1, x2, y2);

                    canvas.StrokeSize = 8;
                    canvas.StrokeColor = Colors.White;
                    canvas.DrawLine(x1, y1, x2, y2);
                }
            }
        }

        /// <summary>
        /// Save current state to image file
        /// </summary>
        private async Task SaveToFile(
            RectF dirtyRect,
            List<MyPoint> nodesLocations,
            List<List<AI_Graphs.Graphs.Node>> adjList,
            List<Color> colors,
            List<int> paths,
            int amount,
            float radius)
        {
            bmp = new SKBitmap((int)dirtyRect.Width, (int)dirtyRect.Height);
            SKCanvas sKCanvas = new SKCanvas(bmp);
            sKCanvas.Clear(SKColor.Parse("#1E1E1E"));

            // Draw lines
            for (int i = 0; i < adjList.Count && i < nodesLocations.Count; i++)
            {
                if (i >= colors.Count) break;

                MyPoint start = nodesLocations[i];
                foreach (var edge in adjList[i])
                {
                    int targetNode = edge.country;
                    if (targetNode < 0 || targetNode >= nodesLocations.Count) continue;

                    MyPoint end = nodesLocations[targetNode];
                    sKCanvas.DrawLine(
                        new SKPoint(start.X, start.Y),
                        new SKPoint(end.X, end.Y),
                        new SKPaint()
                        {
                            Color = new SKColor(
                                (byte)(colors[i].Red * 255),
                                (byte)(colors[i].Green * 255),
                                (byte)(colors[i].Blue * 255),
                                102),
                            StrokeWidth = 4,
                            IsAntialias = true
                        }
                    );
                }
            }

            // Draw path
            if (paths is not null && paths.Count > 1)
            {
                for (int d = 0; d < paths.Count - 1; d++)
                {
                    if (paths[d] < 0 || paths[d] >= nodesLocations.Count ||
                        paths[d + 1] < 0 || paths[d + 1] >= nodesLocations.Count)
                        continue;

                    var startPoint = new SKPoint(nodesLocations[paths[d]].X, nodesLocations[paths[d]].Y);
                    var endPoint = new SKPoint(nodesLocations[paths[d + 1]].X, nodesLocations[paths[d + 1]].Y);

                    sKCanvas.DrawLine(startPoint, endPoint, new SKPaint()
                    {
                        Color = SKColors.Yellow.WithAlpha(77),
                        StrokeWidth = 14,
                        IsAntialias = true
                    });

                    sKCanvas.DrawLine(startPoint, endPoint, new SKPaint()
                    {
                        Color = SKColors.White,
                        StrokeWidth = 8,
                        IsAntialias = true
                    });
                }
            }

            // Draw nodes
            for (int i = 0; i < amount && i < nodesLocations.Count; i++)
            {
                if (i >= colors.Count) break;

                float X = nodesLocations[i].X;
                float Y = nodesLocations[i].Y;

                sKCanvas.DrawCircle(X + 2, Y + 2, radius, new SKPaint()
                {
                    Color = SKColors.Black.WithAlpha(38),
                    IsAntialias = true
                });

                sKCanvas.DrawCircle(X, Y, radius, new SKPaint()
                {
                    Color = new SKColor(
                        (byte)(colors[i].Red * 255),
                        (byte)(colors[i].Green * 255),
                        (byte)(colors[i].Blue * 255)),
                    IsAntialias = true
                });

                sKCanvas.DrawCircle(X, Y, radius, new SKPaint()
                {
                    Color = SKColors.White,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 2,
                    IsAntialias = true
                });

                var desiredColor = (colors[i].GetLuminosity() > 0.5f) ? SKColors.Black : SKColors.White;
                var textPaint = new SKPaint()
                {
                    Color = desiredColor,
                    TextAlign = SKTextAlign.Center,
                    TextSize = radius * 0.7f,
                    IsAntialias = true
                };

                var textBounds = new SKRect();
                textPaint.MeasureText(i.ToString(), ref textBounds);
                float textY = Y + textBounds.Height / 2;
                sKCanvas.DrawText(i.ToString(), new SKPoint(X, textY), textPaint);
            }

            string rootPath = Environment.ProcessPath;
            for (int oo = 0; oo < 6; oo++)
            {
                rootPath = Directory.GetParent(rootPath).FullName;
            }
            await SaveGraphImageAsync(bmp, rootPath);
        }

        private async Task SaveGraphImageAsync(SKBitmap bitmap, string rootPath)
        {
            try
            {
                string imagePath = Path.Combine(rootPath, "Business Logic", "Input", "graph.png");
                string directory = Path.GetDirectoryName(imagePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(imagePath))
                {
                    try
                    {
                        File.Delete(imagePath);
                    }
                    catch (IOException)
                    {
                        await Task.Delay(100);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                }

                using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    bitmap.Encode(fileStream, SKEncodedImageFormat.Png, 100);
                }

                System.Diagnostics.Debug.WriteLine($"✓ Graph image saved: {imagePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"✗ Failed to save: {ex.Message}");
            }
        }
    }
}
