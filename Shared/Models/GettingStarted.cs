using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{
    public class GettingStarted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string GettingStartedId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string ProfileUserId { get; set; }
        public ProfileUser ProfileUser { get; set; }
        public bool AccountIsCreated { get; set; }
        public bool LinkIsCreated { get; set; }
        public bool VideoIsCreated { get; set; }
        
    }
}
