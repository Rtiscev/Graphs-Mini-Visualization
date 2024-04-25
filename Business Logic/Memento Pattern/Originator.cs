namespace AI_Graphs.Business_Logic.Memento_Pattern
{
	// Originator: Creates and restores the state from memento objects
	public class Originator
	{
		public byte[] _workingImage;

		public Memento CreateMemento()
		{
			// Create a memento with a copy of the current state
			return new Memento(_workingImage);
		}

		public void SetMemento(Memento memento)
		{
			// Restore the state from the memento
			_workingImage = memento.GetState();
		}

		public byte[] GetState()
		{
			return _workingImage;
		}
	}
}
