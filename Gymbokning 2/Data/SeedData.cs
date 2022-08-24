using Gymbokning_2.Models;
using Microsoft.AspNetCore.Identity;

namespace Gymbokning_2.Data
{
    public class SeedData
    {
        private static ApplicationDbContext db = default;
        private static RoleManager<IdentityRole> roleManager = default;
        private static UserManager<ApplicationUser> userManager = default;

        public static async Task InitAsync
            (ApplicationDbContext context, IServiceProvider services, string adminPW)
        {
            if (string.IsNullOrWhiteSpace(adminPW))
                throw new Exception("Cant get password from config");
            if (context is null)
                throw new NullReferenceException(nameof(ApplicationDbContext));
            db = context;
            // if (db.Users.Any()) return;
            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            if (roleManager is null)
                throw new NullReferenceException(nameof(RoleManager<IdentityRole>));
            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            if (userManager is null)
                throw new NullReferenceException(nameof(UserManager<ApplicationUser>));


            var adminEmail = "admin@gymbokning.se";


            var applicationUser = new ApplicationUser
            {
                Email = adminEmail,
                EmailConfirmed = true,
                UserName = adminEmail,
                FirstName="Kalle",
                LastName="Anka"
              


            };


            //var found = await userManager.FindByEmailAsync("admin@gymboking.se");
            
            var aa = await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            var bb = await userManager.CreateAsync(applicationUser, adminPW);
            var cc = await userManager.AddToRoleAsync(applicationUser, "Admin");

        }
    }
}
