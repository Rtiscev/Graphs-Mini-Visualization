using System.Numerics;

namespace AI_Graphs.Utils
{
    public static class Utils
    {
        public static float GetRandomFloat(double range, double min)
        {
            Random rand = new();
            double sample = rand.NextDouble();
            double scaled = sample * range + min;
            return (float)scaled;
        }
        public static List<Color> RandomColors(int amount)
        {
            List<Color> newColors = new();

            Random random = new();
            for (int i = 0; i < amount; i++)
            {
                int randomR = random.Next(256);
                int randomG = random.Next(256);
                int randomB = random.Next(256);
                Color color = new(randomR, randomG, randomB);
                newColors.Add(color);
            }

            return newColors;
        }
        public static List<MyPoint> GeneratePoints(int width, int height, int numPoints, int minDistance)
        {
            int maxAttempts = 1000;
            List<MyPoint> points = new List<MyPoint>();
            Random random = new();
            int attempts = 0;

            while (points.Count < numPoints)
            {
                int x = random.Next(minDistance * 2 + 1, width - minDistance * 2 - 1);
                int y = random.Next(minDistance * 2 + 1, height - minDistance * 2 - 1);
                bool valid = true;

                // Check for collisions with existing points
                foreach (var existingPoint in points)
                {
                    bool isOutOfBorders =
                        x + minDistance * 2 >= width ||
                        y + minDistance * 2 >= height ||
                        x - minDistance * 2 <= 0 ||
                        y - minDistance * 2 <= 0;

                    // Calculate the distance between the centers of the circles
                    float distance = (float)Math.Sqrt(Math.Pow(existingPoint.X - x, 2) + Math.Pow(existingPoint.Y - y, 2));

                    // Check if the distance is less than the sum of the radii
                    bool isIntersecting = distance <= 4 * minDistance + 4 * minDistance;

                    if (isIntersecting || isOutOfBorders)
                    {
                        valid = false;
                        break;
                    }
                    else if (attempts > maxAttempts)
                    { valid = true; break; }
                }

                if (valid)
                {
                    points.Add(new MyPoint(x, y));
                    attempts = 0;
                }

                attempts++;
            }

            return points;
        }
        public static bool CheckIfKeyExists(List<Dictionary<int, Vector2>> listik, int key)
        {
            bool exists = false;
            foreach (var item in listik)
            {
                if (item.ContainsKey(key))
                {
                    exists = true;
                    break;
                }
            }
            return exists;

        }
    }
    public struct MyPoint
    {
        public float X; public float Y;
        public MyPoint()
        {

        }
        public MyPoint(float x, float y)
        {
            X = x; Y = y;
        }
        public MyPoint(double x, double y)
        {
            X = (float)x; Y = (float)y;
        }
    }
}