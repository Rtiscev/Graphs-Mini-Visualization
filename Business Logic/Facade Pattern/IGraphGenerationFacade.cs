using AI_Graphs.Utils;
using Microsoft.Maui.Graphics;
using System.Numerics;

namespace AI_Graphs.FacadePattern;

public interface IGraphGenerationFacade
{
    List<MyPoint> GenerateValidPoints(int width, int height);
    List<Color> GenerateColorScheme();
    float GenerateRandomValue(double range, double min);
    bool CheckPointExists(List<Dictionary<int, Vector2>> collection, int key);
}
