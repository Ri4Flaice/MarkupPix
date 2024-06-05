using System.ComponentModel;

namespace MarkupPix.Core.Enums;

/// <summary>
/// User's account type.
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Default (not used).
    /// </summary>
    [Description("Default (not used)")]
    Default = 0,

    /// <summary>
    /// Super admin.
    /// </summary>
    [Description("Super admin")]
    SuperAdmin = 1,

    /// <summary>
    /// Admin.
    /// </summary>
    [Description("Admin")]
    Admin = 2,

    /// <summary>
    /// The markup.
    /// </summary>
    [Description("The markup")]
    Markup = 4,

    /// <summary>
    /// File manager.
    /// </summary>
    [Description("File manager")]
    FileManager = 8,
}