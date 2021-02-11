using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{

    public class Video : Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string VideoId { get; set; }
        
        [Required]
        [Url]
        public string Url { get; set; }
    }
}
