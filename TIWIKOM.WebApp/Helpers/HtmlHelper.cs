using System.Text.RegularExpressions;

namespace TIWIKOM.WebApp.Helpers;

public static class TextHelper
{
    /// <summary>
    /// Strips HTML tags from a string
    /// </summary>
    public static string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        // Remove HTML tags
        var stripped = Regex.Replace(html, "<.*?>", string.Empty);
        
        // Decode HTML entities
        stripped = System.Net.WebUtility.HtmlDecode(stripped);
        
        // Remove extra whitespace
        stripped = Regex.Replace(stripped, @"\s+", " ").Trim();
        
        return stripped;
    }

    /// <summary>
    /// Gets a preview of HTML content (plain text, truncated)
    /// </summary>
    public static string GetPreview(string html, int maxLength = 150)
    {
        var plainText = StripHtml(html);
        
        if (plainText.Length <= maxLength)
            return plainText;
        
        return plainText.Substring(0, maxLength) + "...";
    }
}
