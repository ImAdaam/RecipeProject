using System;
using System.Collections.Generic;

namespace RecipeProject.Entity
{
    public partial class Ingredient : AbstractEntity
    {
        public string Name { get; set; } = null!;
        public int IngredientCategoryId { get; set; }

        public virtual IngredientCategory IngredientCategory { get; set; } = null!;
        public virtual ICollection<IngredientAllergen> IngredientAllergens { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
