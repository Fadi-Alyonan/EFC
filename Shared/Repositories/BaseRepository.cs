using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Shared.Repositories;

public abstract class BaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    private readonly TContext _context;

    protected BaseRepository(TContext context)
    {
        _context = context;
    }


    public virtual async Task<TEntity> Create(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex) {
            Debug.WriteLine("Error :: " + ex.Message);
           
           
            
        }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        try
        {
            var result = await _context.Set<TEntity>().ToListAsync();
            if (result != null)
            {
                return result;
            }
           
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message);}
        return null!;
    }
    public virtual async Task<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (result != null)
            {
                return result;
            }
            
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public virtual async Task<TEntity> Update(Expression<Func<TEntity, bool>> predicate, TEntity entity)
    {

        try
        {
            var entityToUpdate = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entityToUpdate != null)
            {
                _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                
                return entityToUpdate;
            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }
    public virtual async Task<bool> Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;
    }

    public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = await _context.Set<TEntity>().AnyAsync(predicate);
            return result;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;
    }
}
