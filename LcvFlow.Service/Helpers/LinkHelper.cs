using LcvFlow.Service.Extensions;

namespace LcvFlow.Service.Helpers;

public static class LinkHelper
{
    public static string CreateSlug(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        var str = text.ToSlug();

        return str;
    }
    public static string GenerateRsvpLink(string baseUrl, string eventSlug, string accessToken)
    {
        return $"{baseUrl}/rsvp/{eventSlug}/{accessToken}";
    }
}