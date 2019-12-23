using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreGram.Helpers;
using Microsoft.AspNetCore.Http;

namespace CoreGram.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Manejador de excepciones
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode code;
            string message;

            if (exception is BadRequestException) code = HttpStatusCode.BadRequest;
            else if (exception is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (exception is NotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is NotAlloedException) code = HttpStatusCode.MethodNotAllowed;
            else if (exception is UnprocessableEntityException) code = HttpStatusCode.UnprocessableEntity;
            else code = HttpStatusCode.BadRequest;

            return ExceptionResponse(context, code, exception.Message);

        }

        /// <summary>
        /// Prepara el payload de respuesta
        /// </summary>
        /// <param name="context"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static Task ExceptionResponse(HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(new ResponseError()
            {
                Code = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
