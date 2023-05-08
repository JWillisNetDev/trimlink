namespace trimlink.core.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    TimeZoneInfo TimeZone { get; }
}