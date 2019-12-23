using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreGram.Registers
{
    public static class SwaggerRegisters
    {
        public static IServiceCollection addSwaggerRegisters(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "CoreGram API",
                    Version = "v1",
                    Description = "Práctica del curso de ASP.NET Core",

                    Contact = new OpenApiContact
                    {
                        Name = "Alberto Reyes",
                        Email = "areyes@iti.es",
                        Url = new Uri("http://www.iti.es"),
                    }
                });
                
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Introduzca la palabra 'Bearer' seguido de un espacio en blanco y el token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                // Se añade seguridad JWT Bearer Token a Swagger
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer" }
                            }, new List<string>() }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            return services;
        }
    }
}
