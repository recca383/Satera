namespace Satera_Api.Application
{
    public record GetMLAnalysisCommand(
        float Gwa,
        int TrackingDurationDays,
        int TotalScreenTime,
        int TotalAppsTracked,
        int Pickups,
        int DeviceUnlocks,
        App[] Apps,
        string CollectionTimestamp,
        string Platform
        );
}
