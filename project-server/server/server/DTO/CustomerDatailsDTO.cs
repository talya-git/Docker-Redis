namespace WebApplication1.DTOs
{
    public class CustomerDetailsDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int GiftId { get; set; }

        public string GiftName { get; set; }
        public double GiftPrice { get; set; } 

        public int Quntity { get; set; }

        public string Status { get; set; }

    }
}