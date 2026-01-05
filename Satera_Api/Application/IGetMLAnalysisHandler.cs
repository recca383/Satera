using Microsoft.AspNetCore.Http.HttpResults;

namespace Satera_Api.Application
{
    public interface IGetMLAnalysisHandler
    {
        Task<Result<string>> Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken);
    }
}
