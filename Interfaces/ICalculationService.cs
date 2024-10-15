using CalculatePrice.Dtos;

namespace CalculatePrice.Interfaces
{
    public interface ICalculationService
    {
        void BeginCalculation();
        Dictionary<string, ExportProductDto> PerformCalculation();
    }
}
