using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Models
{
    public class TransactionHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int MerchantId { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Amount { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Type { get; set; }
    }
}
