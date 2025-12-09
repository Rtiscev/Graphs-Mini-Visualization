namespace AI_Graphs.FacadePattern;

public struct MyPoint
{
    public float X { get; set; }
    public float Y { get; set; }

    public MyPoint()
    {
        X = 0;
        Y = 0;
    }

    public MyPoint(float x, float y)
    {
        X = x;
        Y = y;
    }

    public MyPoint(double x, double y)
    {
        X = (float)x;
        Y = (float)y;
    }

    public override string ToString() => $"({X}, {Y})";
}
