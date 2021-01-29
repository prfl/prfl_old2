using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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

        public string Name { get; set; } // ex: Facebook
        public string LogoLink { get; set; } // ex: /assets/images/logos
        public string ApplicationLink { get; set; } //ex: https://facebook.com
        public string ApplicationUserLink { get; set; } // ex: https://linkedin.com/in/ or https://github.com/

        public string BackgroundColor { get; set; } // ex: #dcd9d9
        public string TextColor { get; set; } // ex: #dcd9d9
    }
}
