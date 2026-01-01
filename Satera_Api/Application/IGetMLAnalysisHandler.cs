using Microsoft.AspNetCore.Http.HttpResults;

namespace Satera_Api.Application
{
    public interface IGetMLAnalysisHandler
    {
        Task Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken);
    }
}
