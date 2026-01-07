using Microsoft.AspNetCore.Http.HttpResults;
using Satera_Api.Helper;

namespace Satera_Api.Application
{
    public interface IGetMLAnalysisHandler
    {
        Task<Result<Response>> Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken);
    }
}
