using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.IRepositories
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);

        IEnumerable<TodoItem> GetAll();

        TodoItem Find(string key);

        TodoItem Remove(string key);

        void Update(TodoItem item);
    }
}
