using Microsoft.Maui.Graphics;

namespace AI_Graphs.FacadePattern;

internal class ColorGenerationSubsystem
{
    private readonly Random _random;
    private const int MaxColorValue = 256;

    public ColorGenerationSubsystem(Random random) => _random = random;

    public List<Color> GenerateRandomColors(int count)
    {
        List<Color> colors = new List<Color>(count);

        for (int i = 0; i < count; i++)
        {
            colors.Add(GenerateSingleColor());
        }

        return colors;
    }

    private Color GenerateSingleColor()
    {
        int r = _random.Next(MaxColorValue);
        int g = _random.Next(MaxColorValue);
        int b = _random.Next(MaxColorValue);

        return new Color(r, g, b);
    }
}
