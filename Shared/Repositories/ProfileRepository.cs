using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class ProfileRepository(DataContext context) : BaseRepository<ProfileEntity, DataContext>(context)
{
    private readonly DataContext _context = context;
}
