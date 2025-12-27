using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController:ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    
    public OrdersController(ILogger<OrdersController> logger, 
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody]CreateOrderRequest request)
    {
        var paymentsURL = _configuration.GetSection("PaymentsURL").Value;
        var httpClient = _httpClientFactory.CreateClient();

        var paymentRequest = new { Id = Guid.NewGuid(), Amount = request.ProductPrice };
        var response = await httpClient.PostAsJsonAsync(paymentsURL, request);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(500, "Order Failed due to payment failure");
        }
        return Ok(new { Id = paymentRequest.Id, Message="Order Successful" });
    }
}