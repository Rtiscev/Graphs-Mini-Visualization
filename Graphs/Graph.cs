namespace AI_Graphs.Graphs
{
	// Adaptee
	public class Graph
	{
		public int numVertices;
		public List<List<Node>> adjList;

		public Graph(int numVertices)
		{
			this.numVertices = numVertices;
			adjList = new List<List<Node>>(numVertices);

			// Initialize adjacency list with empty lists
			for (int i = 0; i < numVertices; i++)
			{
				adjList.Add(new List<Node>());
			}
		}

		public void AddEdge(int source, int destination, int weight)
		{
			adjList[source].Add(new Node(destination, weight));
		}
	}
	public struct Node
	{
		public int country;
		public int weight;
		public Node() { }
		public Node(int c, int w)
		{
			country = c; weight = w;
		}
	}

	// ITarget
	public interface IGraphAdapter
	{
		List<List<Node9>> GetAdjacencyList();
	}

	// Adapter
	public class GraphAdapter : IGraphAdapter
	{
		private Graph graph;
		public GraphAdapter(Graph graph)
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