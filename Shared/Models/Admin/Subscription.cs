using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models.Admin
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
