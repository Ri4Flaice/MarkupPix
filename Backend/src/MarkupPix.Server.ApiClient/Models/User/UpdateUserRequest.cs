using System.ComponentModel.DataAnnotations;

using MarkupPix.Core.Enums;

namespace MarkupPix.Server.ApiClient.Models.User;

/// <summary>
/// Request to update a user.
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// User's email address.
    /// </summary>
    [Required]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Indicates that the user is blocked.
    /// </summary>
    public bool? Block { get; set; }

    /// <summary>
    /// The type of the user's account.
    /// </summary>
    public AccountType? AccountType { get; set; }
}