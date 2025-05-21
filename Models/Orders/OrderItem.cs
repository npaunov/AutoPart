using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartApp.Models
{
    /// <summary>
    /// Represents a single part item within an order, including quantities, prices, and status.
    /// </summary>
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        /// <summary>
        /// The foreign key to the parent order.
        /// </summary>
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        /// <summary>
        /// Navigation property to the parent order.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// The foreign key to the part being ordered.
        /// </summary>
        [ForeignKey("Part")]
        public string PartId { get; set; }

        /// <summary>
        /// Navigation property to the part.
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// The description of the part at the time of ordering.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The package size for this part in the order.
        /// </summary>
        public int Package { get; set; }

        /// <summary>
        /// The price per unit in BGN at the time of ordering.
        /// </summary>
        public decimal PriceBGN { get; set; }

        /// <summary>
        /// The price per unit in Euro at the time of ordering.
        /// </summary>
        public decimal PriceEuro { get; set; }

        /// <summary>
        /// The quantity of this part ordered.
        /// </summary>
        public int QuantityOrdered { get; set; }

        /// <summary>
        /// The quantity of this part actually received.
        /// </summary>
        public int QuantityReceived { get; set; }

        /// <summary>
        /// The subtotal for this item in BGN (PriceBGN * QuantityOrdered).
        /// </summary>
        public decimal SubtotalBGN { get; set; }

        /// <summary>
        /// The subtotal for this item in Euro (PriceEuro * QuantityOrdered).
        /// </summary>
        public decimal SubtotalEuro { get; set; }

        /// <summary>
        /// The current status of this order item.
        /// </summary>
        public OrderStatus Status { get; set; }
    }
}

