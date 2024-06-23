using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarkupPix.Server.WebApi.Infrastructure;

/// <summary>
/// File upload operation filter.
/// </summary>
public class FileUploadOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the specified operation filter to support file uploads in Swagger UI.
    /// </summary>
    /// <param name="operation">The operation being processed, which represents an API endpoint.</param>
    /// <param name="context">The context in which the operation is being applied, providing metadata and information about the API.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasFileUploadAttribute = context.MethodInfo.GetCustomAttributes(true).OfType<FileUploadAttribute>().Any();

        if (!hasFileUploadAttribute) return;

        operation.Parameters.Clear();

        var isCreateDocument = context.MethodInfo.Name == "CreateDocument";
        var isUpdateDocument = context.MethodInfo.Name == "UpdateDocument";
        var isCreatePages = context.MethodInfo.Name == "CreatePages";
        var isUpdatePage = context.MethodInfo.Name == "UpdatePage";

        var properties = new Dictionary<string, OpenApiSchema>();

        if (isCreateDocument || isUpdateDocument)
        {
            properties.Add("documentName", new OpenApiSchema { Type = "string" });
            properties.Add("numberPages", new OpenApiSchema { Type = "integer" });
            properties.Add("documentDescription", new OpenApiSchema { Type = "string" });
            properties.Add("file", new OpenApiSchema { Type = "string", Format = "binary" });
        }
        else if (isCreatePages)
        {
            properties.Add("documentName", new OpenApiSchema { Type = "string" });
            properties.Add("pages", new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary",
                },
            });
        }
        else if (isUpdatePage)
        {
            properties.Add("documentName", new OpenApiSchema { Type = "string" });
            properties.Add("numberPage", new OpenApiSchema { Type = "integer" });
            properties.Add("page", new OpenApiSchema { Type = "string", Format = "binary" });
        }

        CreateStructureAndFormatData(operation, properties);
    }

    /// <summary>
    /// Creates a request description for the Swagger (OpenAPI) specification.
    /// </summary>
    /// <param name="operation">Operation.</param>
    /// <param name="properties">Properties.</param>
    private static void CreateStructureAndFormatData(OpenApiOperation operation, IDictionary<string, OpenApiSchema> properties)
    {
        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new()
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = properties,
                    },
                },
            },
        };
    }
}