using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;

namespace Team1_FinalProject.Models
{
	public class Ticket
	{
		private decimal fixPrice = 0;
		public Int32 TicketID { get; set; }

		[Display(Name = "Seat Number:")]
		public String SeatNumber { get; set; }
		
		public Boolean SeatClaim { get; set; }

		[DisplayFormat(DataFormatString = "{0:C}")]
		public decimal FixPrice
		{
			get
            {
				return fixPrice;
            }
			set
            {
				fixPrice = Showing.Price.PriceValue;
            }
		}

		public Order Order { get; set; }

		public Showing Showing { get; set; }

	}
    
}
