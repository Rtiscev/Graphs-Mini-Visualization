namespace AI_Graphs.BusinessLogic.IteratorPattern
{
	// Aggregate Interface
	public interface IAbstractCollection
    {
		public interface IAdjacencyListAggregate
		{
			IAdjListIterator GetIterator();
		}
	}
}
