using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class AddressRepository(DataContext context) : BaseRepository<AddressEntity, DataContext>(context)
{
    private readonly DataContext _context = context;
}
