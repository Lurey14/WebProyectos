using AplicacionPedidos.Models;
using Microsoft.AspNetCore.Identity;

namespace AplicacionPedidos.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Cliente", "Empleado" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Nombre = "Administrador",
                    Rol = "Admin"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var clienteEmail = "cliente@gmail.com";
            var clienteUser = await userManager.FindByEmailAsync(clienteEmail);
            
            if (clienteUser == null)
            {
                clienteUser = new ApplicationUser
                {
                    UserName = clienteEmail,
                    Email = clienteEmail,
                    EmailConfirmed = true,
                    Nombre = "Cliente Demo",
                    Rol = "Cliente"
                };

                var result = await userManager.CreateAsync(clienteUser, "Cliente123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(clienteUser, "Cliente");
                }
            }

            var empleadoEmail = "empleado@gmail.com";
            var empleadoUser = await userManager.FindByEmailAsync(empleadoEmail);
            
            if (empleadoUser == null)
            {
                empleadoUser = new ApplicationUser
                {
                    UserName = empleadoEmail,
                    Email = empleadoEmail,
                    EmailConfirmed = true,
                    Nombre = "Empleado Demo",
                    Rol = "Empleado"
                };

                var result = await userManager.CreateAsync(empleadoUser, "Empleado123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(empleadoUser, "Empleado");
                }
            }
        }
    }
}