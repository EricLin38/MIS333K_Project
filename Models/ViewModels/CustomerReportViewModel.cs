using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Team1_FinalProject.Models
{
    public class CustomerReportViewModel
    {
        [Display(Name = "Search Individual Customer:")]
        public int SelectCustomerID { get; set; }
        public string SelectCustomerName { get; set; }
        public int SelectedCustomerID { get; set; }
        public int SeatsSold { get; set; }
        public int PopcornPointsUsed { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalRevenue { get; set; }
    }
}
