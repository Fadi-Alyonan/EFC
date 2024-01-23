using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class PhoneNumberRepository(DataContext context) : BaseRepository<PhoneNumberEntity>(context)
{
    private readonly DataContext _context = context;
}
