using Microsoft.ML;
using Satera_Api.ML;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Satera_Api.Application.ML
{
    public class MLEngine(IHostEnvironment env) : IMLEngine
    {
        public PredictionEngine<ModelInput, Prediction> GetPredictionEngine()
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

            return engine;
        }
    }
}
