﻿using Infrastructure.Models;

namespace Core.Repositories.BaseRepository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetById(int id);
        Task<List<T>> GetAll();
        Task<bool> DoesExist(int id);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task DeleteById(int id);
        Task Delete(T entity);

        Task<int> CountTotal();
    }
}
