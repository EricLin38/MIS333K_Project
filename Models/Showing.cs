using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Team1_FinalProject.Models
{

    // in the controller, we will need to add spaces like below:
    // switch (employee):
    // {
    //   case 1: return "Theater 1";
    //   // same for the rest
    // }
    public enum SStatus
    {
        Pending,
        Published
    }

    public class Showing
    {
        public Int32 ShowingID { get; set; }
        [Required(ErrorMessage = "This is required!")]
        [Display(Name = "Starts At")]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "Ends At")]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime EndDateTime { get; set; }
        [Required(ErrorMessage = "This is required!")]
        [Display(Name = "Theater Room")]
        [Range(1, 2)]
        public int Room { get; set; }
        [Required(ErrorMessage = "This is required!")]
        [Display(Name = "Is This a Special Event?")]
        public Boolean SpecialEvent { get; set; }

        public Movie Movie { get; set; }

        public List<Ticket> Tickets { get; set; }
        [Display(Name = "Price")]
        public Price Price { get; set; }
        public SStatus Status { get; set; }
        [Display(Name = "Tickets Available")]
        public int TicketCount
        {
            get
            {
                List<Ticket> tickets = Tickets.Where(t => t.Order.OrderHistory != OrderHistory.Cancelled).ToList();
                return (20 - tickets.Count());
            }
        }

        public Showing()
        {
            if (Tickets == null)
            {
                Tickets = new List<Ticket>();
            }
        }
        
    }
}
