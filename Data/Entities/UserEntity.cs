using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoBackend.Data.Entities
{
    [Table("user")]
    public class UserEntity : CoreEntity
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Column("username", TypeName = "varchar(255)")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("email", TypeName = "varchar(255)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; } = string.Empty;

        public ICollection<TodoEntity> Todos { get; set; } = new List<TodoEntity>();
    }

}