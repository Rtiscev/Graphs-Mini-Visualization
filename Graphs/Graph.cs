namespace AI_Graphs.Graphs
{
	// Adaptee for IGraph2Adapter and IGraph9Adapter
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
}