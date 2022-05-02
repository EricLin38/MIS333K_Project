using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Team1_FinalProject.Models
{
	public enum MPAA
	{
		G,
		PG,
		PG13,
		R,
		Unrated,
		NC17
	}

	public class Movie
	{

		public Int32 MovieID { get; set; }
		[Display(Name = "Movie #")]
		public Int32 MovieNumber { get; set; }
		[Display(Name = "Movie Title")]
		[Required(ErrorMessage ="This field is required")]
		public string Title { get; set; }
		[Required(ErrorMessage = "This field is required")]
		public Int32 Runtime { get; set; }
		[Required(ErrorMessage = "This field is required")]
		public string Description { get; set; }
		[Display(Name = "MPAA Rating")]
		[Required(ErrorMessage = "This field is required")]
		public MPAA? MPAA { get; set; }

		[Display(Name = "Release Date")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyy}")]
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "This field is required")]
		public DateTime ReleaseDate { get; set; }
		[Required(ErrorMessage = "This field is required")]
		public string Actors { get; set; }

		public string Tagline { get; set; }

		public Int32 Revenue { get; set; }
		//apparently on the seeded data there is a column for this so i think
		// we need a property so we don't run into errors but we can hide it later
		// possibly thru the view
		public Genre Genre { get; set; }

		public List<Showing> Showings { get; set; }
		public List<MovieReview> Reviews { get; set; }
		[Display(Name = "Movie Rating")]
		[DisplayFormat(DataFormatString = "{0:#.#}")]
		public decimal? AverageRating
		{
			get
			{
				List<int> ratings = new List<int>();
				if (Reviews is null)
				{
					return null;
				}

				else
				{
					foreach (MovieReview review in Reviews)
					{
						if (review.Status == MRStatus.Accepted)
						{
							ratings.Add(review.MovieRating);
						}
					}

					if (ratings.Count() == 0)
                    {
						return null;
                    }

					return (decimal)ratings.Average();
				}
			}
		}
	}
}
