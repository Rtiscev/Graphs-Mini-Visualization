using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern
{
	public class BFS_Strategy : ISearchStrategy
	{
		public List<int> Search(Graph graph, int start, int end, Dictionary<int, int> heuristicDistances)
		{
			return BFS.Search(graph, start, end);
		}
	}
}
