using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Team1_FinalProject.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ReportsController : Controller
    {
        private AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public ReportsController(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {

            return View("Index");
        }

        //Aggregate Data Reporting
        public IActionResult DecisionReport()
        {
            return View();
        }

        
        private SelectList GetAllMovies()
        {
            List<Movie> moviesList = _context.Movies.Where(m => m.Showings.Count() != 0).ToList();
            List<Showing> allShowings = _context.Showings.ToList();

            Movie SelectNone = new Movie() { MovieID = 0, Title = "All Movies" };
            moviesList.Add(SelectNone);
            SelectList movieSelectList = new SelectList(moviesList.OrderBy(m => m.MovieID), "MovieID", "Title");

            return movieSelectList;

        }

        [HttpPost]
        public IActionResult DisplayReport(ReportViewModel svm)
        {
            var query = from t in _context.Tickets
                        select t;
            List<Order> Orders = new List<Order>();
            {
                Orders = _context.Orders.Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price)
                                        .Include(o => o.Customer)
                                        .ToList();
            }


            if (svm.SelectedMPAA != null)
            {
                query = query.Where(t => t.Showing.Movie.MPAA == svm.SelectedMPAA);
            }

            if (svm.MovieTitle != null && svm.MovieTitle != "")
            {
                query = query.Where(t => t.Showing.Movie.Title.Contains(svm.MovieTitle));
            }

            if (svm.PopcornPoints)
            {
                query = query.Where(t => t.Order.PopcornPointsUsed == true);

            }

            if (svm.SearchShowingDateStart != null)
            {
                DateTime starts = (DateTime)svm.SearchShowingDateStart;
                if (svm.SearchShowingDateEnd != null)
                {
                    DateTime ends = (DateTime)svm.SearchShowingDateEnd;
                    query = query.Where(t => t.Showing.StartDateTime.Date >= starts && t.Showing.StartDateTime.Date <= ends);

                }

                else
                {
                    query = query.Where(t => t.Showing.StartDateTime.Date == starts);
                }
            }

            if (svm.SearchShowingDateEnd != null && svm.SearchShowingDateStart == null)
            {
                DateTime ends = (DateTime)svm.SearchShowingDateEnd;
                query = query.Where(t => t.Showing.EndDateTime.Date == ends);
            }
            
            if (svm.SearchShowingTimeStart != null)
            {
                TimeSpan starts = (TimeSpan)svm.SearchShowingTimeStart;
                if (svm.SearchShowingTimeEnd != null)
                {
                    TimeSpan ends = (TimeSpan)svm.SearchShowingTimeEnd;                    
                    query = query.Where(m => m.Showing.StartDateTime.TimeOfDay >= starts && m.Showing.StartDateTime.TimeOfDay <= ends);

                }

                else
                {
                    query = query.Where(t => t.Showing.StartDateTime.TimeOfDay >= starts);
                }
            }


            if (svm.SearchShowingTimeEnd != null && svm.SearchShowingTimeStart == null)
            {
                TimeSpan ends = (TimeSpan)svm.SearchShowingTimeEnd;
                query = query.Where(t => t.Showing.StartDateTime.TimeOfDay <= ends);
                
            }
            
            if (svm.Movie != null && svm.Movie.MovieID != 0)
            {
                query = query.Where(t => t.Showing.Movie.MovieID == svm.Movie.MovieID);
            }


            svm.SeatsSold = query.Where(t => t.Order.OrderHistory == OrderHistory.Past).Count();
            List<Ticket> tickets = query.Where(t => t.Order.OrderHistory == OrderHistory.Past).ToList();

            foreach (Ticket ticket in tickets)
            {
                if (ticket.Order.PopcornPointsUsed == false)
                {
                    svm.TotalRevenue += ticket.Order.PostDiscount;
                }
                
            }
            ViewBag.SummarizedTickets = tickets;

            return View(svm);

        }


        //Popcorn Points Reporting
        public IActionResult PPReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DisplayPP(ReportViewModel rvm)
        {
            var query = from o in _context.Tickets
                        select o;
            if (rvm.PopcornPoints == true)
            {
                query = query.Where(o => o.Order.PopcornPointsUsed == true);
            }
            List<Ticket> tickets = query
                .Include(t => t.Order)
                .ThenInclude(o => o.Customer)
                .Include(t => t.Order)
                .ThenInclude(o => o.Discount)
                .Include(t => t.Showing)
                .ThenInclude(s => s.Movie)
                .ToList();

            ViewBag.pptickets = tickets;
            return View(tickets);
        }



        //Customer Reports Stuff
        //select list 
        private async Task<SelectList> GetAllUsers()
        {
            //Get the list of users from the database
            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            List<Int32> customerIDList = new List<Int32>();
            Int32 count = 0;
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Customer") == true) //user is in the role
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

            SelectList customerSelectList = new SelectList(customerList, "SelectCustomerID", "SelectCustomerName");


            return customerSelectList;
        }
        public async Task<IActionResult> CustomerSearch()
        {
            ViewBag.AllCustomers = await GetAllUsers();
            CustomerReportViewModel crvm = new CustomerReportViewModel();
            return View(crvm);
        }
        [HttpPost]
        public async Task<IActionResult> DisplayCustomerReport (CustomerReportViewModel crvm)
        {

            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Customer") == true) //user is in the role
                {
                    CustomerReportViewModel newcus = new CustomerReportViewModel();
                    newcus.SelectCustomerName = user.Email;
                    customerList.Add(newcus);
                }
            }
            AppUser customer = _userManager.Users.Where(u => u.Email == customerList[crvm.SelectedCustomerID].SelectCustomerName).First();            
            List<Order> orders = _context.Orders.Include(o => o.Discount)
                                            .Include(o => o.Tickets)
                                            .ThenInclude(t => t.Showing)
                                            .ThenInclude(s => s.Movie)
                                            .Include(o => o.Tickets)
                                            .ThenInclude(t => t.Showing)
                                            .ThenInclude(s => s.Price).Where(o => o.Customer.UserName == customer.Email).ToList();

            foreach (Order order in orders)
            {
                if (order.OrderHistory == OrderHistory.Past)
                {
                    if (order.PopcornPointsUsed == false)
                    {
                        crvm.TotalRevenue += order.PostDiscount;
                    }
                    else if (order.PopcornPointsUsed == true)
                    {
                        crvm.PopcornPointsUsed += (order.Tickets.Count() * 100);
                    }
                    crvm.SeatsSold += order.Tickets.Count();
                }
            }
            ViewBag.CustomerOrders = orders;
            return View(crvm);
        }


        [HttpPost]
        public async Task<IActionResult> AggregateCustomerReport()
        {
            List<CustomerReportViewModel> customerList = new List<CustomerReportViewModel>();
            foreach (AppUser user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Customer") == true) //user is in the role
                {
                    CustomerReportViewModel newcus = new CustomerReportViewModel();
                    newcus.SelectCustomerName = user.Email;
                    customerList.Add(newcus);
                }
            }

            foreach (CustomerReportViewModel c in customerList)
            {
                //AppUser customer = _userManager.Users.Where(u => u.Email == customerList[c.SelectedCustomerID].SelectCustomerName).First();
                List<Order> orders = _context.Orders.Include(o => o.Discount)
                                            .Include(o => o.Tickets)
                                            .ThenInclude(t => t.Showing)
                                            .ThenInclude(s => s.Movie)
                                            .Include(o => o.Tickets)
                                            .ThenInclude(t => t.Showing)
                                            .ThenInclude(s => s.Price).Where(o => o.Customer.UserName == c.SelectCustomerName).ToList();
                foreach (Order order in orders)
                {
                    if (order.OrderHistory == OrderHistory.Past)
                    {
                        if (order.PopcornPointsUsed == false)
                        {
                            c.TotalRevenue += order.PostDiscount;
                        }
                        else if (order.PopcornPointsUsed == true)
                        {
                            c.PopcornPointsUsed += (order.Tickets.Count() * 100);
                        }
                        c.SeatsSold += order.Tickets.Count();
                    }
                }
            }

            return View(customerList);
            

        }
    }
}
