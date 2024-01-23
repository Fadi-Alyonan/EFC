using System.ComponentModel.DataAnnotations;

namespace Shared.Entities;

public class AddressEntity
{
    [Key]
    public int AddressId { get; set; } 

    [StringLength(60)]
    public string? StreetName { get; set; }

    [StringLength(6)]
    public string? PostalCode { get; set; } 

    [StringLength(60)]
    public string? City { get; set; }

    public virtual ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();

}
