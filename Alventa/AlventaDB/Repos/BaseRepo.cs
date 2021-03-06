﻿using AlventaDB.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlventaDB.Repos
{
    public abstract class BaseRepo<T> where T:class, new()
    {
        public AlventaEntities Context { get; } = new AlventaEntities();
        protected DbSet<T> Table;

        public List<T> GetAll() => Table.ToList();
        public Task<List<T>> GetAllAsync() => Table.ToListAsync();

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
