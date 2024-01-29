using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class RoleRepository(DataContext context) : BaseRepository<RoleEntity, DataContext>(context)
{
    private readonly DataContext _context = context;
}
