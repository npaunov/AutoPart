using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartApp.Models
{
    public class PartSale
    {
        [Key]
        public int SaleId { get; set; } // Unique for each sale

        [Required]
        [ForeignKey("Part")]
        public string PartId { get; set; } = string.Empty;

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation property
        public Part Part { get; set; }
    }
}