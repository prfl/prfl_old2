using System;
using System.Collections.Generic;
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
        Artist,
        Student,
        Teacher
    }

    public class ProfileUser : IdentityUser
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
