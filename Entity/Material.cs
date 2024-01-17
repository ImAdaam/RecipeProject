using System;
using System.Collections.Generic;

namespace RecipeProject.Entity
{
    public partial class Material : AbstractEntity
    {
        public int GroupId { get; set; }
        public int IngredientId { get; set; }
        public int Quantity { get; set; }
        public string Measure { get; set; } = null!;
        public int RecipeId { get; set; }

        public virtual Group Group { get; set; } = null!;
        public virtual Ingredient Ingredient { get; set; } = null!;
        public virtual Recipe Recipe { get; set; } = null!;
    }
}
