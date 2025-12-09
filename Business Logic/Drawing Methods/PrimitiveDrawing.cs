using AI_Graphs.FacadePattern;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Template_Method_Pattern;
using System.Numerics;

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
        private float canvasWidth;
        private float canvasHeight;

        public PrimitiveDrawing(ref ICanvas canvas)
        {
            this.canvas = canvas;

            // Get actual canvas dimensions from DataCollection
            this.canvasWidth = DataCollection.GetInstance().CanvasWidth;
            this.canvasHeight = DataCollection.GetInstance().CanvasHeight;

            // Safety fallback
            if (this.canvasWidth <= 10) this.canvasWidth = 1000;
            if (this.canvasHeight <= 10) this.canvasHeight = 800;

            Radius = DataCollection.GetInstance().Radius;
            Amount = DataCollection.GetInstance().Amount;
            AdjList = DataCollection.GetInstance().AdjList;
            ColorsList = DataCollection.GetInstance().ColorsList;
            Paths = DataCollection.GetInstance().Paths;
            NodesLocations = DataCollection.GetInstance().NodesLocations;

            System.Diagnostics.Debug.WriteLine($"PrimitiveDrawing using: {canvasWidth}w x {canvasHeight}h, Radius: {Radius}");

            ApplyRandomNonOverlappingLayout();
        }

        /// <summary>
        /// Random Placement with Collision Detection
        /// GUARANTEES all nodes stay within bounds
        /// </summary>
        private void ApplyRandomNonOverlappingLayout()
        {
            if (Amount == 0) return;

            List<MyPoint> positions = new List<MyPoint>();
            Random rand = new Random();

            // Minimum distance between node centers
            float minDistance = Radius * 2.2f;

            // Conservative padding - ensure circles are FULLY visible
            float paddingX = Radius + 10;
            float paddingY = Radius + 10;

            // Calculate safe zone - NEVER exceed these bounds
            float minX = paddingX;
            float maxX = canvasWidth - paddingX;
            float minY = paddingY;
            float maxY = canvasHeight - paddingY;

            // Debug safety check
            if (maxX <= minX || maxY <= minY)
            {
                ApplyFallbackLayout();
                return;
            }

            // Available space
            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            // Place each node
            for (int i = 0; i < Amount; i++)
            {
                MyPoint newPos = new MyPoint();
                bool foundValidPosition = false;
                int attempts = 0;
                int maxAttempts = 2000;

                // Try random placement
                while (attempts < maxAttempts)
                {
                    // Generate position - guaranteed within safe zone
                    float x = minX + (float)rand.NextDouble() * availableWidth;
                    float y = minY + (float)rand.NextDouble() * availableHeight;

                    // Double-check bounds (paranoid safety)
                    x = Math.Max(minX, Math.Min(maxX, x));
                    y = Math.Max(minY, Math.Min(maxY, y));

                    MyPoint candidate = new MyPoint { X = x, Y = y };

                    // Check collision with existing nodes
                    bool isValid = true;
                    foreach (var existingPos in positions)
                    {
                        float dx = candidate.X - existingPos.X;
                        float dy = candidate.Y - existingPos.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        if (distance < minDistance)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        newPos = candidate;
                        foundValidPosition = true;
                        break;
                    }

                    attempts++;
                }

                // Force-based placement if random failed
                if (!foundValidPosition)
                {
                    newPos = FindForcedPosition(positions, minX, maxX, minY, maxY, minDistance, rand);
                }

                // FINAL SAFETY CLAMP - absolutely guarantee bounds
                newPos.X = Math.Max(minX, Math.Min(maxX, newPos.X));
                newPos.Y = Math.Max(minY, Math.Min(maxY, newPos.Y));

                positions.Add(newPos);
            }

            NodesLocations = positions;
            DataCollection.GetInstance().NodesLocations = NodesLocations;
        }

        /// <summary>
        /// Force-based positioning with strict bounds
        /// </summary>
        private MyPoint FindForcedPosition(
            List<MyPoint> existingPositions,
            float minX, float maxX,
            float minY, float maxY,
            float minDistance,
            Random rand)
        {
            // Start in safe zone
            float availableWidth = maxX - minX;
            float availableHeight = maxY - minY;

            float x = minX + (float)rand.NextDouble() * availableWidth;
            float y = minY + (float)rand.NextDouble() * availableHeight;

            // Iterative push
            for (int iteration = 0; iteration < 100; iteration++)
            {
                float pushX = 0;
                float pushY = 0;

                foreach (var pos in existingPositions)
                {
                    float dx = x - pos.X;
                    float dy = y - pos.Y;
                    float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distance < minDistance && distance > 0.1f)
                    {
                        float pushStrength = (minDistance - distance) / distance;
                        pushX += dx * pushStrength;
                        pushY += dy * pushStrength;
                    }
                }

                x += pushX * 0.6f;
                y += pushY * 0.6f;

                // Strict clamping every iteration
                x = Math.Max(minX, Math.Min(maxX, x));
                y = Math.Max(minY, Math.Min(maxY, y));
            }

            return new MyPoint { X = x, Y = y };
        }

        /// <summary>
        /// Fallback with guaranteed bounds
        /// </summary>
        private void ApplyFallbackLayout()
        {
            List<MyPoint> positions = new List<MyPoint>();

            int cols = (int)Math.Ceiling(Math.Sqrt(Amount));
            int rows = (int)Math.Ceiling((double)Amount / cols);

            float paddingX = Radius + 10;
            float paddingY = Radius + 10;

            float availableWidth = canvasWidth - (2 * paddingX);
            float availableHeight = canvasHeight - (2 * paddingY);

            float cellWidth = availableWidth / cols;
            float cellHeight = availableHeight / rows;

            for (int i = 0; i < Amount; i++)
            {
                int row = i / cols;
                int col = i % cols;

                float x = paddingX + (col + 0.5f) * cellWidth;
                float y = paddingY + (row + 0.5f) * cellHeight;

                // Final clamp
                x = Math.Max(paddingX, Math.Min(canvasWidth - paddingX, x));
                y = Math.Max(paddingY, Math.Min(canvasHeight - paddingY, y));

                positions.Add(new MyPoint { X = x, Y = y });
            }

            NodesLocations = positions;
            DataCollection.GetInstance().NodesLocations = NodesLocations;
        }

        protected override void DrawLines()
        {
            if (AdjList == null || NodesLocations == null) return;

            canvas.StrokeSize = 4;

            for (int i = 0; i < AdjList.Count && i < NodesLocations.Count; i++)
            {
                if (i >= ColorsList.Count) continue;

                canvas.StrokeColor = ColorsList[i].WithAlpha(0.4f);
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

            // Draw shadows
            canvas.FillColor = Colors.Black.WithAlpha(0.15f);
            for (int i = 0; i < Amount && i < NodesLocations.Count; i++)
            {
                canvas.FillCircle(
                    NodesLocations[i].X + 2,
                    NodesLocations[i].Y + 2,
                    Radius
                );
            }

            // Draw nodes
            for (int i = 0; i < Amount && i < NodesLocations.Count; i++)
            {
                if (i >= ColorsList.Count) continue;

                float X = NodesLocations[i].X;
                float Y = NodesLocations[i].Y;

                // Fill
                canvas.FillColor = ColorsList[i];
                canvas.FillCircle(X, Y, Radius);

                // Border
                canvas.StrokeColor = Colors.White;
                canvas.StrokeSize = 2;
                canvas.DrawCircle(X, Y, Radius);

                // Text
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
            if (Paths == null || Paths.Count < 2 || NodesLocations == null) return;

            for (int d = 0; d < Paths.Count - 1; d++)
            {
                if (Paths[d] < 0 || Paths[d] >= NodesLocations.Count ||
                    Paths[d + 1] < 0 || Paths[d + 1] >= NodesLocations.Count)
                    continue;

                float x1 = NodesLocations[Paths[d]].X;
                float y1 = NodesLocations[Paths[d]].Y;
                float x2 = NodesLocations[Paths[d + 1]].X;
                float y2 = NodesLocations[Paths[d + 1]].Y;

                canvas.StrokeSize = 14;
                canvas.StrokeColor = Colors.Yellow.WithAlpha(0.3f);
                canvas.DrawLine(x1, y1, x2, y2);

                canvas.StrokeSize = 8;
                canvas.StrokeColor = Colors.White;
                canvas.DrawLine(x1, y1, x2, y2);
            }

            canvas.StrokeSize = 3;
            canvas.StrokeColor = Colors.Yellow;
            foreach (int nodeIdx in Paths)
            {
                if (nodeIdx >= 0 && nodeIdx < NodesLocations.Count)
                {
                    canvas.DrawCircle(
                        NodesLocations[nodeIdx].X,
                        NodesLocations[nodeIdx].Y,
                        Radius + 3
                    );
                }
            }
        }
    }
}
