using trimlink.core.Interfaces;

namespace trimlink.core;

internal class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public TimeZoneInfo TimeZone => TimeZoneInfo.Local;
}