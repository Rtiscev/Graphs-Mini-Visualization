namespace AI_Graphs.Graphs
{
	public static class BidirectionalSearch
	{
		public static void BFS2(Graph graph1, Queue<int> queue, bool[] visited, int[] parent)
		{
			int current = queue.Dequeue();
			foreach (var i in graph1.adjList[current])
			{
				// If adjacent vertex is not visited earlier
				// mark it visited by assigning true country
				if (!visited[i.country])
				{
					// set current as parent of this vertex
					parent[i.country] = current;

					// Mark this vertex visited
					visited[i.country] = true;

					// Push to the end of queue
					queue.Enqueue(i.country);
				}
			}
		}
		// check for intersecting vertex
		public static int IsIntersecting(Graph graph1, bool[] s_visited,
								  bool[] t_visited)
		{
			for (int i = 0; i < graph1.numVertices; i++)
			{
				// if a vertex is visited by both front
				// and back BFS search return that node
				// else return -1
				if (s_visited[i] && t_visited[i])
					return i;
			}
			return -1;
		}
		// Print the path from source to target
		public static List<int> PrintPath(int[] s_parent, int[] t_parent,
							  int s, int t, int intersectNode)
		{
			List<int> path = new List<int>();
			path.Add(intersectNode);
			int i = intersectNode;
			while (i != s)
			{
				path.Add(s_parent[i]);
				i = s_parent[i];
			}
			path.Reverse();
			i = intersectNode;
			while (i != t)
			{
				path.Add(t_parent[i]);
				i = t_parent[i];
			}
			return path;
			//Console.WriteLine("*****Path*****");
			//foreach (int it in path) Console.Write(it + " ");
			//Console.WriteLine();
		}
		// Method for bidirectional searching
		public static List<int> BiDirSearch(Graph graph1, int s, int t)
		{
			int V = graph1.numVertices;
			// boolean array for BFS started from
			// source and target(front and backward BFS)
			// for keeping track on visited nodes
			bool[] s_visited = new bool[V];
			bool[] t_visited = new bool[V];

			// Keep track on parents of nodes
			// for front and backward search
			int[] s_parent = new int[V];
			int[] t_parent = new int[V];

			// queue for front and backward search
			Queue<int> s_queue = new Queue<int>();
			Queue<int> t_queue = new Queue<int>();

			int intersectNode = -1;

			// necessary initialization
			for (int i = 0; i < V; i++)
			{
				s_visited[i] = false;
				t_visited[i] = false;
			}

			s_queue.Enqueue(s);
			s_visited[s] = true;

			// parent of source is set to -1
			s_parent[s] = -1;

			t_queue.Enqueue(t);
			t_visited[t] = true;

			// parent of target is set to -1
			t_parent[t] = -1;

			while (s_queue.Count > 0 && t_queue.Count > 0)
			{
				// Do BFS from source and target vertices
				BFS2(graph1, s_queue, s_visited, s_parent);
				BFS2(graph1, t_queue, t_visited, t_parent);

				// check for intersecting vertex
				intersectNode = IsIntersecting(graph1, s_visited, t_visited);

				// If intersecting vertex is found
				// that means there exist a path
				if (intersectNode != -1)
				{
					Console.WriteLine(
						"Path exist between {0} and {1}", s, t);
					Console.WriteLine("Intersection at: {0}",
									  intersectNode);

					// print the path and exit the program
					//PrintPath(s_parent, t_parent, s, t,intersectNode);
					return PrintPath(s_parent, t_parent, s, t, intersectNode);
					//Environment.Exit(0);
					//break;
				}
			}
			return null;
		}
	}


}
