using CoreGram.Helpers;
using CoreGram.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace CoreGram.Registers
{
    public static class AuthenticationRegisters
    {
        public static IServiceCollection addAuthenticationRegisters(this IServiceCollection services, IConfiguration Configuration)
        {
            // Configurar Settings Section
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Configuración jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            // Registramos el servicio de Autenticación
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Verificamos que el usuario sigue existiendo en BD
                        var userService = context.HttpContext.RequestServices.GetRequiredService<UserRepository>();
                        string userName = context.Principal.Identity.Name;
                        var user = userService.GetByName(userName);
                        if (user == null)
                        {
                            context.Fail(new UnauthorizedException("Usuario no autorizado"));
                        }
                        return Task.CompletedTask;
                    }
                };

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }
    }
}
