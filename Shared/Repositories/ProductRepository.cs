using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Shared.Repositories;

public class ProductRepository(ProductDataContext context) : BaseRepository<Product, ProductDataContext>(context)
{
    private readonly ProductDataContext _context = context;
    public override async Task<IEnumerable<Product>> GetAll()
    {
        try
        {
            var result = await _context.Products
                    .Include(u => u.Category)
                    .Include(u => u.Manufacturer)
                    .Include(u => u.Price)
                    .Include(u => u.Production).ToListAsync();
            if (result != null)
            {
                return result;
            }

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override async Task<Product> GetOne(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            var result = await _context.Products
                   .Include(u => u.Category)
                   .Include(u => u.Manufacturer)
                   .Include(u => u.Price)
                   .Include(u => u.Production).FirstOrDefaultAsync(predicate);
            if (result != null)
            {
                return result;
            }

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }
}
