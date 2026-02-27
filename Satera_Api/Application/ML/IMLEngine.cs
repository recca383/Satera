using Microsoft.ML;

namespace Satera_Api.Application.ML
{
    interface IMLEngine
    {
        PredictionEngine<ModelInput, Prediction> GetPredictionEngine();
    }
}
