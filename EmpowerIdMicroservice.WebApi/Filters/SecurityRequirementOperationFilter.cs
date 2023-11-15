using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EmpowerIdMicroservice.WebApi.Filters
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly AuthorizationOptions _authorizationOptions;

        public SecurityRequirementsOperationFilter(IOptions<AuthorizationOptions> authorizationOptions)
        {
            // Beware: This might only part of the truth. If someone exchanges the IAuthorizationPolicyProvider and that loads
            // policies and requirements from another source than the configured options, we might not get all requirements
            // from here. But then we would have to make asynchronous calls from this synchronous interface.
            _authorizationOptions = authorizationOptions?.Value ?? throw new ArgumentNullException(nameof(authorizationOptions));
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var AllowAnonymousAttributeExist = context.MethodInfo.GetCustomAttributes(true).Concat(context.MethodInfo.DeclaringType.GetCustomAttributes(true)).OfType<AllowAnonymousAttribute>().ToList();

            if (!AllowAnonymousAttributeExist.Any())
            {
                //operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    }
                };
            }
        }
    }
}
