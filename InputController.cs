using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApiRequestRouter
{
    [ApiController]
    [Route("[controller]")]
    public class InputProcessingController : ControllerBase
    {
        private IConfiguration _configuration;
        private static List<dynamic> batchForServiceA = new List<dynamic>();
        private static List<dynamic> batchForServiceB = new List<dynamic>();
        private static DateTime lastBatchSentTime = DateTime.Now;

        public InputProcessingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessInput([FromBody] dynamic input)
        {
            const int batchTimeIntervalInSeconds = 10; // For example, we send a batch every 10 seconds
            string inputType = input.type;
            bool isBatchTimeElapsed = (DateTime.Now - lastBatchSentTime).TotalSeconds > batchTimeIntervalInSeconds;

            switch (inputType)
            {
                case "ServiceA":
                    batchForServiceA.Add(input);
                    if (isBatchTimeElapsed)
                    {
                        await SendBatchToServiceA();
                        lastBatchSentTime = DateTime.Now;
                    }
                    break;
                case "ServiceB":
                    batchForServiceB.Add(input);
                    if (isBatchTimeElapsed)
                    {
                        await SendBatchToServiceB();
                        lastBatchSentTime = DateTime.Now;
                    }
                    break;
                default:
                    return BadRequest("Invalid input type.");
            }

            return Accepted("Input received and will be processed in the next batch.");
        }

        private async Task SendBatchToServiceA()
        {
            string serviceAEndpoint = _configuration.GetValue<string>("ServiceAEndpoint");
            batchForServiceA.Clear();
        }

        private async Task SendBatchToServiceB()
        {
            string serviceBEndpoint = _configuration.GetValue<string>("ServiceBEndpoint");
            batchForServiceB.Clear();
        }
    }
}