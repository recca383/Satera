namespace Satera_Api.ML
{
    public sealed record DataPreparationResults
    (
        int UsageSeconds,
        float GwaScaled,
        float AcademicUsageLog,
        float ProductivityUsageLog,
        float Social_usage_Log,
        float EntertainmentUsageLog,
        float FocusratioLog,
        float AcademicEffiencyLog,
        float UnlockIntensityLog,
        float SessionDepthLog,
        float PickupsLog      
        
    );
}
