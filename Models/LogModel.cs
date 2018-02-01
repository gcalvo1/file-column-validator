using System;
using System.ComponentModel.DataAnnotations;

namespace CCubed_2012.Models
{
    public class LogModel
    {
        public int Id { get; set; }

        [Required]
        public string Client { get; set; }

        [Required]
        public string Project { get; set; }

        [Required]
        public string CheckType { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public bool IsValidated { get; set; }

        public string DiscrepancyColumns { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

    }
}