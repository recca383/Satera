namespace Satera_Api
{
    public sealed record Response(
        float AcademicAtRiskScore,
        float AverageBalancedUserScore,
        float DigitalMultitaskerScore,
        float DigitalSelfRegulatedScore,
        float HighFunctioningAcademicScore,
        float MinimalDigitalengagerScore,
        DateTime DateAnalyzed
        );
}
