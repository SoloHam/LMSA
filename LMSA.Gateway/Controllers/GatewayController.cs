using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace LMSA.Gateway
{
    [Route("/")]
    public class Gateway : Controller
    {
        readonly IBus _bus;

        public Gateway(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> Welcome() => Content("Hello there!!");

        [HttpPost]
        public async Task<IActionResult> Publish(string message)
        {
            await _bus.Publish(new ValueEntered(message));

            return Ok();
        }

        public class ValueEntered{
      public ValueEntered(string value)
      {
        Value = value;
      }

      public string Value { get; set; }
        }
    }
}