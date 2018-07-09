using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Domain.IRepositories
{
    public interface ITodoRepository
    {
        string Find(string id);
    }
}
