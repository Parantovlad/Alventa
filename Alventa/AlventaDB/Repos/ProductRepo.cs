using AlventaDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlventaDB.Repos
{
    public class ProductRepo : BaseRepo<Product>, IRepo<Product>
    {
        public ProductRepo()
        {
            Table = Context.Products;
        }
    }
}
