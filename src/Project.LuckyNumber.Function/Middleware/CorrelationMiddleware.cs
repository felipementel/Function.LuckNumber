//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.LuckyNumber.Function.Middleware
{
    internal class CorrelationMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var requestData = await context.GetHttpRequestDataAsync();

            string correlationId;
            if (requestData!.Headers.TryGetValues("x-correlationId", out var values))
            {
                correlationId = values.First();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }

            await next(context);

            context.GetHttpResponseData()?.Headers.Add("x-correlationId", correlationId);            
        }
    }
}