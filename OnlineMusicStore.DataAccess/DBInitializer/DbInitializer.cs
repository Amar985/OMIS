using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineMusic.DataAccess.Data;
using OnlineMusic.DataAccess.DbInitializer;
using OnlineMusic.Models;
using OnlineMusicStore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMusic.DataAccess.DbInitializer
{
    public class DbInitializer:IDbInitializer
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext db)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {

            //migrations if they are not applied


            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }catch (Exception ex) { }
            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();



                //if roles are not created then we will create an admin user role as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@omis.com",
                    Email = "admin@omis.com",
                    Name = "Amar Kumar",
                    PhoneNumber = "4567824593",
                    StreetAddress = "delhi, new delhi",
                    State = "DL",
                    PostalCode = "100100",
                    City = "Delhi"
                }, "Admin@omis#123*").GetAwaiter().GetResult();



                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@omis.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();



            }
            return;
        }
    }
}
