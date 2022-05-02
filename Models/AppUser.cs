using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;


namespace Team1_FinalProject.Models
{
    public enum EmploymentStatus
    {
        Hired,
        Fired
    }
    public class AppUser : IdentityUser
    {

        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        
        public String MiddleInitial { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        public Int32 PopcornPoints { get; set; }
        //1 popcorn point for every $ spent on a movie
        //100 popcorn points = one movie
        //special events not eligible
        //canceled orders return PCPoints


        public EmploymentStatus EStatus { get; set; }

        public String Address { get; set; }

        public List<Order> Orders { get; set; }

        public List<MovieReview> MovieReviews { get; set; }

        public AppUser()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }
        }
    }
}
