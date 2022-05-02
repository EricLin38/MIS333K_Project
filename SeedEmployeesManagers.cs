using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Change these using statements to match your project
using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;


//TODO: Change this namespace to match your project
namespace Team1_FinalProject.Seeding
{
    //add identity data
    public static class SeedIdentity
    {
        public static async Task AddEmployee(IServiceProvider serviceProvider)
        {
            //Get instances of the services needed to add a user & add a user to a role
            AppDbContext _context = serviceProvider.GetRequiredService<AppDbContext>();
            UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //TODO: Add the needed roles
            //if the admin role doesn't exist, add it
            //if the customer role doesn't exist, add it
			if (await _roleManager.RoleExistsAsync("Employee") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }
			
			if (await _roleManager.RoleExistsAsync("Manager") == false)
            {
                await _roleManager.CreateAsync(new IdentityRole("Manager"));
            }

			AppUser newUser = new AppUser();
			
			newUser.UserName = "t.jacobs@mainstreetmovies.com";
			newUser.Email = "t.jacobs@mainstreetmovies.com";
			newUser.PhoneNumber = "9074653365";
			newUser.FirstName = "Jacobs";
			newUser.MiddleInitial = "L";
			newUser.LastName = "Todd";
			newUser.Birthdate = new DateTime(1958,4,25);
			newUser.Address = "4564 Elm St. Georgetown, TX78628";
			
			IdentityResult result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"toddyboy");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "t.jacobs@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "e.rice@mainstreetmovies.com";
			newUser.Email = "e.rice@mainstreetmovies.com";
			newUser.PhoneNumber = "9073876657";
			newUser.FirstName = "Rice";
			newUser.MiddleInitial = "M";
			newUser.LastName = "Eryn";
			newUser.Birthdate = new DateTime(1959,7,2);
			newUser.Address = "3405 Rio Grande Austin, TX78746";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"ricearoni");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "e.rice@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Manager") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Manager");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "a.taylor@mainstreetmovies.com";
			newUser.Email = "a.taylor@mainstreetmovies.com";
			newUser.PhoneNumber = "9074748452";
			newUser.FirstName = "Taylor";
			newUser.MiddleInitial = "R";
			newUser.LastName = "Allison";
			newUser.Birthdate = new DateTime(1964,9,2);
			newUser.Address = "467 Nueces St. Austin, TX78727";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"nostalgic");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "a.taylor@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "g.martinez@mainstreetmovies.com";
			newUser.Email = "g.martinez@mainstreetmovies.com";
			newUser.PhoneNumber = "9078746718";
			newUser.FirstName = "Martinez";
			newUser.MiddleInitial = "R";
			newUser.LastName = "Gregory";
			newUser.Birthdate = new DateTime(1992,3,30);
			newUser.Address = "8295 Sunset Blvd. Austin, TX78712";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"fungus");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "g.martinez@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "m.sheffield@mainstreetmovies.com";
			newUser.Email = "m.sheffield@mainstreetmovies.com";
			newUser.PhoneNumber = "9075479167";
			newUser.FirstName = "Sheffield";
			newUser.MiddleInitial = "J";
			newUser.LastName = "Martin";
			newUser.Birthdate = new DateTime(1996,12,29);
			newUser.Address = "3886 Avenue A San Marcos, TX78666";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"longhorns");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "m.sheffield@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "j.tanner@mainstreetmovies.com";
			newUser.Email = "j.tanner@mainstreetmovies.com";
			newUser.PhoneNumber = "9074590929";
			newUser.FirstName = "Tanner";
			newUser.MiddleInitial = "S";
			newUser.LastName = "Jeremy";
			newUser.Birthdate = new DateTime(1970,8,12);
			newUser.Address = "4347 Almstead Austin, TX78712";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"tanman");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "j.tanner@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Manager") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Manager");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "m.rhodes@mainstreetmovies.com";
			newUser.Email = "m.rhodes@mainstreetmovies.com";
			newUser.PhoneNumber = "9073744746";
			newUser.FirstName = "Rhodes";
			newUser.MiddleInitial = "C";
			newUser.LastName = "Megan";
			newUser.Birthdate = new DateTime(1970,12,18);
			newUser.Address = "4587 Enfield Rd. Austin, TX78729";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"countryrhodes");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "m.rhodes@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "e.stuart@mainstreetmovies.com";
			newUser.Email = "e.stuart@mainstreetmovies.com";
			newUser.PhoneNumber = "9078178335";
			newUser.FirstName = "Stuart";
			newUser.MiddleInitial = "F";
			newUser.LastName = "Eric";
			newUser.Birthdate = new DateTime(1971,3,11);
			newUser.Address = "5576 Toro Ring San Antonio, TX78758";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"stewboy");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "e.stuart@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "r.taylor@mainstreetmovies.com";
			newUser.Email = "r.taylor@mainstreetmovies.com";
			newUser.PhoneNumber = "9074512631";
			newUser.FirstName = "Taylor";
			newUser.MiddleInitial = "O";
			newUser.LastName = "Rachel";
			newUser.Birthdate = new DateTime(1972,12,20);
			newUser.Address = "345 Longview Dr. Austin, TX78746";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"swansong");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "r.taylor@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Manager") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Manager");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "v.lawrence@mainstreetmovies.com";
			newUser.Email = "v.lawrence@mainstreetmovies.com";
			newUser.PhoneNumber = "9079457399";
			newUser.FirstName = "Lawrence";
			newUser.MiddleInitial = "Y";
			newUser.LastName = "Tori";
			newUser.Birthdate = new DateTime(1973,4,28);
			newUser.Address = "6639 Butterfly Ln. Austin, TX78712";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"lottery");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "v.lawrence@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "a.rogers@mainstreetmovies.com";
			newUser.Email = "a.rogers@mainstreetmovies.com";
			newUser.PhoneNumber = "9078752943";
			newUser.FirstName = "Alberts";
			newUser.MiddleInitial = "H";
			newUser.LastName = "Allen";
			newUser.Birthdate = new DateTime(1978,6,21);
			newUser.Address = "4965 Oak Hill Austin, TX78705";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"evanescent");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "a.rogers@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Manager") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Manager");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "c.baker@mainstreetmovies.com";
			newUser.Email = "c.baker@mainstreetmovies.com";
			newUser.PhoneNumber = "9075571146";
			newUser.FirstName = "Baker";
			newUser.MiddleInitial = "E";
			newUser.LastName = "Christopher";
			newUser.Birthdate = new DateTime(1993,3,16);
			newUser.Address = "1245 Lake Anchorage Blvd. Cedar Park, TX78613";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"hecktour");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "c.baker@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "w.sewell@mainstreetmovies.com";
			newUser.Email = "w.sewell@mainstreetmovies.com";
			newUser.PhoneNumber = "9074510084";
			newUser.FirstName = "Sewell";
			newUser.MiddleInitial = "G";
			newUser.LastName = "William";
			newUser.Birthdate = new DateTime(1986,5,25);
			newUser.Address = "2365 51st St. Austin, TX78755";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"walkamile");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "w.sewell@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "j.mason@mainstreetmovies.com";
			newUser.Email = "j.mason@mainstreetmovies.com";
			newUser.PhoneNumber = "9018833432";
			newUser.FirstName = "Mason";
			newUser.MiddleInitial = "L";
			newUser.LastName = "Jack";
			newUser.Birthdate = new DateTime(1986,6,6);
			newUser.Address = "444 45th St Austin, TX78701";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"changalang");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "j.mason@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "j.jackson@mainstreetmovies.com";
			newUser.Email = "j.jackson@mainstreetmovies.com";
			newUser.PhoneNumber = "9075554545";
			newUser.FirstName = "Jackson";
			newUser.MiddleInitial = "J";
			newUser.LastName = "Jack";
			newUser.Birthdate = new DateTime(1989,10,16);
			newUser.Address = "222 Main Austin, TX78760";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"offbeat");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "j.jackson@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "m.nguyen@mainstreetmovies.com";
			newUser.Email = "m.nguyen@mainstreetmovies.com";
			newUser.PhoneNumber = "9075524141";
			newUser.FirstName = "Nguyen";
			newUser.MiddleInitial = "M";
			newUser.LastName = "Andy";
			newUser.Birthdate = new DateTime(1988,4,5);
			newUser.Address = "465 N. Bear Cub Austin, TX78734";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"landus");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "m.nguyen@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Manager") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Manager");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "s.barnes@mainstreetmovies.com";
			newUser.Email = "s.barnes@mainstreetmovies.com";
			newUser.PhoneNumber = "9556662323";
			newUser.FirstName = "Barnes";
			newUser.MiddleInitial = "M";
			newUser.LastName = "Susan";
			newUser.Birthdate = new DateTime(1993,2,22);
			newUser.Address = "888 S. Main Kyle, TX78640";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"rhythm");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "s.barnes@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "l.jones@mainstreetmovies.com";
			newUser.Email = "l.jones@mainstreetmovies.com";
			newUser.PhoneNumber = "9886662222";
			newUser.FirstName = "Johns";
			newUser.MiddleInitial = "L";
			newUser.LastName = "Sarah";
			newUser.Birthdate = new DateTime(1996,6,29);
			newUser.Address = "999 LeBlat Austin, TX78747";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"kindly");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "l.jones@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "c.silva@mainstreetmovies.com";
			newUser.Email = "c.silva@mainstreetmovies.com";
			newUser.PhoneNumber = "9221113333";
			newUser.FirstName = "Silva";
			newUser.MiddleInitial = "S";
			newUser.LastName = "Cindy";
			newUser.Birthdate = new DateTime(1997,12,29);
			newUser.Address = "900 4th St Austin, TX78758";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"arched");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "c.silva@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			
			newUser = new AppUser();
			
			newUser.UserName = "s.rankin@mainstreetmovies.com";
			newUser.Email = "s.rankin@mainstreetmovies.com";
			newUser.PhoneNumber = "9893336666";
			newUser.FirstName = "Rankin";
			newUser.MiddleInitial = "R";
			newUser.LastName = "Suzie";
			newUser.Birthdate = new DateTime(1999,12,17);
			newUser.Address = "23 Polar Bear Road Buda, TX78712";
			
			result = new IdentityResult();
			result = await _userManager.CreateAsync(newUser,"decorate");
			
			_context.SaveChanges();
			newUser = _context.Users.FirstOrDefault(u => u.UserName == "s.rankin@mainstreetmovies.com");
			
			if (await _userManager.IsInRoleAsync(newUser, "Employee") == false)
			{
				await _userManager.AddToRoleAsync(newUser, "Employee");
			}
			
			_context.SaveChanges();
			

			
			
        }

    }
}