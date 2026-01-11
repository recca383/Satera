
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

            return Result.Success(reponse);
        }

        private class Prediction
        {

            [ColumnName("label")]
            public long[] PredictedLabel { get; set; }

            [ColumnName("probabilities")]
            public float[] Scores { get; set; }
        }

        public sealed class ModelInput
        {
            [VectorType(11)]
            [ColumnName("float_input")]
            public float[] Features { get; set; }
        }
    }
}
