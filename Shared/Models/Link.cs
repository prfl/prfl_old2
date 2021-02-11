using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{

    public class Link : Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string LinkId { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
        
    }
}
