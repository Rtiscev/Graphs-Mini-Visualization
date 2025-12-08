using AI_Graphs.StrategyPattern;

namespace AI_Graphs.FactoryMethodPattern;

// Concrete Creator
public class BFSFactory : SearchAlgorithmFactory
{
    public override ISearchStrategy CreateAlgorithm()
    {
        return new BFS_Strategy();
    }
}
