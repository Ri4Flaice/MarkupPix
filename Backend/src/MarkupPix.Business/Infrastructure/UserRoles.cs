using MarkupPix.Core.Enums;

namespace MarkupPix.Business.Infrastructure;

/// <summary>
/// User roles.
/// </summary>
public static class UserRoles
{
    /// <summary>
    /// Default (not used).
    /// </summary>
    public const string Default = nameof(Default);

    /// <summary>
    /// Super admin.
    /// </summary>
    public const string SuperAdmin = nameof(SuperAdmin);

    /// <summary>
    /// Admin.
    /// </summary>
    public const string Admin = nameof(Admin);

    /// <summary>
    /// File manager.
    /// </summary>
    public const string FileManager = nameof(FileManager);

    /// <summary>
    /// The markup.
    /// </summary>
    public const string Markup = nameof(Markup);

    /// <summary>
    /// Access to the FileManager role.
    /// </summary>
    public const string AtFileManager = $"{SuperAdmin}, {Admin}, {FileManager}";

    /// <summary>
    /// Enumeration of roles.
    /// </summary>
    public static readonly Dictionary<AccountType, string> RolesList =
        Enum.GetValues<AccountType>()
            .ToDictionary(t => t, t => t.ToString());
}