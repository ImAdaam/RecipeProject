using System;
using System.Collections.Generic;

namespace RecipeProject.Entity
{
    public partial class Allergen : AbstractEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<IngredientAllergen> IngredientAllergens { get; set; }
    }
}
