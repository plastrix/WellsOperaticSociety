using System;
using Microsoft.Build.Framework;

namespace WellsOperaticSociety.Models.StandardModels
{
    public class BoxOfficeTime
    {
        public int BoxOfficeTimeId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Required]
        public DateTime Date { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Required]
        public TimeSpan OpeningTime { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Required]
        public TimeSpan ClosingTime { get; set; }

        public string Message { get; set; }
    }
}
