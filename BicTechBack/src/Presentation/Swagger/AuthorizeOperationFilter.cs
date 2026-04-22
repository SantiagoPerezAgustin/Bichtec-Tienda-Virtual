using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BicTechBack.src.Presentation.Swagger;

/// <summary>
/// Solo marca con seguridad Bearer los endpoints que tienen [Authorize] (no [AllowAnonymous]).
/// </summary>
public sealed class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.MethodInfo;
        var type = method.DeclaringType;
        if (method.GetCustomAttribute<AllowAnonymousAttribute>(true) != null
            || type?.GetCustomAttribute<AllowAnonymousAttribute>(true) != null)
            return;

        var hasAuthorize = method.GetCustomAttribute<AuthorizeAttribute>(true) != null
                           || type?.GetCustomAttribute<AuthorizeAttribute>(true) != null;
        if (!hasAuthorize)
            return;

        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
