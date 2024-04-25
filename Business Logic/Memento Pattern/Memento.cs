namespace AI_Graphs.Business_Logic.Memento_Pattern
{
	// Memento: Represents the state to be stored
	public class Memento
	{
		private readonly byte[] _state;

		public Memento(byte[] state)
		{
			_state = state;
		}

		public byte[] GetState()
		{
			return _state;
		}
	}
}
