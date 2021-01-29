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
    public enum SubscriptionType {
        Free,
        Business,
        Enterprise
    }
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SubscriptionId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifedOn { get; set; } = DateTime.Now;

        public string ProfileUserId { get; set; }
        public ProfileUser ProfileUser { get; set; }
        public SubscriptionType Type { get; set; }
    }
}
