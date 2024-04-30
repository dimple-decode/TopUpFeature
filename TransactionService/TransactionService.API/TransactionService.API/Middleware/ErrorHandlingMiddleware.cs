namespace TransactionService.API.Middleware
{
    /// <summary>
    /// Error Handling Middleware
    /// </summary>
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                _logger.LogError(message);
            }
        }
    }
}
