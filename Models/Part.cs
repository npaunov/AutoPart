using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPart.Models
{
    public class Part
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal PriceBGN { get; set; }

        [NotMapped]
        public decimal PriceEURO => PriceBGN / 1.95583m; // Example: BGN to EUR fixed rate

        public int Package { get; set; }
        public int InStore { get; set; }

        // Navigation property for one-to-many relationship
        public ICollection<PartSale> Sales { get; set; } = new List<PartSale>();
    }
}