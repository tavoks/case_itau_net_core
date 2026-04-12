using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CaseItau.API.Middlewares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validaçãoÇ {Message}", ex.Message);
                await EscreverRespostaAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado: {Message}", ex.Message);
                await EscreverRespostaAsync(context, HttpStatusCode.InternalServerError, "Erro interno do servidor.");
            }
        }

        private static async Task EscreverRespostaAsync(HttpContext context, HttpStatusCode status, string mensagem)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { mensagem }));
        }
    }
}
