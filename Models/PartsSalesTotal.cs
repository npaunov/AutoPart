using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartApp.Models
{
    public class PartsSalesTotal
    {
        [Key, ForeignKey("Part")]
        [Required]
        public string Id { get; set; } = string.Empty; // Same as Part.Id

        [Required]
        public int TotalSales { get; set; }

        // Navigation property
        public Part Part { get; set; }
    }
}