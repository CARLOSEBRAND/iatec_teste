using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmprestimoBancario
{
    public class TratamentoDeErrosMiddleware
    {
        private readonly RequestDelegate _next;

        public TratamentoDeErrosMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    ValidationException => (int)HttpStatusCode.BadRequest,// custom application error
                    _ => (int)HttpStatusCode.InternalServerError,// unhandled error
                };
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }

        }
    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}
