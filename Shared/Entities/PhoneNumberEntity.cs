using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

public class PhoneNumberEntity
{
    [Key]
    public int PhoneNumberId { get; set; }

    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
}
