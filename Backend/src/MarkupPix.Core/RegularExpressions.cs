using System.Text.RegularExpressions;

namespace MarkupPix.Core;

/// <summary>
/// A regular expressions.
/// </summary>
public static class RegularExpressions
{
    /// <summary>
    /// A regular expression for english naming.
    /// </summary>
    public static readonly Regex EnglishName = new($"^[{EnglishWords}]{{0,}}$", RegexOptions.Compiled);

    /// <summary>
    /// A regular expression for english sentences.
    /// </summary>
    public static readonly Regex EnglishWithSpaces = new($"^[{EnglishWords}]+(\\s[{EnglishWords}]+)*$", RegexOptions.Compiled);

    /// <summary>
    /// A regular expression for email address.
    /// </summary>
    public static readonly Regex Email = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled);

    /// <summary>
    /// A regular expression for password.
    /// </summary>
    public static readonly Regex Password = new(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?]).{10,}$", RegexOptions.Compiled);

    /// <summary>
    /// A constant for storing characters of the English alphabet.
    /// </summary>
    private const string EnglishWords = "a-zA-Z";
}