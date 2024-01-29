using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class ManufacturerRepository(ProductDataContext context) : BaseRepository<Manufacturer, ProductDataContext>(context)
{
    private readonly ProductDataContext _context = context;
}
