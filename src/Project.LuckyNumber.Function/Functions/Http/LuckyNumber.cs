using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Project.LuckyNumber.Function.Functions.Http
{
    public class LuckyNumber
    {
        private readonly ILogger _logger;

        public LuckyNumber(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LuckyNumber>();
        }

        [Function("LuckyNumber")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = "luckynumber/max/{max:int}/min/{min:int}")] HttpRequestData req,
            CancellationToken cancellationToken,
            int max,
            int min)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            int NumberMax = max;
            int NumberMin = min;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string validations = TestNumbers(NumberMax, NumberMin);
                if (validations != string.Empty)
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                    //return new BadRequestObjectResult(validations);
                }

                if (HttpMethods.IsGet(req.Method))
                {
                    int generatedLuckyNumber = 0;
                    generatedLuckyNumber = GenerateLuckyNumber(NumberMin, NumberMax);

                    //return new OkObjectResult(generatedLuckyNumber);

                    var response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(generatedLuckyNumber, cancellationToken);
                    return response;
                }
                else
                {
                    var response = req.CreateResponse(HttpStatusCode.BadRequest);
                    await response.WriteAsJsonAsync($"Http method {req.Method} not allowed.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync($"Error: {ex.Message}.");
                return response;
                //return new BadRequestObjectResult($"Error: {ex.Message}.");
            }
        }

        public static string TestNumbers(int max, int min)
        {
            if (min > max)
            {
                return $"Number min {min} is greater than number max {max}";
            }
            else if (min == max)
            {
                return $"Number min {min} is equal than number max {max}";
            }
            else
            {
                return string.Empty;
            }
        }

        public static int GenerateLuckyNumber(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
        }
    }
}
