using AnnaBot.Domain.Models.Entities.Shared;
using System.Linq.Expressions;

namespace AnnaBot.Domain.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : EntityBase
{
    Task<TEntity> Get(string id);
    Task<IEnumerable<TEntity>> GetAll();
    Task Add(TEntity entity);
    Task AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task<bool> Exists(TEntity entity);
    Task<bool> Any();
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression);
}

