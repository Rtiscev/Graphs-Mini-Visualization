namespace AI_Graphs.Graphs
{
	public class UniformCostSearch
	{
		private List<List<Node9>> adjList;

		public List<int> FindPath(List<List<Node9>> adjList, int startNode, int endNode)
		{
			List<int> path = new List<int>();
			HashSet<int> visited = new HashSet<int>();

			PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>((x, y) => x.Cost.CompareTo(y.Cost));
			Dictionary<int, int> parent = new Dictionary<int, int>();

			priorityQueue.Enqueue(new Node(startNode, 0));
			visited.Add(startNode);

			while (priorityQueue.Count > 0)
			{
				Node currentNode = priorityQueue.Dequeue();

				if (currentNode.Id == endNode)
				{
					path = ReconstructPath(parent, currentNode.Id);
					return path;
				}

				foreach (var neighbor in adjList[currentNode.Id])
				{
					if (!visited.Contains(neighbor.Id))
					{
						visited.Add(neighbor.Id);
						parent[neighbor.Id] = currentNode.Id;
						priorityQueue.Enqueue(new Node(neighbor.Id, currentNode.Cost + neighbor.Cost));
					}
				}
			}

			return null; // No path found
		}

		private List<int> ReconstructPath(Dictionary<int, int> parent, int currentNode)
		{
			List<int> path = new List<int>();
			while (parent.ContainsKey(currentNode))
			{
				path.Insert(0, currentNode);
				currentNode = parent[currentNode];
			}
			path.Insert(0, currentNode);
			return path;
		}

		public class Node
		{
			public int Id { get; }
			public int Cost { get; }

			public Node(int id, int cost)
			{
				Id = id;
				Cost = cost;
			}
		}

		// Priority Queue implementation
		public class PriorityQueue<T>
		{
			private List<T> data;
			private Comparison<T> comparison;

			public int Count { get { return data.Count; } }

			public PriorityQueue(Comparison<T> comparison)
			{
				this.data = new List<T>();
				this.comparison = comparison;
			}

			public void Enqueue(T item)
			{
				data.Add(item);
				int ci = data.Count - 1; // child index; start at end
				while (ci > 0)
				{
					int pi = (ci - 1) / 2; // parent index
					if (comparison(data[ci], data[pi]) >= 0)
						break; // child item is larger than (or equal) parent so we're done
					T tmp = data[ci];
					data[ci] = data[pi];
					data[pi] = tmp;
					ci = pi;
				}
			}

			public T Dequeue()
			{
				// assumes pq isn't empty; up to calling code
				int li = data.Count - 1; // last index (before removal)
				T frontItem = data[0];   // fetch the front
				data[0] = data[li];
				data.RemoveAt(li);

				--li; // last index (after removal)
				int pi = 0; // parent index. start at front of pq
				while (true)
				{
					int ci = pi * 2 + 1; // left child index of parent
					if (ci > li)
						break;  // no children so done
					int rc = ci + 1;     // right child
					if (rc <= li && comparison(data[rc], data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
						ci = rc;
					if (comparison(data[pi], data[ci]) <= 0)
						break; // parent is smaller than (or equal to) smallest child so done
					T tmp = data[pi];
					data[pi] = data[ci];
					data[ci] = tmp; // swap parent and child
					pi = ci;
				}
				return frontItem;
			}
		}
	}


	public class Node9
	{
		public int Id { get; }
		public int Cost { get; }

		public Node9(int id, int cost)
		{
			Id = id;
			Cost = cost;
		}
	}
}
