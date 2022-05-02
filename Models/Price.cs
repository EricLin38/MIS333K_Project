using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
namespace Team1_FinalProject.Models
{
    public enum PType
    {
        WeekdayMorning,
        Tuesday,
        WeekdayAfternoon,
        Weekend,
        SeniorCitizen
    }
    public class Price
    {
        public Int32 PriceID { get; set; }

        public PType PriceType { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PriceValue { get; set; }
        public List<Showing> Showings { get; set; }

    }
}
