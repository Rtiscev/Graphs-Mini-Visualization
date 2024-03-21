namespace AI_Graphs.Graphs
{
	public static class AStar
	{
		private static List<List<Node2>> _adjList;
		private static Dictionary<int, int> _heuristicDistances;

		public static List<int> FindPath(List<List<Node2>> adjList, Dictionary<int, int> heuristicDistances, int startNode, int endNode)
		{
			_adjList = adjList;
			_heuristicDistances = heuristicDistances;

			List<int> path = new List<int>();
			HashSet<int> visited = new HashSet<int>();

			PriorityQueue<Node2> priorityQueue = new PriorityQueue<Node2>((x, y) => x.TotalCost.CompareTo(y.TotalCost));
			Dictionary<int, int> gCosts = new Dictionary<int, int>();
			Dictionary<int, int> parent = new Dictionary<int, int>();

			priorityQueue.Enqueue(new Node2(startNode, 0, _heuristicDistances[startNode]));
			gCosts[startNode] = 0;
			visited.Add(startNode);

			while (priorityQueue.Count > 0)
			{
				Node2 currentNode = priorityQueue.Dequeue();

				if (currentNode.Id == endNode)
				{
					path = ReconstructPath(parent, currentNode.Id);
					return path;
				}

				foreach (var neighbor in _adjList[currentNode.Id])
				{
					int tentativeGCost = gCosts[currentNode.Id] + neighbor.Weight;
					if (!gCosts.ContainsKey(neighbor.Country) || tentativeGCost < gCosts[neighbor.Country])
					{
						gCosts[neighbor.Country] = tentativeGCost;
						int totalCost = tentativeGCost + _heuristicDistances[neighbor.Country];
						if (!visited.Contains(neighbor.Country))
						{
							visited.Add(neighbor.Country);
							parent[neighbor.Country] = currentNode.Id;
							priorityQueue.Enqueue(new Node2(neighbor.Country, tentativeGCost, totalCost));
						}
					}
				}
			}

			return null; // No path found
		}

		private static List<int> ReconstructPath(Dictionary<int, int> parent, int currentNode)
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

		public struct Node2
		{
			public int Country { get; set; }
			public int Weight { get; set; }
			public int Id { get; set; }
			public int TotalCost { get; set; }

			public Node2(int country, int weight, int totalCost)
			{
				Country = country;
				Weight = weight;
				Id = country; // Assuming Id is same as Country in this context
				TotalCost = totalCost;
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
}