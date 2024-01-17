using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace RecipeProject.Entity
{
    public partial class Recipe : AbstractEntity
    {
        public string CodeName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Preparation_time { get; set; } = 0;
        public int Cooking_time { get; set; }

        [NotMapped]
        public int Total_time => Preparation_time + Cooking_time;


        public virtual ICollection<Material> Materials { get; set; }
    }
}
