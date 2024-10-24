using CalculatePrice.Enums;

namespace CalculatePrice.Dtos
{
    public class ProductTierRateDto
    {
        public string? Broker { get; set; }
        public string? Symbol { get; set; }
        public string? LongCaption { get; set; }
        public OrderType? OrderType { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Rate { get; set; }
        public string? Location { get; set; }
    }
}
