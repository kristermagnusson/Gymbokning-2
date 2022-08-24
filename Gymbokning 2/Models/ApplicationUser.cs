using Microsoft.AspNetCore.Identity;

namespace Gymbokning_2.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string FullName => @"{firstname} {lastName}";
        //public string FullName => $"{FirstName} {LastName}";
        public string FullName { get; set; }
    }
}
