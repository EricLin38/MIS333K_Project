using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Team1_FinalProject.Models
{
	public enum MRStatus { WIP, Accepted, Rejected }
	public class MovieReview
	{
		public Int32 MovieReviewID { get; set; }

		[Required(ErrorMessage = "Please select a rating from 1-5")]
		[Range(1, 5, ErrorMessage = "Rating must be between 1-5.")]
		[Display(Name = "Rating")]
		public Int32 MovieRating { get; set; }

		[Display(Name = "Review")]
		[StringLength(280, ErrorMessage = "Character limit is 280 characters.")]
		public string ReviewDescription { get; set; }

		public AppUser User { get; set; }

		public Movie Movie { get; set; }
		public MRStatus Status { get; set; }

       
    }
}
