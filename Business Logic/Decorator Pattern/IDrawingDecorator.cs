using AI_Graphs.Template_Method_Pattern;

namespace AI_Graphs.Business_Logic.Decorator_Pattern
{
	// Decorator base class
	public abstract class IDrawingDecorator : IDrawingTemplate
	{
		protected IDrawingTemplate drawingMethod;

		protected IDrawingDecorator(IDrawingTemplate drawingMethod)
		{
			this.drawingMethod = drawingMethod;
		}

		public virtual void Draw()
		{
			// Call the draw method of the wrapped component
			drawingMethod.Draw();
		}
	}
}
