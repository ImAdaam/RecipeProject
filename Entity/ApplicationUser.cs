using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RecipeProject.Entity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public virtual ICollection<UserFavouriteRecipe> UsersFavourite { get; set; }
    }
}