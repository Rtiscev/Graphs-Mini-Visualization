using static AI_Graphs.BusinessLogic.IteratorPattern.IAbstractCollection;
using AI_Graphs.Graphs;

namespace AI_Graphs.BusinessLogic.IteratorPattern
{
	public class AdjacencyList : IAdjacencyListAggregate
	{
		public List<List<Node>> AdjList { get; private set; }

		public AdjacencyList(List<List<Node>> adjList)
		{
			AdjList = adjList;
		}

		public IAdjListIterator GetIterator()
		{
			return new AdjListIterator(AdjList); // Reuse the existing iterator class
		}
	}
}
