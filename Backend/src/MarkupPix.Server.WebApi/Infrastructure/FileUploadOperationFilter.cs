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

        if (!hasFileUploadAttribute)
        {
            return;
        }

        operation.Parameters.Clear();

        var isCreateDocument = context.MethodInfo.Name == "CreateDocument";
        var isUpdateDocument = context.MethodInfo.Name == "UpdateDocument";
        var isCreatePages = context.MethodInfo.Name == "CreatePages";
        var isUpdatePage = context.MethodInfo.Name == "UpdatePage";

        if (isCreateDocument)
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
                            Properties =
                            {
                                ["userEmailAddress"] = new OpenApiSchema { Type = "string" },
                                ["documentName"] = new OpenApiSchema { Type = "string" },
                                ["numberPages"] = new OpenApiSchema { Type = "integer" },
                                ["documentDescription"] = new OpenApiSchema { Type = "string" },
                                ["file"] = new OpenApiSchema { Type = "string", Format = "binary" },
                            },
                        },
                    },
                },
            };
        }
        else if (isUpdateDocument)
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
                            Properties =
                            {
                                ["userEmailAddress"] = new OpenApiSchema { Type = "string" },
                                ["documentName"] = new OpenApiSchema { Type = "string" },
                                ["numberPages"] = new OpenApiSchema { Type = "integer" },
                                ["documentDescription"] = new OpenApiSchema { Type = "string" },
                                ["file"] = new OpenApiSchema { Type = "string", Format = "binary" },
                            },
                        },
                    },
                },
            };
        }
        else if (isCreatePages)
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
                            Properties =
                            {
                                ["documentName"] = new OpenApiSchema { Type = "string" },
                                ["pages"] = new OpenApiSchema
                                {
                                    Type = "array",
                                    Items = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary",
                                    },
                                },
                            },
                        },
                    },
                },
            };
        }
        else if (isUpdatePage)
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
                            Properties =
                            {
                                ["userEmailAddress"] = new OpenApiSchema { Type = "string" },
                                ["documentName"] = new OpenApiSchema { Type = "string" },
                                ["numberPage"] = new OpenApiSchema { Type = "integer" },
                                ["page"] = new OpenApiSchema { Type = "string", Format = "binary" },
                            },
                        },
                    },
                },
            };
        }
    }
}