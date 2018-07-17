using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostalCodesWebApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace PostalCodesWebApi
{
    public class Startup
    {
        public static string AssemblyName { get; } = typeof(Startup).Assembly.GetName().Name;
        public static string Title { get; } = typeof(Startup).Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public static string InformationalVersion { get; } = typeof(Startup).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public static string Description { get; } = typeof(Startup).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        public static string Copyright { get; } = typeof(Startup).Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

        public const string ProjectUri = "https://github.com/kcg-edu-future-lab/Postal-Codes-JP";
        public const string LicenseUri = "https://github.com/kcg-edu-future-lab/Postal-Codes-JP/blob/master/LICENSE";

        public static string DataZipUri { get; private set; }

        public static string WebRootPath { get; private set; }
        public static Lazy<string> AppDataPath { get; } = new Lazy<string>(() => Path.Combine(WebRootPath, "App_Data"));
        public static Lazy<string> LogFilePath { get; } = new Lazy<string>(() => Path.Combine(AppDataPath.Value, $"{nameof(PostalCodesWebApi)}.log"));

        public static void WriteLog(string message)
        {
            File.AppendAllText(LogFilePath.Value, $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {message}\r\n");
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            // Register the Swagger generator, defining 1 or more Swagger documents.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = Title,
                    Version = $"v{InformationalVersion}",
                    Description = $"{Description}\nWeb API のご利用については下記のプロジェクト サイトをご覧ください。\n{Copyright}",
                    Contact = new Contact
                    {
                        Name = Title,
                        Url = ProjectUri,
                    },
                    License = new License
                    {
                        Name = "MIT License",
                        Url = LicenseUri,
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{AssemblyName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(b => b.AllowAnyOrigin());
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Title} v1");
                c.DocumentTitle = Title;
                c.RoutePrefix = "";
            });

            DataZipUri = Configuration["App:DataZipUri"];
            WebRootPath = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
            Directory.CreateDirectory(AppDataPath.Value);

            WriteLog("LoadData: Begin");
            try
            {
                PostalCodesData.LoadData();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                throw;
            }
            WriteLog("LoadData: End");
        }
    }
}
