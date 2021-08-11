using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.BaseLibrary
{
    public class OrderDbContext:DbContext
    {
        //public OrderDbContext():base("server=.;database=OrderingServiceDemoV1;trusted_connection=true") {}
        public virtual DbSet<Orders> Orders {get; set;}

    }
}
