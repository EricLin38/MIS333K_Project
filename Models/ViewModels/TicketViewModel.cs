using System;
using System.ComponentModel.DataAnnotations;

namespace Team1_FinalProject.Models

{
	public class TicketViewModel
	{
		public int SelectSeatID { get; set; }
        public string SelectSeatNumber {get; set;}
		public int SelectShowingID { get; set; }
		public int? SelectOrderID { get; set; }
		public int[] SelectedSeats { get; set; }
	}
}
