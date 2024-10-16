using Helper = CalculatePrice.Helpers.Helper; 

namespace CalculatePrice.Dtos
{
    public class ExportTestSuiteRow: ExportRowBaseDto
    {
        public string MetalType => Helper.GetMetalTypeFromSymbol(Symbol);
    }
}
