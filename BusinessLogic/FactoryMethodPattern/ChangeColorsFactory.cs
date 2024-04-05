using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	//Concrete Creators
	public class ChangeColorsFactory : DrawingMethodFactory
	{
		protected override IDrawingMethod CreateDrawingMethod()
		{
			return new ChangeColors();
		}
	}
}
