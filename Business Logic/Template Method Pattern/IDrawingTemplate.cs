namespace AI_Graphs.Template_Method_Pattern
{
	// Template Method Pattern
	public abstract class IDrawingTemplate
	{
		public void Draw()
		{
			// Common drawing logic
			DrawLines();
			DrawSpecific();
			DrawNodes();
		}

		protected abstract void DrawLines();
		protected abstract void DrawNodes();
		protected abstract void DrawSpecific(); // Subclass-specific method
	}
}
