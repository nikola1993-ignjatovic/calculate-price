using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatePrice.Dtos
{
    public class ProductTierRateDto
    {
        public string? Broker { get; set; }
        public string? Symbol { get; set; }
        public string? LongCaption { get; set; }
        public string? OrderType { get; set; } //TODO use enum
        public double Low { get; set; }
        public double High { get; set; }
        public double Rate { get; set; }
        public string? Location { get; set; }
    }
}
