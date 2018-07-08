﻿using System;
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
                    Description = $"{Description}\n{Copyright}",
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

            var logPath = Path.Combine(env.WebRootPath, "App_Data", "PostalCodesWebApi.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            File.AppendAllText(logPath, $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: LoadData: Begin\r\n");
            try
            {
                PostalCodesData.LoadData(env.WebRootPath);
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {ex}");
                throw;
            }
            File.AppendAllText(logPath, $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: LoadData: End\r\n");
        }
    }
}
