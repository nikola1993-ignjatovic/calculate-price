namespace CalculatePrice.Helpers
{
    public class Constants
    {
        public const double LowestAllowedRate = 0.005;
        public const byte NumberofUsedRows = 3;
        public const byte NumberOfColumnsBaseRate = 13;
        public const byte NumberOfColumnsAllRates = 11;
        public const byte MaxLenghtOfSheet = 31;
        public static class AppSettings
        {
            public const string InputTiersFilePath = "InputTiersFilePath";
            public const string InputProductFilePath = "InputProductFilePath";
            public const string OutputFilePath = "OutputFilePath";
        }
    }
}
