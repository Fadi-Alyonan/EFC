using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;


public class RoleEntity
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    [StringLength(30)]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
}
