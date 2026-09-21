namespace HRBackEndApi.Middlewares
{
    public class LoggingMiddleware ( RequestDelegate next, ILogger<LoggingMiddleware> logger )
    {
        public async Task Invoke ( HttpContext context )
        {
            logger.LogInformation ("================================== Start New Log Information =========================================");
            logger.LogInformation ( "HTTP Request Information:Scheme:{Scheme}|Method:{Method}|Path:{Path}|Host:{Host}|QueryString:{QueryString}|Body:{Body}"
                , context.Request.Method, context.Request.Path, context.Request.Host, context.Request.QueryString, context.Request.Body,
                context.Request.Scheme );

            await next ( context );

            logger.LogInformation ( "Http Response Information:StatusCode:{StatusCode}", context.Response.StatusCode );
            logger.LogInformation ( "================================== End Log Information =========================================" );
        }
    }
}
