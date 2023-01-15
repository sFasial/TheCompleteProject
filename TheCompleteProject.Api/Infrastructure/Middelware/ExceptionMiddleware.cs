using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheCompleteProject.Api.Infrastructure.Middelware.CustomExceptions;

namespace TheCompleteProject.Api.Infrastructure.Middelware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware : IMiddleware
    {
        //private readonly RequestDelegate _next;
        //private readonly ILogger<ExceptionMiddleware> _logger;


        //public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        //{
        //    _next = next;
        //    _logger = logger;
        //}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong : {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var exceptionData = GetExceptionDetails(exception);
            context.Response.StatusCode = (int)exceptionData.StatusCode;
            await context.Response.WriteAsync(exceptionData.ToString());
        }

        private ResponseModel GetExceptionDetails(Exception exception)
        {
            var model = new ResponseModel();
            var exceptionType = exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                model.StatusCode = (int)HttpStatusCode.Unauthorized;
                model.Message = exception.Message;
            }
            else if (exceptionType == typeof(BadResultException))
            {
                model.StatusCode = (int)HttpStatusCode.BadRequest;
                model.Message = exception.Message;
            }
            else if (exceptionType == typeof(RecordNotFoundException)
                || exceptionType == typeof(DuplicateRecordException))
            {
                model.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                //model.Message = ContentLoader.ReturnLanguageData(exception.Message, "");
                model.Message = exception.Message;
            }
            else if(exceptionType == typeof(System.DivideByZeroException))
            {
                model.StatusCode = (int)HttpStatusCode.BadRequest;
                model.Message = exception.Message;
            }
            else
            {
                model.StatusCode = (int)HttpStatusCode.InternalServerError;
                model.Message = exception.Message;
            }
            return model;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

