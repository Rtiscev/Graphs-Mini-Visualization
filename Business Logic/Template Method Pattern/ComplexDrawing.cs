using AI_Graphs.FacadePattern;
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
            if (AdjList == null || NodesLocations == null) return;

            canvas.StrokeSize = 4;

            for (int i = 0; i < AdjList.Count && i < NodesLocations.Count; i++)
            {
                if (i >= ColorsList.Count) continue;

                canvas.StrokeColor = ColorsList[i].WithAlpha(0.4f); // Semi-transparent
                MyPoint start = NodesLocations[i];

                foreach (var edge in AdjList[i])
                {
                    int targetNode = edge.country;
                    if (targetNode < 0 || targetNode >= NodesLocations.Count) continue;

                    MyPoint end = NodesLocations[targetNode];
                    canvas.DrawLine(start.X, start.Y, end.X, end.Y);
                }
            }
        }

        protected override void DrawNodes()
        {
            if (NodesLocations == null || ColorsList == null) return;

            // Pass 1: Draw shadows
            canvas.FillColor = Colors.Black.WithAlpha(0.15f);
            for (int i = 0; i < Amount && i < NodesLocations.Count; i++)
            {
                canvas.FillCircle(
                    NodesLocations[i].X + 2,
                    NodesLocations[i].Y + 2,
                    Radius
                );
            }

            // Pass 2: Draw node circles
            for (int i = 0; i < Amount && i < NodesLocations.Count; i++)
            {
                if (i >= ColorsList.Count) continue;

                float X = NodesLocations[i].X;
                float Y = NodesLocations[i].Y;

                // Fill circle
                canvas.FillColor = ColorsList[i];
                canvas.FillCircle(X, Y, Radius);

                // White border
                canvas.StrokeColor = Colors.White;
                canvas.StrokeSize = 2;
                canvas.DrawCircle(X, Y, Radius);
            }

            // Pass 3: Draw text labels
            for (int i = 0; i < Amount && i < NodesLocations.Count; i++)
            {
                if (i >= ColorsList.Count) continue;

                float X = NodesLocations[i].X;
                float Y = NodesLocations[i].Y;

                // Smart text color based on background luminosity
                canvas.FontColor = ColorsList[i].GetLuminosity() > 0.5f
                    ? Colors.Black
                    : Colors.White;

                canvas.FontSize = Radius * 0.7f;
                canvas.DrawString(
                    i.ToString(),
                    X - Radius,
                    Y - Radius,
                    Radius * 2,
                    Radius * 2,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center
                );
            }
        }

        protected override void DrawSpecific()
        {
            if (NodesLocations == null || FromIndex < 0 || FromIndex >= NodesLocations.Count)
                return;

            // Draw old traceable lines with glow effect
            if (TraceableLines != null && TraceableLines.Count > 1)
            {
                for (int b = 0; b < TraceableLines.Count && TraceableLines[b].Keys.First() != FromIndex; b++)
                {
                    int key = TraceableLines[b].Keys.First();
                    if (key < 0 || key >= NodesLocations.Count) continue;

                    Vector2 endPoint = TraceableLines[b].Values.First();

                    // Glow layer
                    canvas.StrokeSize = 14;
                    canvas.StrokeColor = Colors.Yellow.WithAlpha(0.3f);
                    canvas.DrawLine(
                        NodesLocations[key].X,
                        NodesLocations[key].Y,
                        endPoint.X,
                        endPoint.Y
                    );

                    // Core line
                    canvas.StrokeSize = 8;
                    canvas.StrokeColor = Colors.White;
                    canvas.DrawLine(
                        NodesLocations[key].X,
                        NodesLocations[key].Y,
                        endPoint.X,
                        endPoint.Y
                    );
                }
            }

            // Update or add new traceable line
            var dic1 = new Dictionary<int, Vector2>
            {
                { FromIndex, NewPoint }
            };

            if (TraceableLines != null)
            {
                if (TraceableLines.Count > 0 && MathUtils.CheckIfKeyExists(TraceableLines, FromIndex))
                {
                    TraceableLines[^1] = dic1;
                }
                else
                {
                    TraceableLines.Add(dic1);
                }
            }

            // Draw current animated line with glow effect
            if (FromIndex >= 0 && FromIndex < NodesLocations.Count)
            {
                // Glow layer
                canvas.StrokeSize = 14;
                canvas.StrokeColor = Colors.Yellow.WithAlpha(0.3f);
                canvas.DrawLine(
                    NodesLocations[FromIndex].X,
                    NodesLocations[FromIndex].Y,
                    NewPoint.X,
                    NewPoint.Y
                );

                // Core line
                canvas.StrokeSize = 8;
                canvas.StrokeColor = Colors.White;
                canvas.DrawLine(
                    NodesLocations[FromIndex].X,
                    NodesLocations[FromIndex].Y,
                    NewPoint.X,
                    NewPoint.Y
                );
            }
        }
    }
}
