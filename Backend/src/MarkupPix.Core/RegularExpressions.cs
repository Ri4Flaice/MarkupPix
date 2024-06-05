using System.Text.RegularExpressions;

namespace MarkupPix.Core;

/// <summary>
/// A regular expressions.
/// </summary>
public static class RegularExpressions
{
    /// <summary>
    /// A regular expression for email address.
    /// </summary>
    public static readonly Regex Email = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled);

    /// <summary>
    /// A regular expression for password.
    /// </summary>
    public static readonly Regex Password = new(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?]).{10,}$", RegexOptions.Compiled);
}