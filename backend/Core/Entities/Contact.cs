using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("contacts")]
    public class Contact : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]  
        public DateTime UpdatedAt { get; set; }

    }
}
