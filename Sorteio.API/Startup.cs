using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;

namespace Sorteio.API
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = (IConfigurationRoot)configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // CORS
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            //serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                              new Info
                              {
                                  Title = "Sorteio.Api",
                                  Version = "v1"
                              });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });

            app.UseCors("AllowSpecificOrigin");


        }
    }
}
