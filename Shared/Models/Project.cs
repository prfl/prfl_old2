using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{

    public class Project : Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProjectId { get; set; }
        
        [Required]
        [Url]
        public string Url { get; set; }
        
    }
}
