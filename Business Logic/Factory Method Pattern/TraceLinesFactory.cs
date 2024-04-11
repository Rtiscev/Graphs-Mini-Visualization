using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	//Concrete Creators
	public class TraceLinesFactory : DrawingMethod
	{
		public override IDrawingMethod CreateDrawMethod()
		{
			return new TraceLines();
		}
	}
}
