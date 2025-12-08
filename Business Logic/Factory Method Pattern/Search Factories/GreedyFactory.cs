using AI_Graphs.StrategyPattern;

namespace AI_Graphs.FactoryMethodPattern;

// Concrete Creator
public class GreedyFactory : SearchAlgorithmFactory
{
    public override ISearchStrategy CreateAlgorithm()
    {
        return new GreedyBestFirst_Strategy();
    }
}
