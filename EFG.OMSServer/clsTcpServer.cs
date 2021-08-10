using NLog;
using OMS.BaseLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EFG.OMSServer
{
    public class clsTcpServer
    {
        static TcpListener o_TcpListener;
        Thread o_ServerListenerThread;
        private static readonly Logger m_Logger = LogManager.GetCurrentClassLogger();


        private bool IsWorking = true;
        public static string queueSrc;
        private Dictionary<string, ClientSessionHandler> collection;
        public static MessageQueue msgQueue;
        tdsBroadCasting.OrderDataTable orderTable = new tdsBroadCasting.OrderDataTable();
        tdsBroadCasting.OrderRow orderDataRow;
        public clsTcpServer()
        {
            collection = new Dictionary<string, ClientSessionHandler>();
            File.AppendAllText("C:\\app\\LogFile.txt", Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString() + Environment.NewLine);
            o_TcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8097);
            o_ServerListenerThread = new Thread(new ThreadStart(this.Listen));
            o_ServerListenerThread.Name = "OMS Server main listen";
            o_ServerListenerThread.Start();
            o_ServerListenerThread.IsBackground = true;
        }

        private void Listen()
        {
            File.AppendAllText("C:\\app\\LogFile.txt", "Before Listen Start" + Environment.NewLine);
            o_TcpListener.Start();
            File.AppendAllText("C:\\app\\LogFile.txt", "After Listen Start" + Environment.NewLine);

            while (IsWorking)
            {
                try
                {
                    Socket o_Socket = o_TcpListener.AcceptSocket();
                    if (o_Socket.Connected)
                    {
                        string SessionID = Guid.NewGuid().ToString();
                        ClientSessionHandler o_ClientSessionHandler = new ClientSessionHandler(o_Socket, orderTable);
                        collection[SessionID] = o_ClientSessionHandler;
                    }
                }
                catch (Exception gXp)
                {
                    m_Logger.ErrorException("An error occurred while accepting/starting DistributionManager session", gXp);
                }
            }
        }
        public void BroadcastMsgToClients(o_orderData orderData)
        {
            foreach (var item in collection)
            {
                try
                {
                    var o_oEnvelopMessagetoClient = new oEnvelop(_MessageType.DataFeed);
                    o_oEnvelopMessagetoClient.IsClientMessage = true;


                    o_oEnvelopMessagetoClient.oMessages.Add(orderData);
                    o_oEnvelopMessagetoClient.Serialize(item.Value.o_NetworkStream);

                    m_Logger.Trace($"server is sending new message to clients orderData: {orderData}");
                }
                catch (Exception gXp)
                {

                }
            }
        }

        public void ListenToQueue()
        {
            queueSrc = ConfigurationManager.AppSettings["queueSrc"].ToString();
            msgQueue = new MessageQueue(queueSrc);
            //msgQueue = new MessageQueue(@".\private$\orders2");
            msgQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(o_orderData) });

            msgQueue.PeekCompleted += new PeekCompletedEventHandler(msgQueue_PeekCompleted);
            msgQueue.BeginPeek();
        }

        private void msgQueue_PeekCompleted(object source, PeekCompletedEventArgs e)
        {
            try
            {
                MessageQueue messageQueue = (MessageQueue)source;
                Message message = messageQueue.EndPeek(e.AsyncResult);
                var msg = (o_orderData)message.Body;
                orderDataRow = orderTable.NewOrderRow();
                orderDataRow.Account = msg.Account;
                orderDataRow.Price = msg.Price;
                orderDataRow.Quantity = msg.Quantity;
                orderTable.AddOrderRow(orderDataRow);

                BroadcastMsgToClients(msg);

                messageQueue.Receive();

            }
            catch (Exception)
            {


            }

            msgQueue.BeginPeek();

        }



    }
}
