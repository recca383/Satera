using Satera_Api.Application;

namespace Satera_Api.ML
{
    public interface IDataPreparationHandler
    {
        Task<DataPreparationResults> Handle(GetMLAnalysisCommand inputData, CancellationToken cancellation);
    }
}