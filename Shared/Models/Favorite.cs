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
    public enum LinkType {
        Account,
        Article,
        Link,
        Post,
        Project,
        Video,
        Schedule
    }
    public class Favorite
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FavoriteId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifedOn { get; set; } = DateTime.Now;

        public string LinkId { get; set; }
        public string ProfileUserId { get; set; }
        public ProfileUser ProfileUser { get; set; }
        public LinkType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public int Order { get; set; }
        
    }
}
