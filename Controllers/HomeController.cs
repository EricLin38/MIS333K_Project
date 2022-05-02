using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Team1_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Showing> showingstoday = _context.Showings.Include(s => s.Movie).Include(s => s.Tickets).ThenInclude(t => t.Order).Where(s => s.Status == SStatus.Published).Where(s => s.StartDateTime.Date == DateTime.Now.Date).ToList();
            return View(showingstoday);
        }
        // GET: Home/SearchSelect
        public IActionResult SearchSelect()
        {
            SearchViewModel svm = new SearchViewModel();
            return View("SearchSelect", svm);
        }

        // kinda like the POST method for SearchSelect
        // GET: Home/SearchMoviesShowings
        public IActionResult SearchMoviesShowings(SearchViewModel svm)
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllAvailableMovies = GetAllAvailableMovies();
            svm.SelectGenreID = 0;
            if (svm.MovieShowing == Select.Movie)
            {
                return View("SearchMovies", svm);
            }
            else
            {
                return View("SearchShowings", svm);
            }
        }

        private SelectList GetAllGenres()
        {
            List<Genre> genreList = _context.Genres.ToList();

            Genre SelectNone = new Genre() { GenreID = 0, GenreName = "All Genres" };
            genreList.Add(SelectNone);

            SelectList genreSelectList = new SelectList(genreList.OrderBy(m => m.GenreID), "GenreID", "GenreName");

            return genreSelectList;
        }

        private MultiSelectList GetAllAvailableMovies()
        {
            List<Movie> moviesList = _context.Movies.Where(m => m.Showings.Count() != 0).ToList();
            List<Showing> allShowings = _context.Showings.ToList();
            foreach (Showing showing in allShowings)
            {
                // if a showing is older than today
                if (showing.StartDateTime < DateTime.Now)
                {
                    // first see if the showing's movie is still in the list; if not, go to next showing
                    if (moviesList.Contains(showing.Movie) == false)
                    {
                        continue;
                    }

                    bool nocurrent = true;
                    // check to see if that showing's movie has any current showings, meaning its start date is >= today
                    foreach (Showing mshowing in showing.Movie.Showings)
                    {
                        if (mshowing.StartDateTime >= DateTime.Now)
                        {
                            nocurrent = false;
                            break;
                        }
                    }
                    if (nocurrent == true)
                    {
                        moviesList.Remove(showing.Movie);
                    }
                }
            }

            MultiSelectList movieSelectList = new MultiSelectList(moviesList.OrderBy(m => m.MovieID), "MovieID", "Title");

            return movieSelectList;

        }

        //[HttpPost]
        //public ActionResult DisplayReportsResults(ReportViewModel rvm) { }
    }
}
