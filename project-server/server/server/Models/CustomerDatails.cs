namespace WebApplication1.Models
{
    public class CustomerDatails
    {

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public CustomerModel customer { get; set; }
        public int GiftId { get; set; }
        public GiftModel gift { get; set; }
        public int Quntity { get; set; }
        public string Status { get; set; } = "Draft";



    }
}
