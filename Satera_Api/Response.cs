namespace Satera_Api
{
    public sealed record Response(
        float Score,
        string Label,
        Dictionary<string, float> CategoryScores,
        DateTime DateAnalyzed
        );
}
