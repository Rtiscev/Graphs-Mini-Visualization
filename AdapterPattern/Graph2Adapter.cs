using AI_Graphs.Graphs;

namespace AI_Graphs.AdapterPattern
{
	// Adapter
	public class Graph2Adapter : IGraph2Adapter
	{
		private Graph graph;
		public Graph2Adapter(Graph graph)
		{
			this.graph = graph;
		}
		public List<List<AStar.Node2>> GetAdjacencyList()
		{
			var graph2 = new List<List<AStar.Node2>>();
			foreach (var ff in graph.adjList)
			{
				var minigraph2 = new List<AStar.Node2>();
				foreach (var gg in ff)
				{
					minigraph2.Add(new AStar.Node2() { Country = gg.country, Weight = gg.weight, Id = default, TotalCost = default });
				}
				graph2.Add(minigraph2);
			}
			return graph2;
		}
	}
}
