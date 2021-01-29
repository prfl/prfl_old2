using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models.Codgram
{
    public enum Provider {
        SendGrid,
        Google,
        Facebook,
        Instagram,
        Mailchimp
    }
    public enum Entity {
        Datalk,
        ServiceManager,
        Shop,
        prfl,
    }
    public class API {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string APIId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifedOn { get; set; } = DateTime.Now;
        public Entity Entity { get; set; }
        public Provider Provider { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string APIKey { get; set; }
    }
}