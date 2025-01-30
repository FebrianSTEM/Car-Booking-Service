using hyundai_testDriveBooking_service.Domain.Interfaces;
using hyundai_testDriveBooking_service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace hyundai_testDriveBooking_service.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error occurred while Get data", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error occurred while retrieved data", ex);
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbSet.AddAsync(entity);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return entity;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new ArgumentException($"Error occurred while Saving the {entity.GetType().Name}", ex);
                }
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return entity;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new ArgumentException($"Error occurred while update the {entity.GetType().Name}", ex);
                }
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new ArgumentException($"Error occurred while delete the {entity.GetType().Name}" , ex);
                }
            }
        }
    }
}