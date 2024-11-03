using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; 

namespace ApiRequestRouter
{
    [ApiController]
    [Route("[controller]")]
    public class InputProcessingController : ControllerBase
    {
        private IConfiguration _configuration;
        private ILogger<InputProcessingController> _logger; 
        private static List<dynamic> batchForServiceA = new List<dynamic>();
        private static List<dynamic> batchForServiceB = new List<dynamic>();
        private static DateTime lastBatchSentTime = DateTime.Now;

        public InputProcessingController(IConfiguration configuration, ILogger<InputProcessingController> logger)
        {
            _configuration = configuration;
            _logger = logger; 
        }

        [HttpPost]
        public async Task<IActionResult> ProcessInput([FromBody] dynamic input)
        {
            try
            {
                const int batchTimeIntervalInSeconds = 10; 
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing input.");
                return StatusCode(500, "An error occurred while processing your request.");
            }

            return Accepted("Input received and will be processed in the next batch.");
        }

        private async Task SendBatchToServiceA()
        {
            try
            {
                string serviceAEndpoint = _configuration.GetValue<string>("ServiceAEndpoint");
                batchForServiceA.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send batch to Service A");
            }
        }

        private async Task SendBatchToServiceB()
        {
            try
            {
                string serviceBEndpoint = _configuration.GetValue<string>("ServiceBEndpoint");
                batchForServiceB.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send batch to Service B");
            }
        }
    }
}