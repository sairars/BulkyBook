﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity? Get(Expression<Func<TEntity, bool>> predicate, IEnumerable<string>? includeProperties = null);
        IEnumerable<TEntity> GetAll(IEnumerable<string>? includeProperties = null);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
