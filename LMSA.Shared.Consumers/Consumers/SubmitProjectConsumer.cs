using System.Threading.Tasks;
using LMSA.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LMSA.Shared.Consumer
{
    public class SubmitProjectConsumer : IConsumer<ISubmitProject>
    {
        ILogger<SubmitProjectConsumer> _logger;

        public SubmitProjectConsumer(ILogger<SubmitProjectConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISubmitProject> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.Title);
        }
    }
}