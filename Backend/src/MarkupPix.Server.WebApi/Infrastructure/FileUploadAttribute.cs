namespace MarkupPix.Server.WebApi.Infrastructure;

/// <summary>
/// File upload attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class FileUploadAttribute : Attribute
{
}