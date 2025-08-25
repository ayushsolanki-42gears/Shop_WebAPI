using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Implementations;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogServices _logServices;

        public LogController(ILogServices logServices)
        {
            _logServices = logServices;
        }

        [HttpGet()]
        #region Get all logs
        public IActionResult GetAllLogs()
        {
            ApiResponse response;
            var logs = _logServices.GetAllLogs();
            if (!logs.Any() || logs == null)
            {
                response = new ApiResponse("Logs not found", 404);
                return NotFound(response);
            }
            response = new ApiResponse(logs, "Logs Ftech successfully", 200);
            return Ok(response);
        }
        #endregion

        [HttpGet("getById/{id}")]
        #region Get by id
        public IActionResult GetById(string id)
        {
            ApiResponse response;
            var log = _logServices.GetLogById(id);
            if (log == null)
            {
                response = new ApiResponse("Log is not found for this id", 404);
                return NotFound(response);
            }
            response = new ApiResponse(log, "Log fetch successfully.", 200);
            return Ok(response);
        }
        #endregion

        [HttpGet("event/{eventType}")]
        #region Get logs by event
        public IActionResult GetByEvent(string eventType)
        {
            ApiResponse response;
            var logs = _logServices.GetLogsByEvent(eventType);
            if (logs == null || !logs.Any())
            {
                response = new ApiResponse("Logs not found for this event", 404);
                return NotFound(response);
            }
            response = new ApiResponse(logs, "Logs fetch successfully", 200);
            return Ok(response);
        }
        #endregion

        [HttpGet("user/{userId}")]
        #region Get logs by event
        public IActionResult GetByUser(int userId)
        {
            ApiResponse response;
            var logs = _logServices.GetLogsByUser(userId);
            if (logs == null || !logs.Any())
            {
                response = new ApiResponse("Logs not found for this user", 404);
                return NotFound(response);
            }
            response = new ApiResponse(logs, "Logs fetch successfully", 200);
            return Ok(response);
        }
        #endregion

        [HttpGet("event-type")]
        #region Get event types
        public IActionResult GetEventType()
        {
            ApiResponse response;
            var types = _logServices.GetEventTypes();
            if (types == null || !types.Any())
            {
                response = new ApiResponse("Event Types not found", 404);
                return NotFound(response);
            }
            response = new ApiResponse(types, "Event types fetch successfully", 200);
            return Ok(response);
        }
        #endregion
    }
}