using Microsoft.AspNetCore.Mvc;

namespace APIForBuildInMCPServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<OrderRecord> Orders =
    [
        new("George Washington", [new("Apple", 2), new("Peach", 1), new("Cherry", 6)]),
        new("Ghengis Khan", [new("Pineapple", 3), new("Peach", 2), new("Orange", 4)]),
        new("William Shakespeare", [new("Strawberry", 5), new("Fig", 2), new("Apple", 1)]),
        new("Captain Ahab", [new("Pineapple", 2), new("Kiwi", 7), new("Banana", 3)]),
        new("King Kong", [new("Peach", 4), new("Banana", 8), new("Mangoes in syrup", 2)]),
        new("Cleopatra", [new("Grape", 9), new("Pomegranate", 1), new("Fig", 3)])
    ];

    [HttpGet("items-by-customer/{customerName}")]
    public IActionResult GetItemsByCustomer(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            return BadRequest(new { message = "A customer name is required." });
        }

        var order = Orders.FirstOrDefault(o =>
            string.Equals(o.CustomerName, customerName, StringComparison.OrdinalIgnoreCase));

        if (order is null)
        {
            return NotFound(new { message = $"No order was found for customer '{customerName}'." });
        }

        return Ok(new
        {
            customerName = order.CustomerName,
            items = order.Items.Select(i => new { name = i.Name, quantity = i.Quantity })
        });
    }

    [HttpGet("customers-by-item/{itemName}")]
    public IActionResult GetCustomersByItem(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            return BadRequest(new { message = "An item name is required." });
        }

        var customers = Orders
            .Where(o => o.Items.Any(i => string.Equals(i.Name, itemName, StringComparison.OrdinalIgnoreCase)))
            .Select(o => o.CustomerName)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name)
            .ToList();

        var totalQuantity = Orders
            .SelectMany(o => o.Items)
            .Where(i => string.Equals(i.Name, itemName, StringComparison.OrdinalIgnoreCase))
            .Sum(i => i.Quantity);

        if (customers.Count == 0)
        {
            return NotFound(new { message = $"No customers were found for item '{itemName}'." });
        }

        return Ok(new
        {
            itemName,
            customers,
            totalQuantity
        });
    }

    [HttpGet("customers")]
    public IActionResult GetCustomers()
    {
        var customers = Orders.Select(o => o.CustomerName).OrderBy(name => name).ToList();
        return Ok(customers);
    }

    public sealed record OrderRecord(string CustomerName, List<OrderItemRecord> Items);
    public sealed record OrderItemRecord(string Name, int Quantity);
}
