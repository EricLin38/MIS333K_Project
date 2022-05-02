using System;
using System.ComponentModel.DataAnnotations;

namespace Team1_FinalProject.Models

{
	public enum RatingsRange
	{
		Greater,
		Lesser
	}

	public enum Select
    {
		Movie,
		Showing
    }

    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute() : base(typeof(DateTime), DateTime.Now.Date.ToString(), DateTime.Now.AddYears(20).Date.ToString())
        { }
    }


	public class SearchViewModel
	{
		[Display(Name = "Title:")]
		public string SearchTitle { get; set; }
		[Display(Name = "Tagline:")]
		public string SearchTagline { get; set; }
		[Display(Name = "Genre:")]
		public Int32 SelectGenreID { get; set; }
		[Display(Name = "Showing Date:")]
		[CustomDateRange(ErrorMessage = "Please select a date that has not passed already.")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		[DataType(DataType.Date)]
		public DateTime? SearchShowingDateStart { get; set; }
		[Display(Name = "to:")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		[DataType(DataType.Date)]
		public DateTime? SearchShowingDateEnd { get; set; }
		[Display(Name = "Showing Time:")]
		[DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
		[DataType(DataType.Time)]
		public TimeSpan? SearchShowingTimeStart { get; set; }
		[Display(Name = "to:")]
		[DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
		[DataType(DataType.Time)]
		public TimeSpan? SearchShowingTimeEnd { get; set; }
		[Display(Name = "Release Year:")]
		public int? SearchReleaseDateStart { get; set; }
		[Display(Name = "to:")]
		public int? SearchReleaseDateEnd { get; set; }
		[Display(Name = "MPAA Rating:")]
		public MPAA? SelectMPAA { get; set; } 
		[Display(Name = "Actors:")]
		public String SearchActor { get; set; }
		[Display(Name = "Customer Rating:")]
		[Range(1, 5, ErrorMessage = "Rating must be between 1-5.")]
		public Decimal? SearchRating { get; set; }
		[Display(Name = "")]
		public RatingsRange? RatingsRange { get; set; }
		[Display(Name = "Search Options:")]
		public Select MovieShowing { get; set; }
		[Display(Name = "Select Movies Shown This Week:")]
		public int[] SelectMovieID { get; set; }

	}
}
