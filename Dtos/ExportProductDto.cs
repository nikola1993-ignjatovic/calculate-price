namespace CalculatePrice.Dtos
{
    public class ExportProductDto
    {
        public byte NumberOfLocations { get; set; }
         
        public List<ExportRowBaseDto> ExportBaseTiersRows { get; set; }
        public List<ExportRowBaseDto> ExportAllTiersRows { get; set; }
        public List<ExportRowBaseDto> ExportTSBaseTiersRows { get; set; }
    }
}
