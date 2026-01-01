
namespace Satera_Api.Application
{
    internal sealed class GetMLAnalysisHandler : IGetMLAnalysisHandler
    {
        public Task Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello");

            return Task.CompletedTask;
        }
    }
}
