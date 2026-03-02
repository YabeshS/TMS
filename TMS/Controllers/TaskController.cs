using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public readonly ITaskService _taskservice;
        public TaskController(ITaskService taskservice) 
        { 
            _taskservice = taskservice;
        }
        [HttpPost("Create Task")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateTask(CreateTaskDto ceatetaskdto)
        {
            var result = await _taskservice.CreateTaskAsync(ceatetaskdto);
            return Ok(result);
        }
        [HttpGet("Get All Tasks")]
        [Authorize(Roles ="Manager,Admin")]
        public async Task<IActionResult> GetallTask()
        {
            var result = await _taskservice.GetallTasks();
            return Ok(result);
        }
    }
}
