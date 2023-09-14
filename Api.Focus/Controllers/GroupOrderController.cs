using System;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Api.Focus.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupOrderController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<GroupOrderController> _logger;

    public GroupOrderController(ILogger<GroupOrderController> logger)
    {
        _logger = logger;
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
}

