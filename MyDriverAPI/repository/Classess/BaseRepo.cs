using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyDriver.Model.DB;
using MyDriver.repository.Interfaces;
using System.Linq.Expressions;

namespace MyDriver.repository.Classess
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly AppDbContext context;

        
        public BaseRepo( AppDbContext context )
        {
            this.context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            var res =await context.Set<T>().AddAsync(entity);
            if(res != null) 
                return entity;

            context.SaveChanges();
            return null;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var user = await context.Set<T>().FindAsync(id);
            context.Set<T>().Remove(user);
            return user;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithInclude(Expression<Func<T, bool>> match, string[] includes=null)
        {
            IQueryable<T> query = context.Set<T>();
            if( includes!=null)
            {
                foreach(var include in includes)
                    query = query.Include(include);
            } 
                
            return await query.Where(match).ToListAsync();
        }

        public async Task<T> GetOneWithInclude(Expression<Func<T, bool>> match, string[] includes=null)
        {
            IQueryable<T> query = context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(match);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByAppIDAsync(Expression<Func<T, bool>> match)
        {
            return await context.Set<T>().SingleOrDefaultAsync(match);
        }

      
    }
}
