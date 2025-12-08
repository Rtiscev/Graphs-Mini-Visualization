using AI_Graphs.Graphs;

namespace AI_Graphs.ProxyPattern;

// Subject Interface
public interface ISearchService
{
    List<int> FindPath(Graph graph, int start, int end, Dictionary<int, int> heuristics);
    long GetExecutionTime();
    string GetAlgorithmName();
}
