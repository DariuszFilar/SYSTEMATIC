﻿using Microsoft.EntityFrameworkCore;
using SYSTEMATIC.DB;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;

namespace SYSTEMATIC.INFRASTRUCTURE.Repositories.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SystematicDbContext _dbContext;

        public Repository(SystematicDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            _ = await _dbContext.Set<TEntity>().AddAsync(entity);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _ = _dbContext.Set<TEntity>().Update(entity);
            _ = await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _ = _dbContext.Set<TEntity>().Remove(entity);
            _ = await _dbContext.SaveChangesAsync();
        }
    }
}
