using Microsoft.AspNetCore.Mvc;
using aspnet_logger_backend.Models;
using aspnet_logger_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace aspnet_logger_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoggerAppController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    public LoggerAppController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ReadLog()
    {
        return Ok(_dbContext.Logentrys.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult ReadLogEntry(int id)
    {
        var logentry = _dbContext.Logentrys.Find(id);
        if (logentry == null)
        {
            return NotFound();
        }
        return Ok(logentry);
    }

    [HttpPost]
    public IActionResult CreateLogEntry([FromBody] Logentry logentry)
    {
        _dbContext.Logentrys.Add(logentry);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(CreateLogEntry), new { id = logentry.id }, logentry);
    }

    [HttpPut]
    public ActionResult UpdateLogEntry(int id, [FromBody] Logentry logentry)
    {
        var logentryInDb = _dbContext.Logentrys.Find(id);
        if (logentryInDb == null)
        {
            return NotFound();
        }

        logentryInDb.Message = logentry.Message;

        _dbContext.SaveChanges();
        return Ok();
    }

    [HttpDelete]
    public ActionResult DeleteLogEntry(int id)
    {
        var logentry = _dbContext.Logentrys.Find(id);
        if (logentry == null)
        {
            return NotFound();
        }

        _dbContext.Logentrys.Remove(logentry);
        _dbContext.SaveChanges();
        return Ok();
    }
}

