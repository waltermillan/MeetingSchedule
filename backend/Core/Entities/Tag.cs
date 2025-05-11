using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("tags")]
    public class Tag : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("color")]
        public string Color { get; set; }
    }
}
