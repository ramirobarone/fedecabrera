using System.Data.SqlTypes;
using System.Text.Json;

namespace TodoList.Handler
{
    public class HandlerException(RequestDelegate _next, ILogger<HandlerException> logger)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandlerExceptionAsync(httpContext, e);
            }
        }
        private async Task HandlerExceptionAsync(HttpContext _context, Exception ex)
        {
            _context.Response.ContentType = "application/json";

            _context.Response.StatusCode = ex switch
            {
                OverflowException => StatusCodes.Status500InternalServerError,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                SqlTypeException => StatusCodes.Status507InsufficientStorage,
                Exception => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };

            logger.LogError(ex.Message);

            var response = new
            {
                error = "Error guardado en log"
            };

            await _context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}