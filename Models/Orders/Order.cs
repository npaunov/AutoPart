using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartApp.Models
{
    /// <summary>
    /// Represents an order placed for parts, including status, totals, and audit fields.
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        /// <summary>
        /// The date and time when the order was created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The total sum of the order in BGN.
        /// </summary>
        public decimal TotalSumBGN { get; set; }

        /// <summary>
        /// The total sum of the order in Euro.
        /// </summary>
        public decimal TotalSumEuro { get; set; }

        /// <summary>
        /// The current status of the order.
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// The date and time when the order was created (audit).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the order was last modified (audit).
        /// </summary>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// The collection of items included in this order.
        /// </summary>
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
