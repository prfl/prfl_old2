using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{
    public enum Weekday {
        
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        MWF, //Monday, Wednesday, Friday
        TTHS, //Tuesday, Thursday, Saturday
        Everyday
    }
    public class Schedule : Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ScheduleId { get; set; }
        public string Location { get; set; }
        public Weekday Weekday { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [Url]
        public string Url { get; set; }
        
    }
}
