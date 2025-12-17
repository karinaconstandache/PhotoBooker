using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PhotoBooker.API.Filters;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile))
            .ToList();

        if (!fileParameters.Any())
            return;

        var parametersToRemove = operation.Parameters
            .Where(p => p.Name == "file" || p.Name == "displayOrder")
            .ToList();
        
        foreach (var param in parametersToRemove)
        {
            operation.Parameters.Remove(param);
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["file"] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary",
                                Description = "The image file to upload (JPG or PNG)"
                            },
                            ["displayOrder"] = new OpenApiSchema
                            {
                                Type = "integer",
                                Format = "int32",
                                Default = new Microsoft.OpenApi.Any.OpenApiInteger(0),
                                Description = "Display order of the image in the portfolio"
                            }
                        },
                        Required = new HashSet<string> { "file" }
                    }
                }
            }
        };
    }
}
