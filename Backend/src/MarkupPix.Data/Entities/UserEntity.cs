using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MarkupPix.Core.Enums;
using MarkupPix.Core.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Data.Entities;

/// <summary>
/// User table.
/// </summary>
[Table("users")]
[Comment("User data")]
public class UserEntity : IdentityUser<long>, IUser
{
    /// <inheritdoc />
    [MaxLength(256)]
    [Comment("The user's email address")]
    public string? EmailAddress { get; set; }

    /// <inheritdoc />
    [Comment("Indicates that the user is blocked")]
    public bool Block { get; set; }

    /// <inheritdoc />
    [Column(TypeName = "date")]
    [Comment("The user's birthdate")]
    public DateTime BirthDate { get; set; }

    /// <inheritdoc />
    [Comment("The type of the user's account")]
    public AccountType AccountType { get; set; }
}