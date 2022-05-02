using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//TODO: Change these using statements to match your project
using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;

//TODO: Change this namespace to match your project
namespace Team1_FinalProject.Controllers
{
    //TODO: Uncomment this line once you have roles working correctly
    [Authorize(Roles = "Manager")]
    public class RoleAdminController : Controller
    {
        //create private variables for the services needed in this controller
        private AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;


        //RoleAdminController constructor
        public RoleAdminController(AppDbContext appDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //populate the values of the variables passed into the controller
            _context = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /RoleAdmin/
        public async Task<ActionResult> Index()
        {
            //Create a list of roles that will need to be edited
            List<RoleEditModel> roles = new List<RoleEditModel>();
            var allRoles = _roleManager.Roles.Where(r => r.Name != "Customer").ToList();
            
            //loop through each of the existing roles
            foreach (IdentityRole role in allRoles)
            {
                //this is a list of all the users who ARE in this role (members)
                List<AppUser> members = new List<AppUser>();

                //this is a list of all the users who ARE NOT in this role (non-members)
                List<AppUser> nonMembers = new List<AppUser>();

                //loop through ALL the users and decide if they are in the role(member) or not (non-member)
                //every user will be evaluated for every role, so this is a SLOW chunk of code because
                //it accesses the database so many times
                foreach (AppUser user in _userManager.Users)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name) == true) //user is in the role
                    {
                        //add user to list of members
                        members.Add(user);
                    }
                    else //user is NOT in the role
                    {
                        //add user to list of non-members
                        nonMembers.Add(user);
                    }
                }

                //create a new instance of the role edit model
                RoleEditModel rem = new RoleEditModel();

                //populate the properties of the role edit model
                rem.Role = role; //role from database
                rem.Members = members; //list of users in this role
                rem.NonMembers = nonMembers; //list of users NOT in this role
                
                //add this role to the list of role edit models
                roles.Add(rem);  
            }

