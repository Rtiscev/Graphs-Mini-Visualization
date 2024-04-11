using AI_Graphs.BusinessLogic.DecoratorPattern;
using AI_Graphs.BusinessLogic.Utils;
using AI_Graphs.Graphs;
using AI_Graphs.SingletonPattern;
using AI_Graphs.Utils;

#pragma warning disable

namespace AI_Graphs.DrawingMethods
{
	// concrete product
	class SimpleDraw : IDrawingMethod
	{
		private PrimitiveDrawing primitiveDrawing;
		public SimpleDraw()
		{
			DataCollection.GetInstance().NodesLocations.Clear();
			Radius = DataCollection.GetInstance().Radius;
			Amount = DataCollection.GetInstance().Amount;
			AdjList = DataCollection.GetInstance().AdjList;
			ColorsList = DataCollection.GetInstance().ColorsList;
			Paths = DataCollection.GetInstance().Paths;
		}

		public event EventHandler<ByteArrayEventArgs> SendBytes;

		public override async Task Draw(ICanvas canvas, RectF dirtyRect)
		{
			// update singleton data
			DataCollection.GetInstance().NodesLocations = AI_Graphs.Utils.Utils.GeneratePoints((int)dirtyRect.Width, (int)dirtyRect.Height, Amount, (int)Radius);
			DataCollection.GetInstance().ColorsList ??= AI_Graphs.Utils.Utils.RandomColors(Amount);
			
			primitiveDrawing=new(ref canvas);
			primitiveDrawing.Draw();
			//primitiveDrawing.DrawLines();
			//primitiveDrawing.DrawPaths();
			//primitiveDrawing.DrawNodes();
		}
	}
}