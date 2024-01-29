using Shared.Contexts;
using Shared.Entities;

namespace Shared.Repositories;

public class CategoryRepository(ProductDataContext context) : BaseRepository<Category, ProductDataContext>(context)
{
    private readonly ProductDataContext _context = context;
}
