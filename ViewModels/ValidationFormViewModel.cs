using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCubed_2012.ViewModels
{
    public class ValidationFormViewModel
    {
        [Display(Name = "Client")]
        [Required]
        public string ClientId { get; set; }

        [Display(Name = "Project")]
        [Required]
        public string ProjectId { get; set; }

        [Display(Name = "Check Type")]
        [Required]
        public string CheckTypeId { get; set; }

        public List<string> Client { get; set; }

        public List<string> Project { get; set; }

        public List<string> CheckType { get; set; }

    }
}
