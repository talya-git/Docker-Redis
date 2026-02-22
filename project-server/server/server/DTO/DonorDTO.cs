namespace WebApplication1.DTOs
{
    public class DonorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<GiftDto> Gifts { get; set; } = new List<GiftDto>();

    }
}