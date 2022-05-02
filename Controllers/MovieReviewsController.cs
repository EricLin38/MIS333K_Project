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
    [Authorize]
    public class MovieReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public MovieReviewsController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MovieReviews
        public IActionResult Index()
        {
            List<MovieReview> movieReviews;
            if (User.IsInRole("Employee") || User.IsInRole("Manager"))
            {
                movieReviews = _context.MovieReviews
                                .Include(o => o.Movie)
                                .OrderBy(o => o.Status)
                                .ToList();
            }
            else //user is a customer, so only display their records
            {
                movieReviews = _context.MovieReviews
                                .Include(o => o.Movie)
                                .Where(o => o.User.UserName == User.Identity.Name)
                                .ToList();
            }

            //
            return View(movieReviews);
        }

        public IActionResult ReviewsIndex(int? id)
        {
            List<MovieReview> reviews = _context.MovieReviews.Where(mr => mr.Movie.MovieID == id).Where(mr => mr.Status == MRStatus.Accepted).ToList();
            ViewBag.MovieTitle = _context.Movies.Find(id).Title;
            return View(reviews);
        }

        // GET: MovieReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "You must specify what movie review to view!" });
            }

            MovieReview movieReview = await _context.MovieReviews.Include(mr => mr.Movie)
                                            .FirstOrDefaultAsync(m => m.MovieReviewID == id);
            if (movieReview == null)
            {
                return View("Error", new String[] { "No movie review exists for this ID yet!" });
            }

           
            return View(movieReview);
        }

        // GET: MovieReviews/Create
        public IActionResult Create()
        {
            ViewBag.AllMovies = GetWatchedMovies();
            return View();
        }

        // POST: MovieReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieReviewID,MovieRating,ReviewDescription,SelectedMovieID")] MovieReview movieReview, int SelectedMovieID)
        {
            List<Order> pastorders = _context.Orders.Include(o => o.Tickets).ThenInclude(o => o.Showing).ThenInclude(o => o.Movie).ToList();
            bool check = false;
            foreach (Order order in pastorders)
            {
                foreach (Ticket ticket in order.Tickets)
                {
                    if (ticket.Showing.Movie.MovieID == SelectedMovieID && ticket.Showing.EndDateTime < DateTime.Now)
                    {
                        check = true;
                        break;
                    }
                }
                if (check == true)
                {
                    break;
                }
            }


            if (check == false)
            {
                return View("Error", new String[] { "You have not watched this movie yet, or have not watched this movie with us; please do that first!" });
            }

            List<MovieReview> userreviews;
            userreviews = _context.MovieReviews.Include(mr => mr.Movie).Where(m => m.User.UserName == User.Identity.Name).ToList();
            if (userreviews.Count() != 0)
            {
                foreach (MovieReview review in userreviews)
                {
                    if (review.Movie.MovieID == SelectedMovieID)
                    {
                        return View("Error", new String[] { "You have already created a review for this movie!!" });
                    }
                }
            }

            if (ModelState.IsValid == false)
            {
                return View(movieReview);
            }

            Movie dbMovie = _context.Movies.Find(SelectedMovieID);

            movieReview.Movie = dbMovie;
            movieReview.Status = MRStatus.WIP;
            movieReview.User = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            _context.Add(movieReview);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "MovieReviews", new { id = movieReview.MovieReviewID });
        }

        // GET: MovieReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "Please specify a movie review to edit!" });
            }

            //find the order detail
            MovieReview movieReview = await _context.MovieReviews
                                                   .Include(mr => mr.Movie)
                                                   .FirstOrDefaultAsync(mr => mr.MovieReviewID == id);
            if (movieReview == null)
            {
                return View("Error", new String[] { "This movie review was not found. Try creating a review instead!" });
            }
            
            return View(movieReview);
        }

        // POST: MovieReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieReviewID,MovieRating,ReviewDescription,Status")] MovieReview movieReview)
        {
            if (id != movieReview.MovieReviewID)
            {
                return View("Error", new String[] { "There was a problem editing this record. Try again!" });
            }

            //information is not valid, try again
            if (ModelState.IsValid == false)
            {
                return View(movieReview);
            }

            //create a new order detail
            MovieReview dbMR;
            //if code gets this far, update the record
            try
            {
                //find the existing movie review in the database
                //include movie
                dbMR = _context.MovieReviews
                      .Include(rd => rd.Movie)
                      .FirstOrDefault(rd => rd.MovieReviewID == movieReview.MovieReviewID);

                //update the scalar properties

                dbMR.MovieRating = movieReview.MovieRating;
                dbMR.ReviewDescription = movieReview.ReviewDescription;
                dbMR.Status = movieReview.Status;

                //save changes
                _context.Update(dbMR);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return View("Error", new String[] { "There was a problem editing this record", ex.Message });
            }

            //if code gets this far, go back to the order details index page
            return RedirectToAction("Details", "MovieReviews", new { id = dbMR.MovieReviewID });
        }

        // GET: MovieReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReviews
                .FirstOrDefaultAsync(m => m.MovieReviewID == id);
            if (movieReview == null)
            {
                return View("Error", new String[] { "This movie review was not found. Try creating a review instead!" });
            }
            

            return View(movieReview);
        }

        // POST: MovieReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieReview = await _context.MovieReviews.FindAsync(id);
            _context.MovieReviews.Remove(movieReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieReviewExists(int id)
        {
            return _context.MovieReviews.Any(e => e.MovieReviewID == id);
        }
        public SelectList GetWatchedMovies()
        {
            List<Order> pastorders = _context.Orders.Include(o => o.Tickets).ThenInclude(o => o.Showing).ThenInclude(o => o.Movie).Where(o => o.Customer.UserName == User.Identity.Name).Where(o => o.OrderHistory == OrderHistory.Past).ToList();
            List<Movie> watchedmovies = new List<Movie>();
            foreach (Order order in pastorders)
            {
                foreach (Ticket ticket in order.Tickets)
                {
                    if (watchedmovies.Contains(ticket.Showing.Movie) == false && ticket.Showing.EndDateTime < DateTime.Now)
                    {
                        watchedmovies.Add(ticket.Showing.Movie);
                    }
                }
            }

            SelectList movieSelectList = new SelectList(watchedmovies.OrderBy(m => m.MovieID), "MovieID", "Title");

            return movieSelectList;
        }
    }
}
