using System;
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
        public InputProcessingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessInput([FromBody] dynamic input)
        {
            string inputType = input.type;

            switch (inputType)
            {
                case "ServiceA":
                    return await HandleServiceA(input);
                case "ServiceB":
                    return await HandleServiceB(input);
                default:
                    return BadRequest("Invalid input type.");
            }
        }

        private async Task<IActionResult> HandleServiceA(dynamic input)
        {
            string serviceAEndpoint = _configuration.GetValue<string>("ServiceAEndpoint");
            return Ok($"Processed by ServiceA with endpoint {serviceAEndpoint}");
        }

        private async Task<IActionResult> HandleServiceB(dynamic input)
        {
            string serviceBEndpoint = _configuration.GetValue<string>("ServiceBEndpoint");
            return Ok($"Processed by ServiceB with endpoint {serviceBEndpoint}");
        }
    }
}