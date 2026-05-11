using Microsoft.AspNetCore.Identity;

namespace HouseBroker.Infrastructure.Identity
{

    public static class SeedRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Broker"))
                await roleManager.CreateAsync(new IdentityRole("Broker"));

            if (!await roleManager.RoleExistsAsync("HouseSeeker"))
                await roleManager.CreateAsync(new IdentityRole("HouseSeeker"));
        }
    }
}
