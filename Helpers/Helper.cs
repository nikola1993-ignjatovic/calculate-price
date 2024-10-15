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
                                                                                                :sheetName;
    }
}
