using OMS.BaseLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EFG.OrderService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderService : IOrderService, IDisposable
    {
        readonly OrderDbContext _context = new OrderDbContext();

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddOrder(Orders order)
        {
            o_orderData orderData = new o_orderData
            { Account = order.Account, Price = order.Price, Quantity = order.Quantity };
            MessageQueue messageQueue = new MessageQueue(@".\private$\orders");
            Message message = new Message();
            try
            {
                using (MessageQueueTransaction transaction = new MessageQueueTransaction())
                {
                    transaction.Begin();
                    message.Label = "Test Message Queue";
                    message.Formatter = new XmlMessageFormatter();
                    message.Body = orderData;
                    message.Priority = MessagePriority.AboveNormal;
                    message.Recoverable = true;
                    message.UseDeadLetterQueue = true;
                    //Console.WriteLine($"{message.Body.ToString()}");
                    messageQueue.Send(message, transaction);
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.Message);
            }

        }

        public void Dispose()
        {

        }

    }
}
