using MarkupPix.Core.Enums;

namespace MarkupPix.Core.User;

/// <summary>
/// User data.
/// </summary>
public interface IUser
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    string? EmailAddress { get; set; }

    /// <summary>
    /// Indicates that the user is blocked.
    /// </summary>
    bool Block { get; set; }

    /// <summary>
    /// The user's birthdate.
    /// </summary>
    DateTime BirthDate { get; set; }

    /// <summary>
    /// The type of the user's account.
    /// </summary>
    AccountType AccountType { get; set; }
}