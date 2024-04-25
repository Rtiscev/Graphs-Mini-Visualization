using AI_Graphs.BusinessLogic.IteratorPattern;

namespace AI_Graphs.Graphs
{
	public static class BFS
	{
		public static List<int> Search(Graph graph, int start, int goal)
		{
			var visited = new bool[graph.numVertices];
			var queue = new Queue<int>();
			var parent = new int[graph.numVertices]; // Added for path reconstruction

			visited[start] = true;
			queue.Enqueue(start);
			parent[start] = -1; // Parent of starting vertex is -1 (no parent)

			while (queue.Count > 0)
			{
				var current = queue.Dequeue();

				if (current == goal)
				{
					// Path found, reconstruct the path from goal to start
					return ReconstructPath(parent, goal);
				}

				AdjencyListModel ADJ = new(graph.adjList);
				IAdjListIterator iterator = ADJ.AdjList.GetIterator();

				while (iterator.HasNext())
				{
					Node node = iterator.GetCurrent();
					// Process the node
					if (!visited[node.country])
					{
						visited[node.country] = true;
						queue.Enqueue(node.country);
						parent[node.country] = current; // Track the parent of the neighbor for path reconstruction
					}
				}
			}

			return null; // No path found
		}

		private static List<int> ReconstructPath(int[] parent, int goal)
		{
			var path = new List<int>();
			var current = goal;

			while (current != -1)
			{
				path.Insert(0, current);
				current = parent[current];
			}

			return path;
		}
	}
}
