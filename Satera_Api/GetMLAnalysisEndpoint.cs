using Satera_Api.Application;

namespace Satera_Api
{
    public class GetMLAnalysisEndpoint()
    {
        private sealed record Request();
        public Task Endpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("getMl/", async (
                Request request,
                IGetMLAnalysisHandler handler,
                CancellationToken cancellationToken) =>
            {
                var command = new GetMLAnalysisCommand();

                
            });
        }
    }
}
