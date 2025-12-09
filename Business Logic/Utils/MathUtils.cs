using System.Numerics;

namespace AI_Graphs.Utils;

public static class MathUtils
{
    public static float GetRandomFloat(double range, double min)
    {
        Random rand = new();
        double sample = rand.NextDouble();
        double scaled = sample * range + min;
        return (float)scaled;
    }

    public static bool CheckIfKeyExists(List<Dictionary<int, Vector2>> listik, int key)
    {
        foreach (var item in listik)
        {
            if (item.ContainsKey(key))
            {
                return true;
            }
        }
        return false;
    }
}