            //pass the list of roles to the view
            return View(roles);
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                //attempt to create the new role using the role manager
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));

                //if the role was created successfully, take the user to the index page
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else //role was not added succesfully, so add errors to model 
                //and let the user try again
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            //if code gets this far, we need to show an error
            return View(name);
        }

        public async Task<ActionResult> Edit(string id)
        {
            //look up the role requested by the user
            IdentityRole role = await _roleManager.FindByIdAsync(id);

            //create a list for the members of the role
            List<AppUser> members = new List<AppUser>();

            //create a list for the non-members of the role
            List<AppUser> nonMembers = new List<AppUser>();

            //through ALL the users and decide if they are in the role(member) or not (non-member)
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name) == true) //user is in the role
                {
                    //add the user to the list of members
                    members.Add(user);
                }
                else if ((await _userManager.IsInRoleAsync(user, "Manager") == true || await _userManager.IsInRoleAsync(user, "FiredEmployee") == true) && (role.Name == "Employee")) //user is NOT in the role
                {
                    nonMembers.Add(user);
                }
                else if (await _userManager.IsInRoleAsync(user, "Employee") == true && (role.Name == "Manager")) //user is NOT in the role
                {
                    nonMembers.Add(user);
                }
                else if (await _userManager.IsInRoleAsync(user, "Employee") == true && (role.Name == "FiredEmployee"))
                {
                    nonMembers.Add(user);
                }

            }

            //create a new instance of the role edit model
            RoleEditModel rem = new RoleEditModel();

            //populate the properties of the role edit model
            rem.Role = role; //role looked up from database
            rem.Members = members; //list of users in the role
            rem.NonMembers = nonMembers; //list of users NOT in the role

            //send user to view with populated role edit model
            return View(rem);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel rmm)
        {
            //create a result to refer to later
            IdentityResult result;

            //if RoleModificationModel is valid, add new users
            if (ModelState.IsValid)
            {
                //if there are users to add, then add them
                if (rmm.IdsToAdd != null)
                {
                    foreach (string userId in rmm.IdsToAdd)
                    {
                        //find the user in the database using their id
                        AppUser user = await _userManager.FindByIdAsync(userId);

                        //attempt to add the user to the role using the UserManager
                        result = await _userManager.AddToRoleAsync(user, rmm.RoleName);
                        
                        //if attempt to add user to role didn't work, show user the error page
                        if (result.Succeeded == false)
                        {
                            //send user to error page
                            return View("Error", result.Errors);
                        }
                    }
                }

                //if there are users to remove from the role, remove them
                if (rmm.IdsToDelete != null)
                {
                    //loop through all the ids to remove from role
                    foreach (string userId in rmm.IdsToDelete)
                    {
                        //find the user in the database using their id
                        AppUser user = await _userManager.FindByIdAsync(userId);

                        //attempt to remove the user from the role using the UserManager
                        result = await _userManager.RemoveFromRoleAsync(user, rmm.RoleName);

                        //if attempt to remove the user from role didn't work, show the error page
                        if (result.Succeeded == false)
                        {
                            //show user the error page
                            return View("Error", result.Errors);
                        }
                    }
                }

                //this is the happy path - all edits worked
                //take the user back to the RoleAdmin Index page
                return RedirectToAction("Index");
            }

            //this is a sad path - the role was not found
            //show the user the error page
            return View("Error", new string[] { "Role Not Found" });
        }

        //select list 
        private async Task<SelectList> GetAllEmployees()
        {
            //Get the list of users from the database
            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            List<Int32> customerIDList = new List<Int32>();
            Int32 count = 0;
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Employee") == true) //user is in the role
                {
                    if (user.EStatus == EmploymentStatus.Hired)
                    {
                        //add user to list of member
                        CustomerReportViewModel newcus = new CustomerReportViewModel();
                        newcus.SelectCustomerName = user.Email;
                        newcus.SelectCustomerID = count;
                        customerIDList.Add(count);
                        customerList.Add(newcus);
                        count += 1;
                    }
                }
            }

            SelectList customerSelectList = new SelectList(customerList, "SelectCustomerID", "SelectCustomerName");


            return customerSelectList;
        }

        [HttpGet]
        public async Task<ActionResult> ManageEmployeesIndex()
        {
            ViewBag.AllEmployees =  await GetAllEmployees();
            ViewBag.FiredEmployees = await GetAllFiredEmployees();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FireEmployeesIndex(CustomerReportViewModel crvm)
        {
            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Employee") == true) //user is in the role
                {
                    CustomerReportViewModel newcus = new CustomerReportViewModel();
                    newcus.SelectCustomerName = user.Email;
                    customerList.Add(newcus);
                }
            }
            AppUser customer = _userManager.Users.Where(u => u.Email == customerList[crvm.SelectedCustomerID].SelectCustomerName).First();

            customer.EStatus = EmploymentStatus.Fired;
            return View("ConfirmFire", customer);
        }

        private async Task<SelectList> GetAllFiredEmployees()
        {
            //Get the list of users from the database
            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            List<Int32> customerIDList = new List<Int32>();
            Int32 count = 0;
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Employee") == true) //user is in the role
                {
                    if (user.EStatus == EmploymentStatus.Fired)
                    {
                        //add user to list of members
                        CustomerReportViewModel newcus = new CustomerReportViewModel();
                        newcus.SelectCustomerName = user.Email;
                        newcus.SelectCustomerID = count;
                        customerIDList.Add(count);
                        customerList.Add(newcus);
                        count += 1;
                    }
                }
            }

            SelectList customerSelectList = new SelectList(customerList, "SelectCustomerID", "SelectCustomerName");


            return customerSelectList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReHireEmployeesIndex(CustomerReportViewModel crvm)
        {
            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Employee") == true) //user is in the role
                {
                    CustomerReportViewModel newcus = new CustomerReportViewModel();
                    newcus.SelectCustomerName = user.Email;
                    customerList.Add(newcus);
                }
            }
            AppUser customer = _userManager.Users.Where(u => u.Email == customerList[crvm.SelectedCustomerID].SelectCustomerName).First();

            customer.EStatus = EmploymentStatus.Hired;
            return View("ConfirmFire", customer);
        }


    }
}

//limit login if employemnt status is fired in account login and role admin display