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
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace aspnet_logger_backend.Controllers;

[Route("[controller]")]
[ApiController]
public class LogController : ControllerBase
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

    [HttpGet("/info")]
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

    [HttpGet("/get")]
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

    [HttpPost("/insert")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult insert([FromBody] Logentry logentry)
    {
        try
        {
            // JSON in einen String serialisieren
            //string requestString = jdata.GetRawText();

            string error;
            //string result = null;

            //log.Debug(requestString);

            //JObject jrequest = JObject.Parse(requestString);

            //string data = (string)jrequest["data"];

            string idResult;
            if ((idResult = _db.insertOrUpdateData(null, logentry.data, out error)) != null)
            {
                return Ok(idResult);
            }
            else
            {
                return Problem(
                    detail: error,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
        catch (HttpRequestException ex)
        {
            log.Error(ex.Message);
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpPatch("/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public IActionResult update([FromBody] Logentry logentry)
    {
        try
        {
            string error;

            string idResult;
            if ((idResult = _db.insertOrUpdateData(logentry.id, logentry.data, out error)) != null)
            {
                return Ok(idResult);
            }
            else
            {
                return Problem(
                    detail: error,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
        catch (HttpRequestException ex)
        {
            log.Error(ex.Message);
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpDelete("/delete")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [EnableCors]
    public IActionResult delete(string id)
    {
        try
        {
            string error;
            if (_db.deleteData(id, out error))
            {
                return Ok("Daten gelöscht");
            }
            else
            {
                return Problem(
                    detail: error,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
        catch (HttpRequestException ex)
        {
            log.Error(ex.Message);
            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}

