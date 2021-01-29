using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Profile.Shared.Models.Admin;

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
