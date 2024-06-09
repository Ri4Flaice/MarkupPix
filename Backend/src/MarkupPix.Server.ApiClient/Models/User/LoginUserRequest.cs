namespace MarkupPix.Server.ApiClient.Models.User;

/// <summary>
/// Login user request.
/// </summary>
public class LoginUserRequest
{
    /// <summary>
    /// User's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// User's password.
    /// </summary>
    public string? Password { get; set; }
}