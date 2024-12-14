namespace Domain.Events
{
    public class OrderPlacedEvent
    {
        public Guid OrderId { get; set; }
        public Guid TraderId { get; set; }
        public string StockSymbol { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string OrderType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public OrderPlacedEvent(Guid orderId, Guid traderId, string stockSymbol, int quantity, decimal price, string orderType, DateTime createdAt)
        {
            OrderId = orderId;
            TraderId = traderId;
            StockSymbol = stockSymbol;
            Quantity = quantity;
            Price = price;
            OrderType = orderType;
            CreatedAt = createdAt;
        }
    }
}
