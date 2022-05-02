using Team1_FinalProject.DAL;
using System;
using System.Linq;

namespace Team1_FinalProject.Utilities
{
    public static class GenerateMovieNumber
    {
        public static Int32 GetNextMovieNumber(AppDbContext _context)
        {
            //Set a number where the course numbers should start
            const Int32 START_NUMBER = 3000;

            Int32 intMaxMovieNumber; //the current maximum course number
            Int32 intNextMovieNumber; //the course number for the next class

            if (_context.Movies.Count() == 0) //there are no courses in the database yet
            {
                intMaxMovieNumber = START_NUMBER; //course numbers start at 3001
            }
            else
            {
                intMaxMovieNumber = _context.Movies.Max(c => c.MovieNumber); //this is the highest number in the database right now
            }

            //You added courses before you realized that you needed this code
            //and now you have some course numbers less than 3000
            if (intMaxMovieNumber < START_NUMBER)
            {
                intMaxMovieNumber = START_NUMBER;
            }

            //add one to the current max to find the next one
            intNextMovieNumber = intMaxMovieNumber + 1;

            //return the value
            return intNextMovieNumber;
        }

    }
}