using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class BaseEntity
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
}
