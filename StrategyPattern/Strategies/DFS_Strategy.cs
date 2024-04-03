using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern
{
	public class DFS_Strategy : ISearchStrategy
	{
		public List<int> Search(Graph graph, int start, int end, Dictionary<int, int> heuristicDistances)
		{
			return DFS.Search(graph, start, end);
		}
	}
}
