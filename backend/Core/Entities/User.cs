using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Column("user_name")]
        public string? UserName { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("password")]
        public string? Password { get; set; }
    }
}
