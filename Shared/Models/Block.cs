using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{

    public class Block
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifedOn { get; set; } = DateTime.Now;

        public string ProfileUserId { get; set; }
        public ProfileUser ProfileUser { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsFavorite { get; set; }
        public int Order { get; set; }
        
    }
}
