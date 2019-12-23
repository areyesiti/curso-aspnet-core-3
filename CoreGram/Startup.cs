using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreGram.Data;
using CoreGram.Helpers;
using CoreGram.Middlewares;
using CoreGram.Registers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreGram
{
    public class Startup
    {
        private IConfiguration _configuration;
        private readonly string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers();

            // Configuraciones para Automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Registro del DbContext
            services.AddDbContext<DataContext>(op => op.UseSqlServer(_configuration.GetConnectionString("DatabaseConnection")));            

            // Otras configuraciones
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {                    
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                });

            // Otra forma de obtener AppSettings
            //var appSettingsSection = _configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(appSettingsSection);
            //var appSettings = appSettingsSection.Get<AppSettings>();

            // Registro de repositorios en Startup
            //services.AddTransient(typeof(UserRepository));
            //services.AddTransient<UserRepository>();

            // Registro del servicio de autenticación
            services.addAuthenticationRegisters(_configuration);

            // Registro de los servicios propios
            services.addCustomRegisters();

            // Registro y configuración de swagger
            services.addSwaggerRegisters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseErrorHandlerMiddleware();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endoints =>
           {
               endoints.MapControllers();
           });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });


            // Ejemplo middleware en linea
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Hola desde el primer middleware");
            //    await next.Invoke();
            //    Console.WriteLine("Adiós desde el primer middleware");
            //});

            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Hola desde el segundo middleware");
            //    await next.Invoke();
            //    Console.WriteLine("Adiós desde el segundo middleware");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseMvc();
        }
    }
}
