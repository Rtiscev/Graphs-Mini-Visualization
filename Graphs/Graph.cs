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
	public interface IGraph9Adapter
	{
		List<List<Node9>> GetAdjacencyList();
	}

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

	public interface IGraph2Adapter
	{
		List<List<AStar.Node2>> GetAdjacencyList();
	}

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