using System.Diagnostics;

namespace HRBackEndApi.Middlewares
{
    public class ProfilingMiddleware ( RequestDelegate next, ILogger<ProfilingMiddleware> logger )
    {
    public async Task InvokeAsync ( HttpContext context )
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start ();
            await next ( context );
            stopWatch.Stop ();
            logger.LogInformation ( $"Request `{context.Request.Path}` took `{stopWatch.ElapsedMilliseconds}ms` to execute" );
        }

    }

}
