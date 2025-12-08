using AI_Graphs.StrategyPattern;

namespace AI_Graphs.FactoryMethodPattern;

// Concrete Creator
public class AStarFactory : SearchAlgorithmFactory
{
    public override ISearchStrategy CreateAlgorithm()
    {
        return new AStar_Strategy();
    }
}
