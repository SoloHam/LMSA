using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<IActionResult> Publish(string message)
        {
            await _publishEndpoint.Publish<string>(new
            {
                Value = message
            });

            return Ok();
        }
    }
}