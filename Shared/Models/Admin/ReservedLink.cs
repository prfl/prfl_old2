using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models.Admin
{
    public class ReservedLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservedLinkId { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }
}
