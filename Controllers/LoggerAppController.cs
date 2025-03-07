using Microsoft.AspNetCore.Mvc;
using aspnet_logger_backend.Models;
using aspnet_logger_backend.Data;
using Microsoft.EntityFrameworkCore;
using log4net;
using aspnet_logger_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aspnet_logger_backend.Controllers;

[Route("[controller]")]
[ApiController]
public class LoggerAppController : ControllerBase
{
    private ILog log = LogManager.GetLogger(typeof(LoggerAppController));

    protected ConfigService _config;
    protected DatabaseService _db;

    public LoggerAppController(ConfigService configService, DatabaseService db)
    {
        log.Debug("Initialisiere Controller");
        this._config = configService;
        this._db = db;
    }

    //private readonly ApplicationDbContext _dbContext;
    //public LoggerAppController(ApplicationDbContext dbContext)
    //{
    //    _dbContext = dbContext;
    //}

    [HttpGet("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors]
    public ActionResult<ApiInfo> info()
    {
        ApiInfo apiInfo = new ApiInfo();
        apiInfo.Name = this._config.get("api:name");
        apiInfo.Version = this._config.get("api:version");
        apiInfo.Date = DateTime.Now;
        return Ok(apiInfo);
    }

    [HttpGet("get")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    [EnableCors]
    public IActionResult ReadLog()
    {
        try
        {
            string error;
            object[] data = _db.getAll(out error);
            if (data != null && error == null)
            {
                return Ok(data);
            }
            return Ok("Test");
            //return Ok(_dbContext.Logentrys.ToList());
        }
        catch
        {
            return BadRequest();
        }
        
    }

    //[HttpGet("{id}")]
    //public IActionResult ReadLogEntry(int id)
    //{
    //    var logentry = _dbContext.Logentrys.Find(id);
    //    if (logentry == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(logentry);
    //}

    //[HttpPost]
    //public IActionResult CreateLogEntry([FromBody] Logentry logentry)
    //{
    //    _dbContext.Logentrys.Add(logentry);
    //    _dbContext.SaveChanges();
    //    return CreatedAtAction(nameof(CreateLogEntry), new { id = logentry.id }, logentry);
    //}

    //[HttpPut]
    //public ActionResult UpdateLogEntry(int id, [FromBody] Logentry logentry)
    //{
    //    var logentryInDb = _dbContext.Logentrys.Find(id);
    //    if (logentryInDb == null)
    //    {
    //        return NotFound();
    //    }

    //    logentryInDb.Message = logentry.Message;

    //    _dbContext.SaveChanges();
    //    return Ok();
    //}

    //[HttpDelete]
    //public ActionResult DeleteLogEntry(int id)
    //{
    //    var logentry = _dbContext.Logentrys.Find(id);
    //    if (logentry == null)
    //    {
    //        return NotFound();
    //    }

    //    _dbContext.Logentrys.Remove(logentry);
    //    _dbContext.SaveChanges();
    //    return Ok();
    //}
}

