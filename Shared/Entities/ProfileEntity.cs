using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

public class ProfileEntity
{
    [Key]
    public int ProfileId { get; set; }

    [Required]
    [StringLength(30)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
}
