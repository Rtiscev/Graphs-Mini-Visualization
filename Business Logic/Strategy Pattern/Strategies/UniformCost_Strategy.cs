using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern
{
	public class UniformCost_Strategy : ISearchStrategy
	{
		public List<int> Search(Graph graph, int start, int end, Dictionary<int, int> heuristicDistances)
		{
			return UniformCostSearch.FindPath(graph, start, end);
		}
	}
}
