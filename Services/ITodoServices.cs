using TodoBackend.Data.Entities;

namespace TodoBackend.Services
{
    // Interface defining operations for managing Todo-related services
    public interface ITodoService
    {
        // Creates a new Todo list in the system and returns the created Todo entity
        Task<TodoEntity> CreateTodoList(TodoEntity list);

        // Retrieves a list of Todo entities for a specific user, filtering out deleted ones
        Task<IEnumerable<TodoEntity>> GetTodosByUserId(Guid userId);

        // Marks a Todo as deleted by setting a "DeletedAt" timestamp, returning a success flag
        Task<bool> DeleteTodo(Guid todoId);

        // Updates an existing Todo entity if it exists, handling concurrency and changes to its items
        Task<TodoEntity> UpdateTodo(Guid userId, Guid todoId, TodoEntity todo);
    }
}
