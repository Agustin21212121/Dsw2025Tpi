using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Dsw2025Tpi.Application.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var errorMessage = ex.InnerException?.Message ?? ex.Message;


                context.Response.StatusCode = ex switch
                {
                    EntityNotFoundException => StatusCodes.Status404NotFound,
                    DuplicatedEntityException => StatusCodes.Status400BadRequest,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    InvalidOperationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    _logger.LogError(ex, "Error no controlado");
                    if (ex.InnerException != null)
                        _logger.LogError(ex.InnerException, "Inner exception");
                }

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    error = errorMessage
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
