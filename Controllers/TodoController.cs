using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Data.Entities;
using TodoBackend.Dtos;
using TodoBackend.Services;

namespace TodoBackend.Controllers
{
    // Controller to manage Todo lists and items
    [ApiController]
    [Route("api/v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        // Constructor to inject the ITodoService dependency
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // Helper method to extract the UserId from the claims in the JWT token
        private Guid? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return null;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        // Endpoint for creating a new Todo list
        [HttpPost]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoRequestDto request)
        {
            // Validate input
            if (request == null)
                return BadRequest("Invalid request data.");

            // Get the authenticated user's ID from claims
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Map the request DTO to the TodoEntity
            var todo = new TodoEntity
            {
                Title = request.Title,
                DueDate = request.DueDate,
                UserId = userId.Value,
                TodoItems = request.TodoItems.Select(item => new TodoItemEntity
                {
                    ItemTitle = item.ItemTitle,
                    Status = item.Status,
                    Deleted = item.Deleted,
                }).ToList()
            };

            // Call the service to create the Todo list
            var createTodo = await _todoService.CreateTodoList(todo);

            // Return a response with the created Todo list's ID
            return CreatedAtAction(nameof(CreateTodoList), new { id = createTodo.Id }, createTodo);
        }

        // Endpoint to get all Todo lists for the authenticated user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Retrieve Todo lists for the user
            var lists = await _todoService.GetTodosByUserId(userId.Value);

            // Return the lists in a successful response
            return Ok(new { Success = true, List = lists });
        }

        // Endpoint for deleting a Todo list by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Call the service to delete the Todo
            var result = await _todoService.DeleteTodo(id);
            if (!result)
                return NotFound(new { Message = "Todo not found." });

            // Return a success message upon successful deletion
            return Ok(new { Success = true, Message = "Todo deleted successfully." });
        }

        // Endpoint for updating the Todo list's due date and items
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDate(Guid id, [FromBody] TodoRequestDto request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            // Get the authenticated user's ID from claims
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Map the updated request data to the TodoEntity
            var todo = new TodoEntity
            {
                DueDate = request.DueDate,
                Title = request.Title,
                RowVersion = request.RowVersion,
                TodoItems = request.TodoItems.Select(item => new TodoItemEntity
                {
                    Id = item.Id,
                    ItemTitle = item.ItemTitle,
                    Status = item.Status,
                    Deleted = item.Deleted,
                }).ToList(),
            };

            try
            {
                // Call the service to update the Todo list
                var result = await _todoService.UpdateTodo(userId.Value, id, todo);
                return Ok(new { Success = true, Data = result.RowVersion });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log and handle concurrency errors
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "A concurrency error occurred while updating the Todo.");
            }
        }
    }
}
