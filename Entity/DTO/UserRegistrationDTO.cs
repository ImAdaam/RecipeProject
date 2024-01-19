namespace RecipeProject.Entity.DTO
{
    public class UserRegistrationDTO
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsWriter { get; set; }
    }
}
