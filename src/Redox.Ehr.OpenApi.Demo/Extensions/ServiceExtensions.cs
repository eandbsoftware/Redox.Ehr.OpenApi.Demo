using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Linq;
using System.Reflection;
using System;

namespace Redox.Ehr.OpenApi.Extensions
{
    public static class ServiceExtensions
    {
        // More info: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
        public static void AddCorsPolicy(this IServiceCollection serviceCollection, string corsPolicyName)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName,
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection serviceCollection, string apiName, bool includeXmlDocumentation = true)
        {
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = $"{apiName} v1.0",
                    Version = "v1.0",
                    Description = "DEFAULT WEB API",
                    Contact = new OpenApiContact
                    {
                        Name = "Matjaz Bravc",
                        Email = "matjaz.bravc@gmail.com",
                        Url = new Uri("https://matjazbravc.github.io/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licenced under MIT license",
                        Url = new Uri("http://opensource.org/licenses/mit-license.php")
                    }
                });
                if (includeXmlDocumentation)
                {
                    var xmlDocFile = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                    if (File.Exists(xmlDocFile))
                    {
                        options.IncludeXmlComments(xmlDocFile);
                    }
                }
                options.DescribeAllParametersInCamelCase();
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }
    }
}