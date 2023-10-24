using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Models
{
    public class CardPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int MerchantId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string CardNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Expiration { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string SecurityCode { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double Amount { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string RecipitentEmail { get; set; }

        [Required]
        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
