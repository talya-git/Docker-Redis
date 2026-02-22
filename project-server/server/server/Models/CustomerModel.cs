using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        [MaxLength(10)]
        [MinLength(8)]
        public string Phone { get; set; }
        [Required]

        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = "user";


        public List<CustomerDatails> CustomerPurchases { get; set; } = new List<CustomerDatails>();

    }
}
