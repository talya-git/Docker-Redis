using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 
namespace WebApplication1.Models
{
    public class GiftModel
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)] 
        public string Name { get; set; }
        public int PriceCard { get; set; }
        public string GiftImage { get; set; }
        public string Category { get; set; }
        public int DonorId { get; set; }
        public DonorModel Donor { get; set; }
        public List<CustomerDatails> customerDatails { get; set; } = new List<CustomerDatails>();
        public int? WinnerId { get; set; }

        [ForeignKey(nameof(WinnerId))]
        public CustomerModel? Winner { get; set; }

    }
}
