using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class ProductionInformationRepository(ProductDataContext context) : BaseRepository<ProductionInformation, ProductDataContext>(context)
{
    private readonly ProductDataContext _context = context;
}
