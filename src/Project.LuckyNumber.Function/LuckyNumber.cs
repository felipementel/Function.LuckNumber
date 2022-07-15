using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Project.LuckyNumber.Function
{
    public static class LuckyNumber
    {
        [FunctionName("LuckyNumber")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = "luckynumber/max/{max:int}/min/{min:int}")] HttpRequest req,
            int max,
            int min,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int NumberMax = max;
            int NumberMin = min;

            try
            {
                string validations = TestNumbers(NumberMax, NumberMin);
                if (validations != String.Empty)
                    return new BadRequestObjectResult(validations);

                if (HttpMethods.IsGet(req.Method))
                {
                    int generatedLuckyNumber = 0;
                    generatedLuckyNumber = GenerateLuckyNumber(NumberMin, NumberMax);

                    return new OkObjectResult(generatedLuckyNumber);
                }
                else
                {
                    return new BadRequestObjectResult($"Http method {req.Method} not allowed.");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error: {ex.Message}.");
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
                return String.Empty;
            }
        }

        public static int GenerateLuckyNumber(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
        }
    }
}
