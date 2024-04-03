using AI_Graphs.Graphs;

namespace AI_Graphs.AdapterPattern
{
	// Adapter
	public class Graph9Adapter : IGraph9Adapter
	{
		private Graph graph;
		public Graph9Adapter(Graph graph)
		{
			this.graph = graph;
		}
		public List<List<Node9>> GetAdjacencyList()
		{
			List<List<Node9>> adjListConverted = new();

			foreach (var ff in graph.adjList)
			{
				var minigraph = new List<Node9>();
				foreach (var gg in ff)
				{
					minigraph.Add(new Node9(gg.country, gg.weight));
				}
				adjListConverted.Add(minigraph);
			}

			return adjListConverted;
		}
	}
}
