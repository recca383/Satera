
using Satera_Api.Helper;

namespace Satera_Api.Application
{
    internal sealed class GetMLAnalysisHandler : IGetMLAnalysisHandler
    {
        public async Task<Result<Response>> Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken)
        {

            Response response = new Response
            (
                0.8f,
                0.6f,
                0.7f,
                0.9f,
                0.85f,
                0.4f,
                DateTime.UtcNow
            );

            return Result.Success(response);
        }
    }
}
