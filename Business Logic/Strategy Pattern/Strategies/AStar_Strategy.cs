using AI_Graphs.Graphs;

namespace AI_Graphs.StrategyPattern;

// Concrete Product
public class AStar_Strategy : ISearchStrategy
{
	public List<int> Search(Graph graph, int start, int end, Dictionary<int, int> heuristicDistances)
	{
		return AStar.FindPath(graph, heuristicDistances, start, end);
	}
}
