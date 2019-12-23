using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace CoreGram.Middlewares
{
    /// <summary>
    /// Clase para la creación del middleware reutilizable
    /// </summary>
    public static class ErrorHandlerExtensions
    {        
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
