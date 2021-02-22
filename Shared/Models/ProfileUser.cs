using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Profile.Shared.Models
{
    public enum ProfileUserType {
        Default,
        Company,
        Trainer,
        Shop,
        Creator,
        Developer,
        Analyst,
        Artist,
        Chef,
        Bartender
    }

    public class ProfileUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ProfileUserType ProfileUserType { get; set; }
        public string ImageUrl { get; set; }
    }
}
