namespace TodoBackend.Dtos
{
    public class RegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class TodoRequestDto
    {
        public required string Title { get; set; }
        public required DateTime DueDate { get; set; }
        public List<TodoItemRequestDto>? TodoItems { get; set; }
        public byte[]? RowVersion { get; set; }
    }

    public class TodoItemRequestDto
    {
        public Guid Id { get; set; }
        public required string ItemTitle { get; set; }
        public required bool Status { get; set; }
        public required bool Deleted { get; set; }
    }
}