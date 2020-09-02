using System.Threading.Tasks;
using LMSA.Shared.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace LMSA.Gateway
{
    [Route("/")]
    public class Gateway : Controller
    {
        readonly IPublishEndpoint _publishEndpoint;

        public Gateway(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Welcome() => Content("Hello there!!");

        [HttpPost("{message}")]
        public async Task<IActionResult> Publish(string message)
        {
            await _publishEndpoint.Publish<ProjectSubmissionAccepted>(new
            {
                Id = 1,
                Title = message,
                Description = message
            });

            return Ok("Yepeee");
        }

        public class ValueEntered
        {
            public ValueEntered(string value)
            {
                Value = value;
            }

            public string Value { get; set; }
        }
    }
}