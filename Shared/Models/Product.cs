using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{
    public enum Currency {
        LBP,
        USD,
        EUR
    }
    public class Product : Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProductId { get; set; }

        public string ProductNo { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Url]
        public string Url { get; set; }
        public string Category { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        
    }
}
