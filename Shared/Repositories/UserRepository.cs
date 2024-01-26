using Microsoft.EntityFrameworkCore;
using Shared.Contexts;
using Shared.Entities;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Shared.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<IEnumerable<UserEntity>> GetAll()
    {
        try
        {
            var result = await _context.Users
                    .Include(u => u.Profile)
                    .Include(u => u.Address)
                    .Include(u => u.Role)
                    .Include(u => u.PhoneNumber).ToListAsync();
            if (result != null)
            {
                return result;
            }

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override async Task<UserEntity> GetOne(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var result = await _context.Users
                    .Include(u => u.Profile)
                    .Include(u => u.Address)
                    .Include(u => u.Role)
                    .Include(u => u.PhoneNumber).FirstOrDefaultAsync(predicate);
            if (result != null)
            {
                return result;
            }

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }
}
