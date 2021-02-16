using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Shared.Models
{
    public enum Measure {
        Piece,
        Cups,
        Dashe,
        Pinche,
        Tablespoon,
        Teaspoon,
        Drop,
        Gallon,
        Pint,
        Quart,
        Ounce,
        Milligram,
        Gram,
        Kilogram,
        Milliliter,
        Liter,
        Millimeter,
        Centimeter
    }
    
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IngredientId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; }
        public Measure Measure { get; set; }
        public int Order { get; set; }
        
    }
}
