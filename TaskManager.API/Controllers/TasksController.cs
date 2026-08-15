using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Contracts;
using TaskManager.API.Models.TaskItems;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _task;

    public TasksController(ITaskRepository task)
    {
        _task = task;
    }

    // GET: api/Tasks
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasks()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tasks = await _task.GetAllTaskItems(userId);

        return Ok(tasks);
    }

    // GET: api/Tasks/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TaskItemDto>> GetTask(int id)
    {
        //var user

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = await _task.GetTaskItem(userId, id, false);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    // PUT: api/Tasks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutTask(int id, TaskItemDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var success = await _task.UpdateTaskItem(userId, taskDto, id);

        if (!success)
            return NotFound();

        return NoContent();
        //if (id != task.Id)
        //{
        //    return BadRequest();
        //}

        //_context.Entry(task).State = EntityState.Modified;

        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!TaskExists(id))
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        throw;
        //    }
        //}
    }

    // POST: api/Tasks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> PostTask(TaskItemDto task)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _task.CreateTaskItem(userId, task);

        return StatusCode(
            StatusCodes.Status201Created,
            new
            {
                message = "Task successfully created"
            }
        );

        //_context.Tasks.Add(task);
        //await _context.SaveChangesAsync();

        //return CreatedAtAction("GetTask", new { id = task.Id }, task);
    }

    //DELETE: api/Tasks/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = await _task.GetTaskItem(userId, id, true);

        if (task == null)
        {
            return NotFound();
        }

        await _task.DeleteTaskItem(userId, task, id);

        return NoContent();
    }
}
