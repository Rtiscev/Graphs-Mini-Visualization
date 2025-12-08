using AI_Graphs.BusinessLogic.DecoratorPattern;
using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.Graphs;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;

#pragma warning disable

namespace AI_Graphs.DrawingMethods;

// Concrete Product
class SimpleDraw : IDrawingMethod
{
    private PrimitiveDrawing primitiveDrawing;
    public event EventHandler<ByteArrayEventArgs> SendBytes;

    public override async Task Draw(ICanvas canvas, RectF dirtyRect)
    {
        //// update singleton data
        DataCollection.GetInstance().NodesLocations = AI_Graphs.Utils.Utils.GeneratePoints((int)dirtyRect.Width, (int)dirtyRect.Height);
        //DataCollection.GetInstance().ColorsList ??= AI_Graphs.Utils.Utils.RandomColors();
        primitiveDrawing = new(ref canvas);
        primitiveDrawing.Draw();
    }
}