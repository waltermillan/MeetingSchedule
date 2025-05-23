using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("contact_tags")]
    public class ContactTag : BaseEntity
    {
        [Column("contact_id")]
        public Guid ContactId { get; set; }
        [Column("tag_id")]
        public Guid TagId { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
    }
}
