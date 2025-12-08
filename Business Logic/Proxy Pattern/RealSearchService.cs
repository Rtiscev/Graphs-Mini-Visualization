using AI_Graphs.Graphs;
using AI_Graphs.StrategyPattern;
using System.Diagnostics;

namespace AI_Graphs.ProxyPattern;

// Real Subject
public class RealSearchService : ISearchService
{
    private readonly ISearchStrategy strategy;
    private readonly string algorithmName;
    private long executionTimeMs;

    public RealSearchService(ISearchStrategy strategy, string name)
    {
        this.strategy = strategy;
        this.algorithmName = name;
    }

    public List<int> FindPath(Graph graph, int start, int end, Dictionary<int, int> heuristics)
    {
        Console.WriteLine($"[RealService] Executing {algorithmName}...");

        var stopwatch = Stopwatch.StartNew();

        SearchContext ctx = new SearchContext();
        ctx.SetSearchStrategy(strategy);
        List<int> path = ctx.FindPath(graph, start, end, heuristics);

        stopwatch.Stop();
        executionTimeMs = stopwatch.ElapsedMilliseconds;

        return path;
    }

    public long GetExecutionTime() => executionTimeMs;

    public string GetAlgorithmName() => algorithmName;
}
