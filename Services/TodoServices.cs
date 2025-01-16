using Microsoft.EntityFrameworkCore;
using TodoBackend.Data;
using TodoBackend.Data.Entities;

namespace TodoBackend.Services
{
    public class todoService : ITodoService
    {
        private readonly AppDbContext _dbContext;

        // Constructor that initializes the DbContext for database operations
        public todoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Creates a new TodoList and saves it to the database
        public async Task<TodoEntity> CreateTodoList(TodoEntity list)
        {
            await _dbContext.todos.AddAsync(list);
            await _dbContext.SaveChangesAsync();
            return list;
        }

        // Retrieves a list of todos associated with a specific user by their userId
        // Only returns todos that are not deleted
        public async Task<IEnumerable<TodoEntity>> GetTodosByUserId(Guid userId)
        {
            return await _dbContext.todos
                .Include(list => list.TodoItems.Where(item => item.Deleted == false))
                .Where(t => t.UserId == userId && t.DeletedAt == null)
                .ToListAsync();
        }

        // Deletes a Todo by setting its DeletedAt timestamp
        // Returns false if the Todo is not found, true if deletion is successful
        public async Task<bool> DeleteTodo(Guid todoId)
        {
            var todo = await _dbContext.todos.FindAsync(todoId);

            if (todo == null) return false;

            todo.DeletedAt = DateTime.UtcNow;

            _dbContext.todos.Update(todo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Updates an existing Todo with new information if it exists and belongs to the specified user
        // Handles concurrency by checking RowVersion to prevent conflicting updates
        public async Task<TodoEntity> UpdateTodo(Guid userId, Guid todoId, TodoEntity todo)
        {
            var existingTodo = await _dbContext.todos
                .Include(t => t.TodoItems)
                .Where(t => t.DeletedAt == null && t.Id == todoId && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (existingTodo == null)
            {
                Console.WriteLine($"Todo not found. TodoId: {todoId}, UserId: {userId}");
                return null;
            }

            if (existingTodo.RowVersion == null || todo.RowVersion == null ||
                !existingTodo.RowVersion.SequenceEqual(todo.RowVersion))
            {
                throw new DbUpdateConcurrencyException("The record was modified by another user.");
            }

            existingTodo.UpdatedAt = DateTime.UtcNow;
            existingTodo.DueDate = todo.DueDate;
            existingTodo.Title = todo.Title;
            existingTodo.RowVersion = Guid.NewGuid().ToByteArray();

            var existingItems = existingTodo.TodoItems ?? new List<TodoItemEntity>();
            var newItems = todo.TodoItems ?? new List<TodoItemEntity>();

            // Syncs the TodoItems: update existing items, remove deleted ones, and add new ones
            foreach (var existingItem in existingItems.ToList())
            {
                var matchingNewItem = newItems.FirstOrDefault(x => x.Id == existingItem.Id);

                if (matchingNewItem != null)
                {
                    existingItem.ItemTitle = matchingNewItem.ItemTitle;
                    existingItem.Status = matchingNewItem.Status;
                    existingItem.Deleted = matchingNewItem.Deleted;

                    newItems.Remove(matchingNewItem);
                }
                else
                {
                    _dbContext.Remove(existingItem);
                }
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return existingTodo;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("Concurrency issue: " + ex.Message);
                return null;
            }
        }
    }
}
