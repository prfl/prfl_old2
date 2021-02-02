using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
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
        Artist
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
    }
}
