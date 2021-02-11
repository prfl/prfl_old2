using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Profile.Shared.Models.Admin;

namespace Profile.Shared.Models
{

    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AccountId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifedOn { get; set; } = DateTime.Now;

        public string ProfileUserId { get; set; }
        public ProfileUser ProfileUser { get; set; }

        [Required]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        
        [Required]
        public string Username { get; set; }

        public bool IsFavorite { get; set; }
        public int Order { get; set; }
    }
}
