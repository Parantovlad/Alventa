using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlventaDB.Repos
{
    interface IRepo<T>
    {
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
    }
}
