using System.Text.RegularExpressions;

namespace MarkupPix.Core;

/// <summary>
/// A regular expressions.
/// </summary>
public static class RegularExpressions
{
    /// <summary>
    /// A regular expression for english sentences with numbers.
    /// </summary>
    public static readonly Regex EnglishWithSpacesAndNumbers = new($"^[{EnglishWordsWithNumbers}]+(\\s[{EnglishWordsWithNumbers}]+)*$", RegexOptions.Compiled);

    /// <summary>
    /// A regular expression for email address.
    /// </summary>
    public static readonly Regex Email = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled);

    /// <summary>
    /// A regular expression for password.
    /// </summary>
    public static readonly Regex Password = new(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?]).{10,}$", RegexOptions.Compiled);

    /// <summary>
    /// A constant for storing characters of the English alphabet with numbers.
    /// </summary>
    private const string EnglishWordsWithNumbers = "a-zA-Z0-9";
}