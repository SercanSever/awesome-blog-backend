using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Blog.CORE.Extensions
{
   public class ExceptionMiddleware
   {
      private readonly ILogger<ExceptionMiddleware> _logger;
      private RequestDelegate _next;

      public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
      {
         _next = next;
         _logger = logger;
      }

      public async Task InvokeAsync(HttpContext httpContext)
      {
         try
         {
            await _next(httpContext);
         }
         catch (Exception e)
         {
            _logger.LogError(e.Message);
            await HandleExceptionAsync(httpContext, e);
         }
      }

      private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
      {
         httpContext.Response.ContentType = "application/json";
         httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

         string message = "Internal Server Error";
         IEnumerable<ValidationFailure> errors;
         if (exception.GetType() == typeof(ValidationException))
         {
            message = exception.Message;
            errors = ((ValidationException)exception).Errors;
            httpContext.Response.StatusCode = 400;

            return httpContext.Response.WriteAsync(new ValidationErrorDetails
            {
               StatusCode = httpContext.Response.StatusCode,
               Message = "Validation Error",
               Errors = errors
            }.ToString());
         }

         return httpContext.Response.WriteAsync(new ErrorDetails
         {
            StatusCode = httpContext.Response.StatusCode,
            Message = message
         }.ToString());
      }
   }
}