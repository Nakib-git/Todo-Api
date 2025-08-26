using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }

        /// <summary>
        /// Get user's todos with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
        /// <returns>Paginated list of todos</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<TodoItemDto>>> GetTodos(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var userId = GetCurrentUserId();
            var result = await _todoService.GetUserTodosAsync(userId, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific todo by ID
        /// </summary>
        /// <param name="id">Todo ID</param>
        /// <returns>Todo item</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodo(int id)
        {
            var userId = GetCurrentUserId();
            var todo = await _todoService.GetUserTodoByIdAsync(userId, id);
            
            if (todo == null)
            {
                return NotFound(new { message = "Todo not found" });
            }
            
            return Ok(todo);
        }

        /// <summary>
        /// Create a new todo
        /// </summary>
        /// <param name="createTodoDto">Todo creation details</param>
        /// <returns>Created todo item</returns>
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> CreateTodo([FromBody] CreateTodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var todo = await _todoService.CreateTodoAsync(userId, createTodoDto);
            
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        /// <summary>
        /// Update an existing todo
        /// </summary>
        /// <param name="id">Todo ID</param>
        /// <param name="updateTodoDto">Todo update details</param>
        /// <returns>Updated todo item</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItemDto>> UpdateTodo(int id, [FromBody] UpdateTodoDto updateTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var todo = await _todoService.UpdateTodoAsync(userId, id, updateTodoDto);
            
            if (todo == null)
            {
                return NotFound(new { message = "Todo not found" });
            }
            
            return Ok(todo);
        }

        /// <summary>
        /// Delete a todo
        /// </summary>
        /// <param name="id">Todo ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var userId = GetCurrentUserId();
            var success = await _todoService.DeleteTodoAsync(userId, id);
            
            if (!success)
            {
                return NotFound(new { message = "Todo not found" });
            }
            
            return NoContent();
        }

        /// <summary>
        /// Get user's todo statistics
        /// </summary>
        /// <returns>Todo statistics</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetTodoStats()
        {
            var userId = GetCurrentUserId();
            var stats = await _todoService.GetUserTodoStatsAsync(userId);
            return Ok(stats);
        }
    }
}
