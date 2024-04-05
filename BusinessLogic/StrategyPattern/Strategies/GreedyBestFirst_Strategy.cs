using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern
{
	public class GreedyBestFirst_Strategy : ISearchStrategy
	{
		public List<int> Search(Graph graph, int start, int end, Dictionary<int, int> heuristicDistances)
		{
			return GreedyBestFirstSearch.FindPath(graph, heuristicDistances, start, end);
		}
	}
}
