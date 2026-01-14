using Microsoft.ML.Data;

namespace Satera_Api.Application.ML
{
    public sealed class ModelInput
    {
        [VectorType(11)]
        [ColumnName("float_input")]
        public required float[] Features { get; set; }
    }
}
