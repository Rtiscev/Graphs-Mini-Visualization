using AI_Graphs.DrawingMethods;

namespace AI_Graphs.FactoryMethodPattern
{
	//Creator (Abstract Class)
	public abstract class DrawingMethodFactory
	{
		protected abstract IDrawingMethod CreateDrawingMethod();
		public IDrawingMethod CreateMethod()
		{
			return this.CreateDrawingMethod();
		}
	}
}
