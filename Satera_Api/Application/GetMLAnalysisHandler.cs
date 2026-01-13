
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using Satera_Api.Helper;
using Satera_Api.ML;

namespace Satera_Api.Application
{
    internal sealed class GetMLAnalysisHandler
        (
            IDataPreparationHandler dataPreparationHandler,
            IHostEnvironment env
        ) : IGetMLAnalysisHandler
    {

        public async Task<Result<Response>> Handle(GetMLAnalysisCommand command, CancellationToken cancellationToken)
        {
            MLContext mLContext = new();

            var mlPath = Path.Combine(env.ContentRootPath, "Static", "xgboost_final_fixed.onnx");

            var datawView = mLContext.Data.LoadFromEnumerable(new List<ModelInput>());
            
            var pipeline = mLContext.Transforms.ApplyOnnxModel(
                modelFile: mlPath,
                inputColumnNames: new[] { "float_input" },
                outputColumnNames: new[] { "label", "probabilities" });

            var model = pipeline.Fit(datawView);

            var engine = mLContext.Model.CreatePredictionEngine<ModelInput, Prediction>(model);

            DataPreparationResults results = await dataPreparationHandler.Handle(new GetMLAnalysisCommand
            (
                command.Gwa,
                command.TrackingDurationDays,
                command.TotalScreenTime,
                command.TotalAppsTracked,
                command.Pickups,
                command.DeviceUnlocks,
                command.Apps,
                command.CollectionTimestamp,
                command.Platform
            ), cancellationToken);

            var inputs = new ModelInput
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

            var prediction = engine.Predict(inputs);

            var reponse = new Response
            (
               prediction.Scores[0],
               prediction.Scores[1],
               prediction.Scores[2],
               prediction.Scores[3],
               prediction.Scores[4],
               prediction.Scores[5],
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
        private class Prediction
        {

            [ColumnName("label")]
            public long[] PredictedLabel { get; set; } = [];

            [ColumnName("probabilities")]
            public float[] Scores { get; set; } = [];
        }

        public sealed class ModelInput
        {
            [VectorType(11)]
            [ColumnName("float_input")]
            public required float[] Features { get; set; }
        }
    }
}
