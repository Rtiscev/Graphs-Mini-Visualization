using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern
{
	class SearchContext
	{
		private ISearchStrategy sarchStrategy;
		public void SetSearchStrategy(ISearchStrategy strategy)
		{
			sarchStrategy = strategy;
		}
		public List<int> FindPath(Graph graph, int start, int end, Dictionary<int, int> heuristicDistances)
		{
			return sarchStrategy.Search(graph, start, end, heuristicDistances);
		}
	}
}
