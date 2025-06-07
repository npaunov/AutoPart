namespace AutoPart.Models.Orders
{
    /// <summary>
    /// Represents the status of an order or order item.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// The order or item is pending and not yet processed.
        /// </summary>
        Pending,

        /// <summary>
        /// The order or item has been sent/ordered but not yet received.
        /// </summary>
        Ordered,

        /// <summary>
        /// The order or item has been received but not yet completed.
        /// </summary>
        Received,

        /// <summary>
        /// The order or item is fully completed and closed.
        /// </summary>
        Completed,

        /// <summary>
        /// The order item has been partially received (not all quantities delivered).
        /// </summary>
        PartiallyReceived
    }
}