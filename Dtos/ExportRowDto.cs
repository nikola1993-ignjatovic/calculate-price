namespace CalculatePrice.Dtos
{
    public class ExportRowDto: ExportRowBaseDto
    {
        public double OldDiscountOnRate { get; set; }
        public override double NewBrokerRate  => BrokerRate * (1 - OldDiscountOnRate);
        public override double Check => ReferentPrice * (1 + NewBrokerRate);
        public override double Discount => ClientPrice - Check;
    }
}
