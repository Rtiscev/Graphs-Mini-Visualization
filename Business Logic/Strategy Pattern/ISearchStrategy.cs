using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern
{
	interface ISearchStrategy
    {
		public List<int> Search(Graph graph,int start, int end, Dictionary<int, int> heuristicDistances);
	}
}
