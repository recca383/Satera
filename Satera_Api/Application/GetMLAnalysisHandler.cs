
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using Satera_Api.Application.ML;
using Satera_Api.Helper;
using Satera_Api.ML;

namespace Satera_Api.Application
{
    internal sealed class GetMLAnalysisHandler
        (
            IDataPreparationHandler dataPreparationHandler,
            IMLEngine mlEngine
        ) : IGetMLAnalysisHandler
    {

        public async Task<Result<Response>> Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken)
        {
            DataPreparationResults results = await dataPreparationHandler.Handle(command, cancellationToken);

            var input = CreateModelInput(results);

            var engine = mlEngine.GetPredictionEngine();

            var prediction = engine.Predict(input);

            var softmaxScores = SoftMax(prediction.Scores);
            var highestScoreIndex = Array.IndexOf(softmaxScores, softmaxScores.Max());

            var GetCategoryUsageSecondsResult = GetTop5UsagePercentages(results.CategoryUsageSeconds);

            var reponse = new Response
            (
               softmaxScores.Max(),
               ClassLabels[highestScoreIndex],
               GetCategoryUsageSecondsResult,
               DateTime.UtcNow
            );

            return Result.Success(reponse);
        }

        private readonly string[] ClassLabels = new[]
        {
            "AcademicAtRisk",
            "AverageBalancedUser",
            "DigitalMultitasker",
            "DigitalSelfRegulated",
            "HighFunctioningAcademic",
            "MinimalDigitalengager",
        };


        private Dictionary<string, float> GetTop5UsagePercentages(Dictionary<string, int> appUsageSeconds)
        {
            var totalseconds = appUsageSeconds.Values.Sum();
            var result = new Dictionary<string, float>();

            foreach(KeyValuePair<string, int> keyValue in appUsageSeconds)
            {
                result.TryAdd(keyValue.Key, ((float)keyValue.Value / totalseconds) * 100);
            }

            return result.OrderByDescending(s => s.Value).Take(5).ToDictionary();
        }

        private ModelInput CreateModelInput(DataPreparationResults results)
        {
            return new ModelInput
            {
                Features = new float[]
                {
                    results.UsageSeconds,
                    results.GwaScaled,
                    results.AcademicUsageLog,
                    results.ProductivityUsageLog,
                    results.Social_usage_Log,
                    results.EntertainmentUsageLog,
                    results.FocusratioLog,
                    results.AcademicEffiencyLog,
                    results.UnlockIntensityLog,
                    results.SessionDepthLog,
                    results.PickupsLog
                }
            };
        }

        public float[] SoftMax(float[] scores)
        {
            var max = scores.Max();
            var exp = scores.Select(s => Math.Exp(s - max)).ToArray();
            var sumExp = exp.Sum();
            return exp.Select(e => (float)(e / sumExp)).ToArray();
        }
    }
}
