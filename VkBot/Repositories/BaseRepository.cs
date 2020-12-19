using System.Linq;
using Microsoft.EntityFrameworkCore;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext DbContext;
        protected readonly DbSet<T> DbSet;

        protected BaseRepository(AppDbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<T>();
        }

        public virtual IQueryable<T> Select()
        {
            return DbSet;
        }

        public virtual IQueryable<T> Get(int id)
        {
            return Select().Where(x => x.Id == id);
        }

        public virtual int InsertOrUpdate(T entity)
        {
            DbContext.Entry(entity).State = entity.Id == 0 ? EntityState.Added : EntityState.Modified;
            DbContext.SaveChanges();
            return entity.Id;
        }
    }

}
