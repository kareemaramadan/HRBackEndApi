namespace HRBackEndApi.Middlewares
{
    public class RateLimitingMiddleware(RequestDelegate next,ILogger<RateLimitingMiddleware> logger)
    {
        private static int _counter = 0;
        private static DateTime _lastRequestDate = DateTime.Now;
        
        public async Task Invoke ( HttpContext context ) { 
            
            _counter++;
            if ( DateTime.Now.Subtract ( _lastRequestDate ).Seconds > 10 ) {

                _counter = 1;
                _lastRequestDate = DateTime.Now;
                await next ( context );
            }
            else
            {
                if ( _counter > 5 )
                {
                    _lastRequestDate = DateTime.Now;
                    await context.Response.WriteAsync ( "Rate Limit exceeded" );
                }
                else
                {
                    _lastRequestDate = DateTime.Now;
                    await next ( context );
                }
            }

        }
    }
}
