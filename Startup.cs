using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

//TODO: Make these using statements match your project
using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;


//TODO: Make this namespace match your project - be sure to remove the []
namespace Team1_FinalProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //This adds the MVC engine and Razor code
            services.AddControllersWithViews();

            //TODO: (For HW4 and beyond) Add a connection string here once you have created it on Azure
            var connectionString = "Server=tcp:fa20team1project.database.windows.net,1433;Initial Catalog=fal20team1finalproject;Persist Security Info=False;User ID=admin@example.com@fa20team1project;Password=Abc123!!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            /*var connectionString = "Server=tcp:fa20finalprojectnoshowings.database.windows.net,1433;Initial Catalog=fa20finalprojectnoshowings;Persist Security Info=False;User ID=admin@example.com@fa20finalprojectnoshowings;Password=Abc123!!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";*/
            /*var connectionString = "Server=tcp:fa20team1finalprojtestwoo.database.windows.net,1433;Initial Catalog=fa20team1finalprojtest;Persist Security Info=False;User ID=admin@example.com@fa20team1finalprojtestwoo;Password=Abc123!!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";*/
            /*^ main testing database*/
            /*
            var connectionString = "Server=tcp:team1test.database.windows.net,1433;Initial Catalog=Team1_FinalProjectTest;Persist Security Info=False;User ID=admin@example.com@team1test;Password=Abc123!!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";             //TODO: Uncomment this line once you have your connection string
            ^this is the old connection string
            */
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            //TODO: Uncomment these lines once you have added Identity to your project
            ////NOTE: This is where you would change your password requirements
            services.AddIdentity<AppUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider service)
        {
            //These lines allow you to see more detailed error messages
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            app.UseAuthentication();

            //This line allows you to use static pages like style sheets and images
            app.UseStaticFiles();

            //This marks the position in the middleware pipeline where a routing decision
            //is made for a URL.
            app.UseRouting();

            //This allows the data annotations for currency to work on a mac
            app.Use(async (context, next) =>
            {
                CultureInfo.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

                await next.Invoke();
            });

            //TODO: Once you have added Identity into your project, you will need to uncomment these lines
            app.UseAuthorization();

            //This method maps the controllers and their actions to a patter for
            //requests that's known as the default route. This route identifies
            //the Home controller as the default controller and the Index() action
            //method as the default action. The default route also identifies a 
            //third segment of the URL that's a parameter named id.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //TODO: Uncomment this after admin is seeded
            //This seeds the admin user
            /*Seeding.SeedCustomers.AddCustomer(service).Wait();
            Seeding.SeedEmployeesManagers.AddEmployee(service).Wait();*/
        }
    }   
}