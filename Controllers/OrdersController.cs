using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;
using Team1_FinalProject.Utilities;


namespace Team1_FinalProject.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index() //Order History
        {
            List<Order> Orders = new List<Order>();
            if (User.IsInRole("Manager") || User.IsInRole("Employee"))
            {
                Orders = _context.Orders.Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price)
                                        .ToList();
            }
            else
            {
                Orders = _context.Orders.Where(o => o.Customer.UserName == User.Identity.Name)
                                        .Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price)
                                        .ToList();
            }

            return View(Orders);
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = _context.Orders
                .Include(o => o.Discount)
                .Include(o => o.Tickets)
                .ThenInclude(o => o.Showing)
                .ThenInclude(o => o.Movie)
                .Include(o => o.Tickets)
                .ThenInclude(o => o.Showing)
                .ThenInclude(o => o.Price)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.OrderID == id);

            if (order.Discount != null)
            {
                if (order.Tickets.Where(t => t.Showing.SpecialEvent == false).Count() >= 2)
                {
                    ViewBag.DiscountTax = string.Format("{0:C}", (order.OrderSubtotal + (order.Discount.PriceValue * 2)) * .0825m);
                }
                else if (order.Tickets.Where(t => t.Showing.SpecialEvent == false).Count() == 1)
                {
                    ViewBag.DiscountTax = string.Format("{0:C}", (order.OrderSubtotal + order.Discount.PriceValue) * .0825m);
                }
                else
                {
                    ViewBag.DiscountTax = string.Format("{0:C}", order.Tax);
                }
            }

            else
            {
                ViewBag.DiscountTax = string.Format("{0:C}", order.Tax);
            }
            if (order == null)
            {
                return NotFound();
            }

            int ticketcount = 0;
            foreach (Ticket ticket in order.Tickets)
            {
                ticketcount += 1;
            }
            ViewBag.ticketcounter = ticketcount;
            //make sure a customer isn't trying to look at someone else's order
            string email = order.GiftEmail;
            if (User.IsInRole("Customer") && (order.Customer.UserName != User.Identity.Name && User.Identity.Name != order.GiftEmail))
            {
                return View("Error", new string[] { "You are not authorized to view this order!" });
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,GiftOrder,PopcornPoints,Date")] Order order)
        {
            if (ModelState.IsValid == false)
            {
                return View(order);
            }
            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,GiftOrder,PopcornPoints,Date")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public SelectList GetAllActiveOrders()
        {
            List<CheckoutForViewModel> cfvm = new List<CheckoutForViewModel>();
            List<Order> activeorders = _context.Orders.Include(o => o.Customer).Where(o => o.OrderHistory == OrderHistory.Future).ToList();
            foreach (Order o in activeorders)
            {
                CheckoutForViewModel temp = new CheckoutForViewModel();
                temp.SelectOrderID = o.OrderID;
                temp.SelectOrderName = o.Customer.UserName + ": Order " + o.OrderID.ToString();
                cfvm.Add(temp);
            }
            SelectList orderSelectList = new SelectList(cfvm.OrderBy(m => m.SelectOrderID), "SelectOrderID", "SelectOrderName");
            return orderSelectList;
        }

        [HttpGet]
        public IActionResult CheckoutFor()
        {
            ViewBag.AllOrders = GetAllActiveOrders();
            CheckoutForViewModel cfvm = new CheckoutForViewModel();
            return View(cfvm);
        }
        [HttpPost]
        public IActionResult CheckoutFor([Bind("SelectedOrderID")] CheckoutForViewModel cfvm)
        {
            return RedirectToAction("Checkout", "Orders", new { id = cfvm.SelectedOrderID });
        }
        // Checkout
        // add if statement to check if list of tickets is null , then send error 
        // also if statement for purchasing tickets of multiple showings for same movie
        public async Task<IActionResult> Checkout(int? id)
        {
            if (User.IsInRole("Manager") || User.IsInRole("Employee"))
            {
                if (id == null)
                {
                    return RedirectToAction("CheckoutFor", "Orders");
                }

                Order currorder = _context.Orders.Include(o => o.Tickets).ThenInclude(o => o.Showing)
                    .ThenInclude(o => o.Movie).Include(o => o.Tickets).ThenInclude(o => o.Showing)
                    .ThenInclude(o => o.Price).Include(o => o.Customer).FirstOrDefault(o => o.OrderID == id);
                List<Ticket> tickets = _context.Tickets.Where(t => t.Order.OrderID == currorder.OrderID).ToList();

                if (tickets.Count() == 0)
                {
                    return View("Error", new String[] { "Your cart is empty. Please choose what to purchase first" });

                }
                else
                {
                    currorder.Date = DateTime.Now;
                    currorder.OrderHistory = OrderHistory.Future;
                    currorder.PopcornPointsUsed = false;
                    currorder.GiftOrder = false;
                    currorder.GiftEmail = null;
                    await _context.SaveChangesAsync();
                    return View(currorder);
                }
            }
            else
            {
                Order currorder = _context.Orders.Include(o => o.Tickets).ThenInclude(o => o.Showing)
                    .ThenInclude(o => o.Movie).Include(o => o.Tickets).ThenInclude(o => o.Showing)
                    .ThenInclude(o => o.Price).Where(o => o.Customer.UserName == User.Identity.Name)
                    .Where(o => o.OrderHistory == OrderHistory.Future).FirstOrDefault();

                if (currorder == null)
                {
                    return View("Error", new String[] { "Your cart is empty. Please choose what to purchase first" });

                }
                else
                {
                    currorder.Date = DateTime.Now;
                    currorder.Customer = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    currorder.OrderHistory = OrderHistory.Future;
                    currorder.PopcornPointsUsed = false;
                    currorder.GiftOrder = false;
                    currorder.GiftEmail = null;
                    await _context.SaveChangesAsync();
                    return View(currorder);
                }
            }
        }

        

        // Review [GET]
        public IActionResult Review([Bind("OrderID, PopcornPointsUsed, GiftOrder, GiftEmail")] Order order)
        {
            Order currorder = _context.Orders.Include(o => o.Discount).Include(o => o.Tickets).ThenInclude(o => o.Showing)
            .ThenInclude(o => o.Movie).Include(o => o.Tickets).ThenInclude(o => o.Showing)
            .ThenInclude(o => o.Price).Include(o => o.Customer).FirstOrDefault(o => o.OrderID == order.OrderID);
            ViewBag.Discount = "N/A";

            if (currorder.Tickets.Count() == 0)
            {
                return View("Error", new String[] { "Your cart is empty. Please choose what to purchase first" });
            }
            if (order.GiftOrder == false && order.GiftEmail != null)
            {
                return View("Error", new String[] { "If you want to checkout as a gift, make sure to check next to Gift Order!" });
            }

            if (order.PopcornPointsUsed == true)
            {
                foreach (Ticket t in currorder.Tickets)
                {
                    if (t.Showing.SpecialEvent == true)
                    {
                        return View("Error", new String[] { "You cannot use PopcornPoints on this order because one or more showings are Special Events." });
                    }
                }
            }

            if (order.GiftOrder == true)
            {
                if (order.GiftEmail == null)
                {
                    return View("Error", new String[] { "If you want to checkout as a gift, make sure to enter the Gift Recipient's email!" });
                }

                string roleID = _context.Roles.FirstOrDefault(r => r.Name == "Customer").Id;
                AppUser gifteduser = _context.Users.Where(u => u.Email == order.GiftEmail).FirstOrDefault();

                bool adultfilm = false;

                foreach (Ticket t in currorder.Tickets)
                {
                    if (t.Showing.Movie.MPAA == MPAA.R || t.Showing.Movie.MPAA == MPAA.NC17)
                    {
                        adultfilm = true;
                        break;
                    }
                }

                if (order.GiftOrder == true && gifteduser == null)
                {
                    return View("Error", new String[] { "If you want to checkout as a gift, make sure that your friend has an account with us!" });
                }

                string userroleid = _context.UserRoles.Where(ur => ur.UserId == gifteduser.Id).FirstOrDefault().RoleId;
                if (order.GiftOrder == true && userroleid != roleID)
                {
                    return View("Error", new String[] { "If you want to checkout as a gift, make sure that your friend has a customer account with us!" });
                }

                if (((DateTime.Now - gifteduser.Birthdate).TotalDays < 6570) && adultfilm == true)
                {
                    return View("Error", new String[] { "You cannot purchase a gift order with R-rated or NC-17-rated showings for a customer that is younger than 18." });
                }
            }


            currorder.GiftEmail = order.GiftEmail;
            currorder.GiftOrder = order.GiftOrder;


            if ((DateTime.Now.Date - currorder.Customer.Birthdate).TotalDays >= 21900)
            {
                // we will use viewbags rn because they may want to make changes still; not yet confirmed. we will officially change the values when they do confirm
                decimal discount = currorder.Discount.PriceValue;
                ViewBag.Discount = string.Format("{0:C}", discount);
                List<Ticket> nonspecialtickets = currorder.Tickets.Where(t => t.Showing.SpecialEvent == false).ToList();

                if (nonspecialtickets.Count() >= 2)
                {
                    ViewBag.DiscountDouble = string.Format("{0:C}", discount * 2);
                    ViewBag.DiscountSubtotal = string.Format("{0:C}", currorder.OrderSubtotal + (discount * 2));
                    ViewBag.DiscountTax = string.Format("{0:C}", Math.Round(((currorder.OrderSubtotal + (discount * 2)) * .0875m), 2));
                    ViewBag.DiscountDiff = string.Format("{0:C}", currorder.OrderSubtotal + discount * 2);
                    ViewBag.DiscountTotal = string.Format("{0:C}", Math.Round(((currorder.OrderSubtotal + (discount * 2)) * (1 + .0875m)), 2));
                }
                else
                {
                    ViewBag.DiscountSubtotal = string.Format("{0:C}", currorder.OrderSubtotal + (discount));
                    ViewBag.DiscountTax = string.Format("{0:C}", Math.Round(((currorder.OrderSubtotal + (discount)) * .0875m), 2));
                    ViewBag.DiscountDiff = string.Format("{0:C}", currorder.OrderSubtotal + discount);
                    ViewBag.DiscountTotal = string.Format("{0:C}", Math.Round(((currorder.OrderSubtotal + (discount)) * (1 + .0875m)), 2));
                }
            }
            
            if ((order.PopcornPointsUsed == true && currorder.Tickets.Count() <= ((currorder.Customer.PopcornPoints) / 100)) || (order.PopcornPointsUsed == false))
            {
                currorder.PopcornPointsUsed = order.PopcornPointsUsed;
            }

            else
            {
                return View("Error", new String[] { "You do not have enough Popcorn Points to use for this order; you have " + currorder.Customer.PopcornPoints + " but you need " +
                    (currorder.Tickets.Count() * 100) + " to cover this order." });
            }

            _context.Orders.Update(currorder);
            _context.SaveChanges();

            return View(currorder);
        }
        [HttpGet]
        public IActionResult CancelOrderView(int id)
        {
            Order order = _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price)
                                        .Where(o => o.OrderID == id)
                                        .FirstOrDefault();
           
            if (order.OrderHistory == OrderHistory.Cancelled)
            {
                return View("Error", new String[] { "This order has already been cancelled!" });
            }
            if (order.OrderHistory == OrderHistory.Future)
            {
                return View("Error", new String[] { "This order has not been purchased yet!" });
            }
            return View("CancelOrder", order);
        }
        //Cancel
        [HttpPost]
        public IActionResult CancelOrder(Order order)
        {
            Order o = _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price)
                                        .Where(o => o.OrderID == order.OrderID)
                                        .FirstOrDefault();

            foreach (Ticket t in o.Tickets)
            {
                if ((t.Showing.StartDateTime - DateTime.Now).TotalMinutes < 0)
                {
                    return View("Error", new String[] { "You cannot cancel this order because one or more of the tickets are for a showing has already started." });
                }
                if ((t.Showing.StartDateTime - DateTime.Now).TotalMinutes <= 60)
                {
                    return View("Error", new String[] { "You cannot cancel this order because one or more of the tickets are for a showing that will start within the hour." });
                }
            }

            o.OrderHistory = OrderHistory.Cancelled;

            int? add = null;
            if (o.PopcornPointsUsed == true)
            {
                add = (o.Tickets.Count() * 100);
                o.Customer.PopcornPoints += (int)add;
            }

            if (o.PopcornPointsUsed == false)
            {
                add = (int)Decimal.Truncate(o.PostDiscount);
                o.Customer.PopcornPoints -= (int)add;
            }

            EmailMessaging.SendEmail(o.Customer.Email, "Order Cancellation Confirmation", "We have confirmed that you have cancelled: " + o.OrderNumber + " You should recieve your refund promptly. We hope to see you again soon!");
            _context.Orders.Update(o);
            _context.Users.Update(o.Customer);
            _context.SaveChanges();
            return RedirectToAction("CancelSuccess", new { id = o.OrderID, pcp = add });
        }

        public IActionResult CancelSuccess(int id, int? pcp)
        {
            Order o = _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price)
                                        .Where(o => o.OrderID == id)
                                        .FirstOrDefault();
            ViewBag.Add = pcp;
            return View(o);
        }
        public IActionResult Confirmation([Bind("OrderID, Tax, OrderTotal, PopcornPointsUsed, OrderNumber")] Order order)
        {
            Order pastorder = _context.Orders.Include(o => o.Tickets).ThenInclude(o => o.Showing)
            .ThenInclude(o => o.Movie).Include(o => o.Tickets).ThenInclude(o => o.Showing)
            .ThenInclude(o => o.Price).Include(o => o.Customer).FirstOrDefault(o => o.OrderID == order.OrderID);
            pastorder.OrderHistory = OrderHistory.Past;
            ViewBag.Earned = 0;

            // if they used PC points
            if (order.PopcornPointsUsed == true)
            {
                int points = pastorder.Tickets.Count();
                pastorder.Customer.PopcornPoints -= points * 100;
            }

            // if they didn't use PC points
            else
            {
                int points = (int)Decimal.Truncate(pastorder.PostDiscount);
                ViewBag.Earned = points;
                pastorder.Customer.PopcornPoints += points;
            }

            _context.Orders.Update(pastorder);
            _context.SaveChanges();

            if (pastorder.GiftEmail != null)
            {
                EmailMessaging.SendEmail(pastorder.Customer.Email, "Gift Order Purchase Confirmation", "We confirmed you just placed an order for your friend at " + pastorder.GiftEmail + "! You order number is: " + order.OrderNumber);
                EmailMessaging.SendEmail(pastorder.GiftEmail, "Gift Order Purchase Confirmation", "Your friend " + pastorder.Customer.FirstName + " " + pastorder.Customer.LastName + " " + "bought you some tickets! You order number is: " + order.OrderNumber);
            }

            else
            {
                EmailMessaging.SendEmail(pastorder.Customer.Email, "Order Purchase Confirmation", "We confirmed you just placed an order! You order number is: " + order.OrderNumber);
            }
            
            return View(pastorder);
        }

        // Gift Order
        public IActionResult GiftIndex()
        {
            if (User.IsInRole("Customer"))
            {
                List<Order> orders = _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price).Where(o => o.GiftEmail == User.Identity.Name).ToList();
                return View(orders);
            }
            else
            {
                List<Order> orders = _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Discount)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Movie)
                                        .Include(o => o.Tickets)
                                        .ThenInclude(t => t.Showing)
                                        .ThenInclude(s => s.Price).Where(o => o.GiftOrder == true).ToList();
                return View(orders);
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
        
