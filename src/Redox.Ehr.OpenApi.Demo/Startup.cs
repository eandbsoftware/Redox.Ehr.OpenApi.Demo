using Flurl.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Timeout;
using Polly;
using Redox.DataModel.Builder.Configuration;
using Redox.Ehr.Contracts.Converters;
using Redox.Ehr.Contracts.Models.Dto;
using Redox.Ehr.Contracts.Models.Redox.Patientadmin.Newpatient;
using Redox.Ehr.OpenApi.Extensions;
using Redox.Ehr.OpenApi.Middleware;
using Redox.Ehr.OpenApi.Services.Flurl;
using Redox.Ehr.OpenApi.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Redox.Ehr.OpenApi
{
    public class Startup
    {
        private const string API_NAME = "Redox EHR OpenAPI Demo";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddHttpContextAccessor();

            // Add the whole configuration object here
            services.AddSingleton(Configuration);

            // Configure DI for application services
            RegisterServices(services);

            // Register RedoxApiConfig
            services.Configure<RedoxApiConfig>(Configuration.GetSection("RedoxApiConfig"));
            services.AddTransient(provider => provider.GetService<IOptions<RedoxApiConfig>>().Value);

            // Register custom HttpClientFactory with retry policy handler
            services.AddHttpClient<PollyHttpClientFactory>()
                .SetHandlerLifetime(TimeSpan.FromSeconds(30))
                .AddPolicyHandler(RetryPolicy());

            //services.AddControllers(x => x.Filters.Add(typeof(RedoxActionFilterHelper)))
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true; // To disable the automatic 400 behavior, set the SuppressModelStateInvalidFilter property to true
                    options.SuppressMapClientErrors = true;
                    options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.MaxDepth = 999999999;
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            // Configure Swagger support
            services.ConfigureSwagger(API_NAME);

            // Configure CORS
            services.AddCorsPolicy("EnableCORS");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Configure ServiceActivator
            ServiceActivator.Configure(app.ApplicationServices);

            // Get custom HttpClientFactory and configure Flurl to use it
            var factory = (PollyHttpClientFactory)app.ApplicationServices.GetService(typeof(PollyHttpClientFactory));
            FlurlHttp.Configure((settings) =>
            {
                settings.HttpClientFactory = factory;
                settings.OnErrorAsync = call =>
                {
                    logger.LogError($"Call failed: {call.Exception}");
                    return Task.CompletedTask;
                };
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", $"{API_NAME} v1.0");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Read more: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Global Exception handling middleware
            app.UseMiddleware<ExceptionHandling>();

            // Request/Response logging middleware
            app.UseMiddleware<ApiLogging>();

            app.UseMiddleware<RedoxDataModelDispatcher>();

            app.UseRouting();
            app.UseCors("EnableCORS");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IAsyncPolicy<HttpResponseMessage> RetryPolicy()
        {
            return Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(5)
                },
                (delegateResult, retryCount) =>
                {
                    Console.WriteLine($"[App|Policy]: Retry delegate fired, attempt {retryCount}");
                });
        }

        protected virtual void RegisterServices(IServiceCollection services)
        {
            // Register middlewares
            services.AddTransient<ApiLogging>();
            services.AddTransient<ExceptionHandling>();
            services.AddTransient<RedoxDataModelDispatcher>();

            // Register services
            services.AddSingleton<IAuthService, AuthService>();
            services.AddTransient<IPatientAdminService, PatientAdminService>();
            services.AddTransient<IPatientSearchService, PatientSearchService>();
            services.AddTransient<IRedoxClient, RedoxClient>();

            // Register converters
            services.AddTransient<IConverter<PatientDto, Newpatient>, PatientDtoToNewpatientCoverter>();
        }
    }
}
