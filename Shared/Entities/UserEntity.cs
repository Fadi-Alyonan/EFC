using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    [ForeignKey(nameof(ProfileEntity))]
    public int ProfileId { get; set; }
    public virtual ProfileEntity Profile { get; set; } = null!;


    [ForeignKey(nameof(AddressEntity))]
    public int AddressId { get; set; }
    public virtual AddressEntity Address { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }
    public virtual RoleEntity Role { get; set; }= null!;


    [ForeignKey(nameof(PhoneNumberEntity))]
    public int PhoneNumberId { get; set;}
    public virtual PhoneNumberEntity PhoneNumber { get; set; } = null!;


    [Required]
    [StringLength(200)]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

   
    
}