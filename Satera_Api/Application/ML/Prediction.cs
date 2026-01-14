using Microsoft.ML.Data;

namespace Satera_Api.Application.ML
{
    public sealed class Prediction
    {
        [ColumnName("label")]
        public long[] PredictedLabel { get; set; } = [];

        [ColumnName("probabilities")]
        public float[] Scores { get; set; } = [];
    }
}
