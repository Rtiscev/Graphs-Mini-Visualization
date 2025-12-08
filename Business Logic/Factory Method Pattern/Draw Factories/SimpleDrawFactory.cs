using AI_Graphs.DrawingMethods;
using AI_Graphs.SingletonPattern;

namespace AI_Graphs.FactoryMethodPattern
{
	// Concrete Creator
	public class SimpleDrawFactory : DrawingMethodFactory
	{
		public override IDrawingMethod Create()
		{
			var method = new SimpleDraw();
			DataCollection.GetInstance().NodesLocations.Clear();
			// update singleton data
			DataCollection.GetInstance().ColorsList ??= AI_Graphs.Utils.Utils.RandomColors();
			return method;
		}
	}
}
