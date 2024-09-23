using System.Linq.Expressions;
using FinanceApplication.Core.Extensions;
using FinanceApplication.Core.Repository;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApplication.Core.EntityFramework;

public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : BaseEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                entity.Status = (byte)StatusEnum.Active;
                entity.CreatedDate = DateTime.UtcNow;
                var addEntity = context.Entry(entity);
                addEntity.State = EntityState.Added;
                context.SaveChanges();

            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                entity.Status = (byte)StatusEnum.Deleted;
                entity.DeletedDate = DateTime.UtcNow;
                var delEntity = context.Entry(entity);
                delEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        
        public void HardDelete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var delEntity = context.Entry(entity);
                delEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, string includeTables = null)
        {
            using (var context = new TContext())
            {
                var listItem = context.Set<TEntity>().AsQueryable();
                listItem = listItem.IncludeNested(includeTables);
                return listItem.FirstOrDefault(filter);
            }
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null, string includeTables = null)
        {
            using (var context = new TContext())
            {
                var listItem = context.Set<TEntity>().AsQueryable();
                listItem = listItem.IncludeNested(includeTables);
                return filter == null
                    ? listItem.ToList()
                    : listItem.Where(filter).ToList();

            }
        }
        
        public TEntity GetLast(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, int>> orderby = null)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().OrderBy(orderby).LastOrDefault(filter);
            }
        }
        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                entity.UpdatedDate = DateTime.UtcNow;
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();

            }
        }
    }