using ESLimitAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace ESLimitAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string connectionESLimits = Configuration.GetConnectionString("ESLimits");

            services.AddDbContext<ESLimits>(options => 
                options.UseSqlServer(connectionESLimits));

            services.AddMvc().AddJsonOptions(options =>
                options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Limit API", Version = "v1" });
                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                //Set the comments path for the swagger json and ui.
                var xmlPath = Path.Combine(basePath, "ESLimitAPI.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddRequestLogger(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.InjectStylesheet("/swagger.css");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESLimitAPI");
            });

            app.UseExceptionHandler(options => {
                options.Run(async context => {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if (ex != null)
                    {
                        var err = JsonConvert.SerializeObject(new Error()
                        {
                            Message = ex.Error.Message,
                            Source = ex.Error.Source,
                            StackTrace = ex.Error.StackTrace
                        });

                        await context.Response.WriteAsync(err).ConfigureAwait(false);
                    }
                });
            });

            app.UseDefaultFiles(); // For index.html
            app.UseStaticFiles(); // For the wwwroot folder

            app.UseMvc();
        }
    }
}
