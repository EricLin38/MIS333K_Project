using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Team1_FinalProject.Models
{
    public class ReportViewModel
    {
        public enum decision
        {
            SeatSold, TotalRevenue, Both
        }
        public ReportViewModel()
        {
        }
        [Display(Name ="Report Type:")]
        [Required]
        public decision Decision { get; set; }
        public int SeatsSold { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalRevenue { get; set; }
        [Display(Name ="Search orders purchased with Popcorn Points:")]
        public bool PopcornPoints { get; set; }
        public Movie Movie { get; set; }
        [Display(Name = "Search by MPAA Rating:")]
        public MPAA? SelectedMPAA { get; set; }

        [Display(Name = "Available Customers")]
        public int SelectCustomerID { get; set; }
        public string SelectCustomerName { get; set; }
        public int SelectedCustomerID { get; set; }

        [Display(Name = "Search Movie Title:")]
        public string MovieTitle { get; set; }
        [Display(Name = "Showing Date:")]
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
        


        //public List<Showing> Showings { get; set; }
        //public List<Ticket> Tickets { get; set; }
        //public List<Order> Orders { get; set; }

    }
}
