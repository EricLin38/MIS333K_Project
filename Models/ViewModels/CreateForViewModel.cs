using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Team1_FinalProject.Models
{
    public class CreateForViewModel
    {
        [Required]
        [Display(Name = "Available Customers")]
        public int SelectCustomerID { get; set; }
        public string SelectCustomerName { get; set; }
        public int SelectedCustomerID { get; set; }
        public int SelectShowingID { get; set; }
    }
}
