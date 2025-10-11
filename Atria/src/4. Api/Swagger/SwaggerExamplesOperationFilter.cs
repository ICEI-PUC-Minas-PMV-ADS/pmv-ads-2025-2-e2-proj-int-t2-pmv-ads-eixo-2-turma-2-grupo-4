using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Atria.Api.Swagger;

public class SwaggerExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null) return;

        // PUT /api/users/{id} - example request body
        if (operation.RequestBody != null && context.MethodInfo.DeclaringType != null)
        {
            var route = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? string.Empty;

            if (route.StartsWith("api/users/") && context.ApiDescription.HttpMethod == "PUT")
            {
                var example = new OpenApiObject
                {
                    ["nome"] = new OpenApiString("Maria Souza"),
                    ["matricula"] = new OpenApiString("P12345"),
                    ["areaAtuacao"] = new OpenApiString("Física")
                };

                foreach (var content in operation.RequestBody.Content)
                {
                    content.Value.Example = example;
                }
            }
        }

        // GET /api/users/me - example response
        if (operation.Responses != null)
        {
            var route = context.ApiDescription.RelativePath?.ToLowerInvariant() ?? string.Empty;
            if (route == "api/users/me" && operation.Responses.TryGetValue("200", out var resp))
            {
                var example = new OpenApiObject
                {
                    ["idUsuario"] = new OpenApiString("a1b2c3d4-e5f6-..."),
                    ["nome"] = new OpenApiString("Maria Souza"),
                    ["email"] = new OpenApiString("maria.souza@example.com"),
                    ["tipoUsuario"] = new OpenApiString("Professor"),
                    ["dataCadastro"] = new OpenApiString("2025-10-10T12:34:56Z"),
                    ["matricula"] = new OpenApiString("P12345"),
                    ["areaAtuacao"] = new OpenApiString("Física")
                };

                foreach (var content in resp.Content)
                {
                    content.Value.Example = example;
                }
            }
        }
    }
}