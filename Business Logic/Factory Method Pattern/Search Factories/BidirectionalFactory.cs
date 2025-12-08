using AI_Graphs.StrategyPattern;

namespace AI_Graphs.FactoryMethodPattern;

// Concrete Creator
public class BidirectionalFactory : SearchAlgorithmFactory
{
    public override ISearchStrategy CreateAlgorithm()
    {
        return new Bidirectional_Strategy();
    }
}
