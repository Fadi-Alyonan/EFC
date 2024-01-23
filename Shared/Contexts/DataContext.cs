using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Contexts;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<AddressEntity> Addresses { get; set; }
    public virtual DbSet<PhoneNumberEntity> PhoneNumbers { get; set; }
    public virtual DbSet<ProfileEntity> Profiles { get; set; }
    public virtual DbSet<RoleEntity> Roles { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }

}
