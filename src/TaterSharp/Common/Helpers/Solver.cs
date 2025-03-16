using TaterSharp.Common.ApiModels;

namespace TaterSharp.Common.Helpers;
public static class Solver
{
    public static SingleBlockSubmission Solve(string lastHash, string minerId, string color)
    {
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
