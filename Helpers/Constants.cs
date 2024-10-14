using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Helpers
{
    public class Constants
    {
        public const double LowestAllowedRate = 0.005;
        public const int NumberOfRowForFirstTier = 2;
        public static class AppSettings
        {
            public const string InputTiersFilePath = "InputTiersFilePath";
            public const string InputProductFilePath = "InputProductFilePath";
            public const string OutputFilePath = "OutputFilePath";
        }
    }
}
