namespace CalculatePrice.Dtos
{
    public class ExportProductDto
    {
        public byte NumberOfLocations { get; set; }
         
        public required List<ExportRowBaseDto> ExportBaseTiersRows { get; set; }
        public required List<ExportRowBaseDto> ExportAllTiersRows { get; set; }
    }
}
