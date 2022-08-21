using AnnaBot.Domain.Interfaces.Repositories;
using AnnaBot.Domain.Models.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AnnaBot.Infrastructure.Repositories.Shared
{
    public class RepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity> where TEntity : EntityBase where TContext : DbContext
    {
        private readonly TContext? _db = null;
        private readonly DbSet<TEntity>? _entities = null;

        public RepositoryBase(TContext context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context));
            _entities = _db.Set<TEntity>();
        }

        public virtual async Task<TEntity> Get(string id) => await _entities.SingleOrDefaultAsync(s => s.Id == id);
        public virtual async Task<IEnumerable<TEntity>> GetAll() => await _entities.ToListAsync();
        public async virtual Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> expression) => await _entities.Where(expression).ToListAsync();
        public async Task Add(TEntity entity) => await _entities.AddAsync(entity);
        public async Task AddRange(IEnumerable<TEntity> entities) => await _entities.AddRangeAsync(entities);
        public void Update(TEntity entity) => _entities.Update(entity);
        public void Remove(TEntity entity) => _entities.Remove(entity);
        public void RemoveRange(IEnumerable<TEntity> entities) => _entities.RemoveRange(entities);
        public async Task<bool> Exists(TEntity entity) => await _entities.AnyAsync(TEntity => TEntity.Id == entity.Id);
        public async Task<bool> Any() => await _entities.AnyAsync();
    }

}
