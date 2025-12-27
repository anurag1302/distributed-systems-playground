namespace OrderService.Models;

public class CreateOrderRequest
{
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
}