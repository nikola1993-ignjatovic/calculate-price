using CalculatePrice.Helpers;
namespace CalculatePrice.Dtos
{
    public class ExportRowFirstDto : ExportRowBaseDto
    {
        public override double DesiredClientPrice => ClientPrice + Discount;   //change to minus
        public override double NewBrokerRate { get { return Helper.CalculateNewBrokerRate(DesiredClientPrice, ReferentPrice);  } }  
        public override double DiscountOnRate { get { return (BrokerRate - NewBrokerRate) / BrokerRate;  } }
        public override double Check { get { return ReferentPrice * (1 + NewBrokerRate);  } } 
    }
}
