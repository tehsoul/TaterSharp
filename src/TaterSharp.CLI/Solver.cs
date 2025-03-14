using TaterSharp.CLI.ApiModels;

namespace TaterSharp.CLI;
public static class Solver
{
    public static SingleBlockSubmission Solve(string companyId, string minerId, string lastHash)
    {
        var color = $"#{companyId}";
        string stringToHash = $"{lastHash} {minerId} {color}";

        var hash = Sha256Helper.GetSha256HexDigest(stringToHash);

        return new SingleBlockSubmission
        {
            Hash = hash,
            MinerId = minerId,
            Color = color
        };
    }
}
