using AI_Graphs.StrategyPattern;

namespace AI_Graphs.FactoryMethodPattern;

// Concrete Creator
public class DFSFactory : SearchAlgorithmFactory
{
    public override ISearchStrategy CreateAlgorithm()
    {
        return new DFS_Strategy();
    }
}
