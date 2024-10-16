using CalculatePrice.Dtos;
using CalculatePrice.Enums;

namespace CalculatePrice.Helpers
{
    public static class Helper
    {
        public static double CalculateNewBrokerRate(double desiredClientPrice, double referentPrice)
        {
            return desiredClientPrice / referentPrice - 1 < Constants.LowestAllowedRate
                                                                    ? Constants.LowestAllowedRate
                                                                    : desiredClientPrice / referentPrice - 1;
        }
        public static string FixSheetName(string sheetName) => sheetName.Length > Constants.MaxLenghtOfSheet ?
                                                                                                sheetName.Substring(0, Constants.MaxLenghtOfSheet - 3) + "..."
                                                                                                : sheetName;
        public static double GetReferentPrice(ProductDto productDto, ProductTierRateDto productTierRateDto) => productDto.Price / (1 + (double)(productTierRateDto?.Rate));
        public static string GetMetalTypeFromSymbol(string symbol)
        {
            var metalType = MetalType.Unknown;
            if (!string.IsNullOrWhiteSpace(symbol) && symbol.Length >= 2)
            {
                switch (symbol.Substring(1, 1))
                {
                    case "S":
                        metalType = MetalType.Silver;
                        break;
                    case "G":
                        metalType = MetalType.Gold;
                        break;
                    //TODO add other metal types, and get attribute instead of S and G
                    default:
                        break;
                }
            }
            return metalType.ToString();
        }
        public static string AddTestSuitePrefix(string productCaption) => string.Format($"{Constants.TSPrefix}{productCaption}");
        public static string CommentTestSuiteCommand(string command) => $"{Constants.TestSuiteComment}{command}";
    }
}
