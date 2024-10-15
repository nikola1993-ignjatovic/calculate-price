namespace CalculatePrice.Dtos
{
    public class ExportRowBaseDto
    {
        //TODO description attribute
        public virtual string? Symbol { get; set; }
        public virtual string? OrderType { get; set; }
        public virtual double Low { get; set; }
        public virtual double High { get; set; }
        public virtual double BrokerRate { get; set; }
        public virtual string? Location { get; set; }
        public virtual double ClientPrice { get; set; }
        public virtual double Discount { get; set; }
        public virtual double ReferentPrice { get; set; }
        public virtual double DesiredClientPrice { get; set; } //change to minus
        public virtual double NewBrokerRate { get; set; }
        public virtual double DiscountOnRate { get; set; }
        public virtual double Check { get; set; }
        public virtual bool ShowFirstRow { get; set; }
    }
}
