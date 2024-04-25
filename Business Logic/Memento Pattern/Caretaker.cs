namespace AI_Graphs.Business_Logic.Memento_Pattern
{
	// Caretaker: Manages the memento objects
	public class Caretaker
	{
		private List<Memento> _mementos = new();

		public void SetMemento(Memento memento)
		{
			_mementos.Add(memento);
		}

		public Memento GetMemento(int index)
		{
			return _mementos[index];
		}

		public int GetCount()
		{
			return _mementos.Count;
		}
	}
}
