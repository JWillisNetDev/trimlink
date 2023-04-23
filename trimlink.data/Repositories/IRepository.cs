﻿using Microsoft.EntityFrameworkCore;

namespace trimlink.data.Repositories;

public interface IRepository<TEntity, TKey> : IDisposable
    where TEntity : class
{
    TEntity? Get(TKey id);
    IEnumerable<TEntity> GetAll();

    TEntity? Find(Func<TEntity, bool> predicate);
    IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

    void Add(TEntity entity);
    void Remove(TKey id);
    void Update(TEntity entity);
}