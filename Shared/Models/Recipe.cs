using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{
    public enum RecipeType {
        Food,
        Beverage
    }
    public class Recipe : Block
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RecipeId { get; set; }

        public RecipeType Type { get; set; }

        [Url]
        public string Url { get; set; }

        public string Instruction { get; set; }

        public string Origin { get; set; }
        
    }
}
