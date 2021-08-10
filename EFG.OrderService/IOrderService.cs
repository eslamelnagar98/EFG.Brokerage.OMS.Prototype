using OMS.BaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EFG.OrderService
{
    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        void AddOrder(Orders order);
    }
}
