using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Models
{
    public class Merchant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string StreetAddress { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string City { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string State { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Zip { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Country { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string FeeName { get; set; }

        [Column(TypeName = "float")]
        public double FeePercent { get; set; }

        [Column(TypeName = "float")]
        public double TaxRate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string ReceiptEmail { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string AppPassword { get; set; }
    }
}
