namespace Satera_Api
{
    public sealed record Request(
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

    public sealed class App
    {
        public required string PackageName { get; set; }
        public int TotalTimeInForeground { get; set; }
    }

}