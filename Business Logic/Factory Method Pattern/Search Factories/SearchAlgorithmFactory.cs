using AI_Graphs.StrategyPattern;

namespace AI_Graphs.FactoryMethodPattern;

// Creator
public abstract class SearchAlgorithmFactory
{
    public abstract ISearchStrategy CreateAlgorithm();
    public string GetAlgorithmName()
    {
        var algorithm = CreateAlgorithm();
        return algorithm.GetType().Name.Replace("_Strategy", "");
    }
}
