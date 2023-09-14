using System;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Api.Focus.Interfaces;
using Api.Focus.ViewModels;
using System.Diagnostics;
using Api.Focus.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api.Focus.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupOrderController : ControllerBase
{
    private readonly IFocusGroupOrderRepository repo;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<GroupOrderController> _logger;

    public GroupOrderController(ILogger<GroupOrderController> logger, IFocusGroupOrderRepository busiinessRepo)
    {
        _logger = logger;
        repo = busiinessRepo;
    }

    [HttpGet(Name = "GetOrdersForUser")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    [Route("/CreateOrder")]
    public async Task<ActionResult> CreateOrder([FromBody] NewOrder param)
    {
        try
        {
            var x = await repo.CreateNewOrder(param);
            return Ok(new UpdateDto { success = x.success, error = x.error, orderId = x.orderId });
        }
        catch(Exception ex)
        {
            Debug.WriteLine($"exception type {ex.GetType()}, {ex.Message}");
            return Ok(new UpdateDto { success = false, error = $"error {ex.GetType()}, {ex.Message}" });
        }
    }

    [HttpPost]
    [Route("/EditOrder")]
    public async Task<ActionResult> EditOrder([FromBody] EditOrder param)
    {
        try
        {
            var x = await repo.EditOrderForUser(param);
            return Ok(new UpdateDto { success = x.success, error = x.error });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"exception type {ex.GetType()}, {ex.Message}");
            return Ok(new UpdateDto { success = false, error = $"error {ex.GetType()}, {ex.Message}" });
        }
    }

    [HttpGet]
    [Route("/GetProducts")]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        try
        {
            return await repo.GetProducts();
        }
        catch (Exception ex)
        {
            return null; //todo: return an error message or something
        }
    }

    [HttpPost]
    [Route("/GetUser")]
    public async Task<ActionResult<UserDto>> GetUser([FromBody] UserDto user)
    {
        try
        {
            var results = ConvertToUserDto( await repo.GetAllOrdersForUser(user.Email));

            return results;
        }
        catch (Exception ex)
        {
            return null; //todo: return an error message or something
        }
    }

    UserDto ConvertToUserDto(User obj)
    {
        return new UserDto
        {
            Email = obj.Email,
            UserId = obj.UserId,
            Orders = obj.UserOrders?.ConvertAll(new Converter<UserOrder, EditOrder>(ConvertToUserOrderDto)) ?? new List<EditOrder>()
        };
    }

    EditOrder ConvertToUserOrderDto(UserOrder obj)
    {
        return new EditOrder
        {
             OrderId = obj.OrderId,
             Email = obj.User?.Email,
             LineItemsPerUser = obj.LineItemsPerUser?.ConvertAll(new Converter<LineItemPerUser, LineItem>(ConvertToLineItem)) ?? new List<LineItem>(),
             IsComplete = obj.IsComplete ?? false
        };
    }

    LineItem ConvertToLineItem(LineItemPerUser obj)
    {
        return new LineItem
        {
            ProductId = obj.ProductId,
            Quantity = obj.Quantity
        };
    }


    public class UpdateDto
    {
        public UpdateDto() { }

        public bool success { get; set; }
        public string error { get; set; }
        public int orderId { get; set; }
    }

    public class UserDto
    {
        public UserDto() { }
        public int UserId { get; set; }
        public string Email { get; set; }

        public List<EditOrder> Orders { get; set; } = new List<EditOrder>();

    }
}

