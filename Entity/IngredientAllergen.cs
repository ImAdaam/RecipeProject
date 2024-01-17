using System;
using System.Collections.Generic;

namespace RecipeProject.Entity
{
    public partial class IngredientAllergen
    {
        public int AllergenId { get; set; }
        public int IngredientId { get; set; }

        public virtual Allergen Allergen { get; set; } = null!;
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
}
