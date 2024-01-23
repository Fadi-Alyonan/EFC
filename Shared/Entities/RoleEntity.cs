using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

[Index(nameof(RoleName), IsUnique = true)]
public class RoleEntity
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    [StringLength(30)]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
}
