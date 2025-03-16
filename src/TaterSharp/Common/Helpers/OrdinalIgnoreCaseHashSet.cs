namespace TaterSharp.Common.Helpers;
public class OrdinalIgnoreCaseHashSet : HashSet<string>
{
    public OrdinalIgnoreCaseHashSet() : base(StringComparer.OrdinalIgnoreCase)
    {

    }
}
