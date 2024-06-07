using System.ComponentModel.DataAnnotations;

using MarkupPix.Core.Enums;
using MarkupPix.Core.User;

namespace MarkupPix.Server.ApiClient.Models.User;

/// <summary>
/// Response to get a user.
/// </summary>
public class GetUserResponse : IUser
{
    /// <summary>
    /// User ID.
    /// </summary>
    public long UserId { get; set; }

    /// <inheritdoc />
    [Required]
    [MaxLength(256)]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    [Required]
    public string? Password { get; set; }

    /// <inheritdoc />
    [Required]
    public bool Block { get; set; }

    /// <inheritdoc />
    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    /// <inheritdoc />
    [Required]
    public AccountType AccountType { get; set; }
}