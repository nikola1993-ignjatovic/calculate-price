namespace CalculatePrice.Dtos
{
    public class ProductDto
    {
        public string Caption { get; set; }
        public double Price { get; set; }
        public double RaiseOrLower { get; set; }
        public string? Symbol {  get; set; }
    }
}
