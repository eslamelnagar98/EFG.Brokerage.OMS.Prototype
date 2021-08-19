using OMS.BaseLibrary;
using System;
using System.Configuration;
using System.Messaging;
using System.ServiceModel;

namespace EFG.OrderService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderService : IOrderService, IDisposable
    {
        OrderDbContext _context = new OrderDbContext();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddOrder(Orders order)
        {
            Logger.Info("Adding Order.");
            o_orderData orderData = new o_orderData
            { Account = order.Account, Price = order.Price, Quantity = order.Quantity };
            var  queueSrc = ConfigurationManager.AppSettings["queueSrc"].ToString();
            MessageQueue messageQueue = new MessageQueue(queueSrc);
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

                Logger.Info("Order has been added.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        public void Dispose()
        {

        }

    }
}
