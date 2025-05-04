using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Models;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IBaseRepository<TEntity, TModel> where TEntity : class
    {
    Task<RepositoryResult<bool>> AddAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
    Task<RepositoryResult<bool>> ExistAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
    }

public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
    {
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(AppDbContext context)
        {
        _context = context;
        _table = context.Set<TEntity>();
        }

    public virtual async Task<RepositoryResult<bool>> AddAsync(TEntity entity)
        {
        if (entity == null)
            return new RepositoryResult<bool>
                {
                Success = false,
                Error = "Entity cannot be null",
                StatusCode = 400
                };
        try
            {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool>
                {
                Success = true,
                Result = true,
                StatusCode = 201
                };
            }
        catch (Exception ex)
            {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool>
                {
                Success = false,
                Error = ex.Message,
                StatusCode = 500
                };
            }
        }

    public virtual async Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync
        (
            bool orderByDescending = false,
            Expression<Func<TEntity, object>>? sortBy = null,
            Expression<Func<TEntity, bool>>? where = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
        IQueryable<TEntity> query = _table;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes) ;

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TModel>());
        return new RepositoryResult<IEnumerable<TModel>>
            {
            Success = true,
            Result = result,
            StatusCode = 200
            };
        }

    public virtual async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>
    (
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        params Expression<Func<TEntity, object>>[] includes
    )
        {
        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.Select(selector).ToListAsync();



        var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return new RepositoryResult<IEnumerable<TSelect>>
            {
            Success = true,
            Result = result,
            StatusCode = 200
            };
        }

    public virtual async Task<RepositoryResult<TModel>> GetAsync
        (
            Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
        IQueryable<TEntity> query = _table;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(where);



        if (entity == null)
            return new RepositoryResult<TModel>
                {
                Success = false,
                StatusCode = 404,
                Error = "Entity not found."
                };

        var result = entity.MapTo<TModel>();
        return new RepositoryResult<TModel>
            {
            Success = true,
            StatusCode = 200,
            Result = result
            };
        }

    public virtual async Task<RepositoryResult<bool>> ExistAsync(Expression<Func<TEntity, bool>> findBy)
        {
        var exists = await _table.AnyAsync(findBy);
        return !exists ? new RepositoryResult<bool>
            {
            Success = false,
            Error = "Entity not found.",
            StatusCode = 404
            }
         : new RepositoryResult<bool>
             {
             Success = true,
             StatusCode = 200,
             };
        }

    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
        {
        if (entity == null)
            return new RepositoryResult<bool>
                {
                Success = false,
                Error = "Entity cannot be null",
                StatusCode = 400
                };
        try
            {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool>
                {
                Success = true,
                StatusCode = 200
                };
            }
        catch (Exception ex)
            {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool>
                {
                Success = false,
                Error = ex.Message,
                StatusCode = 500
                };
            }
        }

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
        {
        if (entity == null)
            return new RepositoryResult<bool>
                {
                Success = false,
                Error = "Entity cannot be null",
                StatusCode = 400
                };
        try
            {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool>
                {
                Success = true,
                StatusCode = 200
                };
            }
        catch (Exception ex)
            {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool>
                {
                Success = false,
                Error = ex.Message,
                StatusCode = 500
                };
            }
        }
    }