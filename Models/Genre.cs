using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Team1_FinalProject.Models
{
    public class Genre
    {
        public Int32 GenreID { get; set; }
        [Display(Name = "Genre")]
        public String GenreName { get; set; }

        public List<Movie> Movies { get; set; }

        public Genre()
        {
            if (Movies == null)
            {
                Movies = new List<Movie>();
            }
        }

    }
}