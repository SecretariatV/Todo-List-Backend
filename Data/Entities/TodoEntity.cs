using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoBackend.Data.Entities
{
    [Table("todos")]
    public class TodoEntity : CoreEntity
    {
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("title", TypeName = "varchar(255)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("due_date")]
        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public ICollection<TodoItemEntity> TodoItems { get; set; } = new List<TodoItemEntity>();

        [Column("row_version")]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [ForeignKey("User")]
        [Column("user_id")]
        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
    }
}