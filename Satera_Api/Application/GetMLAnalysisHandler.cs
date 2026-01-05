
namespace Satera_Api.Application
{
    internal sealed class GetMLAnalysisHandler : IGetMLAnalysisHandler
    {
        public async Task<Result<string>> Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken)
        {
            string result = "Hello " + command.name;

            return Result.Success(result);
        }
    }
}
