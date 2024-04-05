namespace AI_Graphs.Graphs
{
	public static class DFS
	{
		public static List<int> Search(Graph graph, int start, int end)
		{
			var visited = new bool[graph.numVertices];
			var path = new List<int>(); // Stores the path

			if (SearchHelper(graph, start, end, visited, path))
			{
				return path; // Path found, return it
			}

			return null; // No path found
		}

		private static bool SearchHelper(Graph graph, int current, int end, bool[] visited, List<int> path)
		{
			visited[current] = true;
			path.Add(current); // Add current vertex to the path

			if (current == end)
			{
				return true; // Goal reached, path found
			}

			foreach (var neighbor in graph.adjList[current])
			{
				if (!visited[neighbor.country])
				{
					if (SearchHelper(graph, neighbor.country, end, visited, path)) // Recursive call for unvisited neighbors
					{
						return true; // Path found through a neighbor
					}
				}
			}

			// Backtrack if no path found through current neighbors
			path.RemoveAt(path.Count - 1); // Remove current vertex from path (backtracking)
			return false;
		}
	}
}