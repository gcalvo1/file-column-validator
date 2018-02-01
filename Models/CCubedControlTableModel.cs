using System.ComponentModel.DataAnnotations;

namespace CCubed_2012.Models
{
    public class CCubedControlTableModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string CheckType { get; set; }

        [Required]
        public string Client { get; set; }

        [Required]
        public string Project { get; set; }

        [Required]
        public string TemplateFilePath { get; set; }

        [Required]
        public string RawFilePath { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public System.DateTime CreatedDate { get; set; }

        [Required]
        public string ColumnDelimiter { get; set; }

        [Required]
        public string Extension { get; set; }
    }
}
