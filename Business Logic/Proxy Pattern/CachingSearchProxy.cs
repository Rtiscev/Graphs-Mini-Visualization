using AI_Graphs.Graphs;
using AI_Graphs.StrategyPattern;

namespace AI_Graphs.ProxyPattern;

// Caching Proxy
public class CachingSearchProxy : ISearchService
{
    private readonly RealSearchService realService;
    private readonly Dictionary<string, (List<int> path, long time)> cache;
    private string lastCacheKey;

    public CachingSearchProxy(ISearchStrategy strategy, string algorithmName)
    {
        this.realService = new RealSearchService(strategy, algorithmName);
        this.cache = new Dictionary<string, (List<int>, long)>();
    }

    public List<int> FindPath(Graph graph, int start, int end, Dictionary<int, int> heuristics)
    {
        // Create cache key based on algorithm name and search parameters
        string cacheKey = $"{GetAlgorithmName()}_{start}_{end}";
        lastCacheKey = cacheKey;

        // Check cache first (Proxy intercepts the call)
        if (cache.ContainsKey(cacheKey))
        {
            Console.WriteLine($"✓ [PROXY] Cache HIT for {GetAlgorithmName()}: {start} → {end}");
            return cache[cacheKey].path;
        }

        // Cache miss - delegate to real service
        Console.WriteLine($"✗ [PROXY] Cache MISS for {GetAlgorithmName()}: {start} → {end}");
        List<int> result = realService.FindPath(graph, start, end, heuristics);
        long time = realService.GetExecutionTime();

        // Store in cache for future use
        cache[cacheKey] = (result, time);

        return result;
    }

    public long GetExecutionTime()
    {
        // If cache hit, return cached time
        if (lastCacheKey != null && cache.ContainsKey(lastCacheKey))
        {
            return cache[lastCacheKey].time;
        }
        return realService.GetExecutionTime();
    }

    public string GetAlgorithmName() => realService.GetAlgorithmName();

    // Extra methods for cache management
    public void ClearCache()
    {
        cache.Clear();
        Console.WriteLine($"[PROXY] Cache cleared for {GetAlgorithmName()}");
    }

    public int GetCacheSize() => cache.Count;

    public bool IsCached(int start, int end)
    {
        string key = $"{GetAlgorithmName()}_{start}_{end}";
        return cache.ContainsKey(key);
    }
}
