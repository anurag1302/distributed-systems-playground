using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("payments")]
public class PaymentsController:ControllerBase
{
    private static readonly Random random = new Random();

    [HttpPost()]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPayment model)
    {
        var randomNumber = random.Next(1, 10);
        
        //Simulate a faulty payment system, at times it has some delay, at times it fails
        if (randomNumber <= 5)
        {
            await Task.Delay(3000);
        }

        if (randomNumber <= 3)
        {
            return StatusCode(500, "Payment Gateway Failure");
        }

        return Ok(new
        {
            Id = Guid.NewGuid(),
            Message = "Payment Successful"
        });
    }
}