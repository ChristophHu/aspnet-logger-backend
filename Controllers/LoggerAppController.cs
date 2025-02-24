using Microsoft.AspNetCore.Mvc;
using aspnet_logger_backend.Models;

namespace aspnet_logger_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoggerAppController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ReadLog()
    {
        return Ok("Test API");
    }

    [HttpGet("{id}")]
    public IActionResult ReadLogEntry(int id)
    {
        return CreatedAtAction(nameof(CreateLogEntry), new { id = id });
    }

    [HttpPost]
    public IActionResult CreateLogEntry([FromBody] Logentry logentry)
    {
        return CreatedAtAction(nameof(CreateLogEntry), new { id = logentry.id }, logentry);
    }

    [HttpPut]
    public ActionResult UpdateLogEntry([FromBody] Logentry logentry)
    {
        Console.WriteLine(logentry);
        return Ok();
    }

    [HttpDelete]
    public ActionResult DeleteLogEntry([FromBody] string id)
    {
        Console.WriteLine(id);
        return Ok();
    }
}

