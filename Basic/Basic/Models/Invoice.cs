using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int MerchantId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }

        [Required]
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

        [Required]
        [Column(TypeName = "datetimeoffset")]
        public DateTimeOffset Timestamp { get; set; }


    }
}
