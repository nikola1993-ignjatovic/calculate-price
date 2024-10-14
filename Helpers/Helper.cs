using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
