using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	//Concrete Creators
	public class TraceLinesFactory : DrawingMethodFactory
	{
		protected override IDrawingMethod CreateDrawingMethod()
		{
			return new TraceLines();
		}
	}
}
