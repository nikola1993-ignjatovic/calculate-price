using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Enums
{
    public enum LocationType
    {
        Sydney, 
        SaltLakeCity,
        NewYork, 
        Singapore
    }
    public static class LocationTypeExtensions
    {
        public static string ToLocationString(this LocationType location)
        {
            switch (location)
            {
                case LocationType.SaltLakeCity:
                    return "Salt Lake City";
                case LocationType.NewYork:
                    return "New York";
                default:
                    return location.ToString();
            }
        }
    }
}
