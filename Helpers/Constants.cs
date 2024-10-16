namespace CalculatePrice.Helpers
{
    public class Constants
    {
        public const double LowestAllowedRate = 0.005;
        public const byte NumberofUsedRows = 3;
        public const byte NumberOfColumnsBaseRate = 13;
        public const byte NumberOfColumnsAllRates = 11;
        public const byte MaxLenghtOfSheet = 31;
        public const string BrokerCode = "HAA";
        public const string SetBroker = "SetBroker";
        public const string Header = "AddBrokerTransactionTier";
        public const string AccountCode = "";
        public const byte WorksheetStyledRow = 1;
        public const byte WorksheetTestsuiteStyledRow = 5;
        public static class AppSettings
        {
            public const string InputTiersFilePath = "InputTiersFilePath";
            public const string InputProductFilePath = "InputProductFilePath";
            public const string OutputFilePath = "OutputFilePath";
        }
        public const string TSPrefix = "TS-";
        public const string TestSuiteComment = "!";
        public static class FolderNames
        {
            public const string TestSuiteCommands = "TestSuite Commands";
            public const string Products = "Products";
        }
    }
}
