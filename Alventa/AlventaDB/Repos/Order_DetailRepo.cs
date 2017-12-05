using AlventaDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlventaDB.Repos
{
    public class Order_DetailRepo : BaseRepo<Order_Detail>, IRepo<Order_Detail>
    {
        public Order_DetailRepo()
        {
            Table = Context.Order_Details;
        }
    }
}
