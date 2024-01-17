using System;
using System.Collections.Generic;

namespace RecipeProject.Entity
{
    public partial class Group : AbstractEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Material> Materials { get; set; }
    }
}
