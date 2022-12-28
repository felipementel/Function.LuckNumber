using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace Project.LuckyNumber.Function.Functions.Http
{
    public class Ping
    {
        private readonly ILogger _logger;

        private readonly PingResponse _pingResponse;

        public Ping(
            ILoggerFactory loggerFactory,
            PingResponse pingResponse)
        {
            _logger = loggerFactory.CreateLogger<Ping>();
            _pingResponse = pingResponse;
        }

        [Function("Ping")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            cancellationToken.ThrowIfCancellationRequested();

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.WriteAsJsonAsync(_pingResponse,
                cancellationToken);
            return response;
        }
    }

    public class PingResponse
    {
        public string _machine { get => Environment.MachineName; }

        public string _osVersion { get => Environment.OSVersion.VersionString; }

        public string _framework { get => RuntimeInformation.FrameworkDescription; }
    }
}
