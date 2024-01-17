using System;
using System.Collections.Generic;

namespace RecipeProject.Entity
{
    public partial class IngredientCategory : AbstractEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
