namespace TaterSharp.CLI.Mining;
public class CompanyMiner
{
    public string MinerId { get; private set; }
    public string Color { get; private set; }
    public CompanyMiner(string minerId)
    {
        MinerId = minerId;
        Color = $"#{minerId}";
    }
}
