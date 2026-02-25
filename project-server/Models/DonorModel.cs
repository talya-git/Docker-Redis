using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; 
namespace WebApplication1.Models
{
    public class DonorModel
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public List<GiftModel> Gifts { get; set; } = new List<GiftModel>();

    }
}
