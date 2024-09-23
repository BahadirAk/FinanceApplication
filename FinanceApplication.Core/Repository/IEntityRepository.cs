using System.Linq.Expressions;
using FinanceApplication.Entities.Concrete;

namespace FinanceApplication.Core.Repository;

public interface IEntityRepository<T> where T : BaseEntity, new()
{
    T Get(Expression<Func<T, bool>> filter=null, string includeTables = null);

    IList<T> GetList(Expression<Func<T, bool>> filter = null, string includeTables = null);
    T GetLast(Expression<Func<T, bool>> filter = null, Expression<Func<T, int>> orderby = null);
    void Add(T entity);

    void Delete(T entity);
    void HardDelete(T entity);
    void Update(T entity);
    
}