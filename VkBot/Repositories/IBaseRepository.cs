using System.Linq;
using VkBot.Entities;

namespace VkBot.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> Get(int id);
        IQueryable<T> Select();
        int InsertOrUpdate(T entity);
    }
}
