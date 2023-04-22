using System.Text.Json.Serialization;

namespace trimlink.website.Contracts;

public class LinkCreate
{
    public string RedirectToUrl { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public bool IsNeverExpires { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public TimeSpan GetDurationTimeSpan()
    {
        if (TimeSpan.TryParse(Duration, out TimeSpan asTimeSpan))
            return asTimeSpan;
        return TimeSpan.Zero;
    }
}