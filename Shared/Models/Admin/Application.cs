using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models.Admin
{

    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ApplicationId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifedOn { get; set; } = DateTime.Now;

        public string Name { get; set; }
        public string LogoLink { get; set; }
        public string ApplicationLink { get; set; }
        public string ApplicationUserLink { get; set; }

        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
    }
}
