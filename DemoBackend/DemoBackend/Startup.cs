using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using DemoBackend.Database.Interface;
using DemoBackend.Database.Repositories;
using DemoBackend.Services.Interfaces;
using DemoBackend.Services.Implementation;
using DemoBackend.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using DemoBackend.DatabaseConfig;

namespace DemoBackend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

            services.AddMvc();

            services.AddAutoMapper();

            services.AddDbContext<DemoBackendContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Defa‌​ultConnection")));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "My Demo API",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DemoBackendContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Demo API V1");
            });

            DbInitializer.Initialize(context);
        }
    }
}
