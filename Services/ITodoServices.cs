using TodoBackend.Data.Entities;

namespace TodoBackend.Services
{
    public interface ITodoService
    {
        Task<TodoEntity> CreateTodoList(TodoEntity list);

        // Retrieves a list of Todo entities for a specific user, filtering out deleted ones
        Task<IEnumerable<TodoEntity>> GetTodosByUserId(Guid userId);

        // Marks a Todo as deleted by setting a "DeletedAt" timestamp, returning a success flag
        Task<bool> DeleteTodo(Guid todoId);

        // Updates an existing Todo entity if it exists, handling concurrency and changes to its items
        Task<TodoEntity> UpdateTodo(Guid userId, Guid todoId, TodoEntity todo);
    }
}
