using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	// Concrete Creators
	public class SaveGraphImageFactory : DrawingMethodFactory
	{
		public override IDrawingMethod Create()
		{
			return new SaveGraphImage();
		}
	}
}
