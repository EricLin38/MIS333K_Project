using Team1_FinalProject.DAL;
using Team1_FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Team1_FinalProject.Seeding
{
	public static class SeedGenres
	{
		public static void AddGenre(AppDbContext db)
		{
		    //Create a new list for all of the Genres
		List<Genre> AllGenres = new List<Genre>();
		AllGenres.Add(new Genre
		{
			GenreName = "Action",
		});
		
		AllGenres.Add(new Genre
		{
			GenreName = "Adventure",
		});
		
		AllGenres.Add(new Genre
		{
			GenreName = "Animation",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Comedy",
		});
		
		AllGenres.Add(new Genre
		{
			GenreName = "Crime",
		});	
				
		AllGenres.Add(new Genre
		{
			GenreName = "Drama",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Family",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Fantasy",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Horror",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Musical",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Romance",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Science Fiction",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "Thriller",
		});
				
		AllGenres.Add(new Genre
		{
			GenreName = "War",
		});

		AllGenres.Add(new Genre
		{
			GenreName = "Western",
		});
				//create some counters to help debug problems
		Int32 intGenreID = 0;
		String strGenreTitle = "Start";

		//loop through the list to add or update the genre
		try
		{
			foreach (Genre seedGenre in AllGenres)
			{
				//update the counters
				intGenreID = seedGenre.GenreID;
				strGenreTitle = seedGenre.GenreName;
				//see if the genre is already in the database using the UniqueIdentifier
				Genre dbGenre = db.Genres.FirstOrDefault(m => m.GenreID == seedGenre.GenreID);

					//if genre is null, genre is not in database
					if (dbGenre == null)
					{
						//Add the genre to the database
						db.Genres.Add(seedGenre);
						db.SaveChanges();
					}
					else //the genre is in the database - reset all fields except ID and Unique Identifier
					{
						dbGenre.GenreName = seedGenre.GenreName;
						db.SaveChanges();
					}
				}
			}
			catch (Exception ex) //throw error if there is a problem in the database
			{
				StringBuilder msg = new StringBuilder();
				msg.Append("There was a problem adding the genre with the title: ");
				msg.Append(strGenreTitle);
				msg.Append(" (GenreID: ");
				msg.Append(intGenreID);
				msg.Append(")");
				throw new Exception(msg.ToString(), ex);
			}
		}
	}
}
