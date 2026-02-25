namespace WebApplication1.DTOs
{
    public class GiftDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceCard { get; set; }
        public string GiftImage { get; set; }
        public string Category { get; set; }
        
        public int DonorId { get; set; }
        public string DonorName { get; set; }

        public int? CustomerId{ get; set; }
        public string? CustomerName { get; set; }
    }
}