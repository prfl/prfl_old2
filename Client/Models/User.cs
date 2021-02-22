namespace Profile.Shared.Models
{
    public class User {
        public string UserId { get; set; }
        public ProfileUserType ProfileUserType { get; set; }
        public bool UserHasPhoneNumber { get; set; }
        public string ImageUrl { get; set; }
    }    
}