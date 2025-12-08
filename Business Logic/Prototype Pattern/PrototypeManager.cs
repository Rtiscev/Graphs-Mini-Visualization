using AI_Graphs.Business_Logic.Prototype_Pattern;

namespace AI_Graphs.PrototypePattern;

public class PrototypeManager
{
    private Dictionary<string, IGraphPrototype> prototypes;

    public PrototypeManager()
    {
        prototypes = new Dictionary<string, IGraphPrototype>();
    }

    public void AddPrototype(string key, IGraphPrototype prototype)
    {
        prototypes[key] = prototype;
    }

    public IGraphPrototype GetPrototype(string key)
    {
        if (prototypes.ContainsKey(key))
        {
            return prototypes[key].Clone();
        }
        return null;
    }

    public void RemovePrototype(string key)
    {
        if (prototypes.ContainsKey(key))
        {
            prototypes.Remove(key);
        }
    }

    public bool HasPrototype(string key)
    {
        return prototypes.ContainsKey(key);
    }
}
