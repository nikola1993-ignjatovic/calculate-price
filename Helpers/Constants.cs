namespace CalculatePrice.Helpers
{
    public class Constants
    {
        public const double LowestAllowedRate = 0.005;
        public const int NumberOfRowForFirstTier = 2;
        public const int MaxLenghtOfSheet = 31;
        public static class AppSettings
        {
            public const string InputTiersFilePath = "InputTiersFilePath";
            public const string InputProductFilePath = "InputProductFilePath";
            public const string OutputFilePath = "OutputFilePath";
        }
    }
}
