using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class PriceRepository(ProductDataContext context) : BaseRepository<Price, ProductDataContext>(context)
{
    private readonly ProductDataContext _context = context;
}
