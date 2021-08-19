using OMS.BaseLibrary;
using OMSClient.ServiceReference1;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace OMSClient
{
    public partial class Form1 : Form
    {
        private static TcpClient o_TcpClient;
        private static NetworkStream o_NetworkStream;
        private Thread o_Thread;

        public bool IsWorking = true;
        private DateTime dtLastRecivedDMMessageTime;

        tdsBroadCasting.OrderDataTable  orderTable = new tdsBroadCasting.OrderDataTable();
        tdsBroadCasting.OrderRow orderDataRow;
        public Form1()
        {
            InitializeComponent();
            this.DataFeedGrid.DataSource = orderTable;
            o_TcpClient = new TcpClient("localhost", 8097);
            o_NetworkStream = o_TcpClient.GetStream();
            o_Thread = new Thread(new ThreadStart(Listen));
            o_Thread.IsBackground = true;
            o_Thread.Start();

        }
        private static void SendMsgToServer(oEnvelop o_oEnvelop)
        {
            try
            {
                if (o_TcpClient.Connected)
                {
                    o_oEnvelop.Serialize(o_NetworkStream);
                }
                else
                {
                    throw new Exception("Connection with DM is closed");
                }
            }
            catch (Exception gXp)
            {
                throw gXp;
            }
        }
        internal static void Authantication(string strUserName, string strPassword)
        {
            try
            {
                var o_oEnvelopMsgToServer = new oEnvelop();
                o_oEnvelopMsgToServer.MessageType = _MessageType.ClientLogIn;
                oClientLogIn o_oClientLogIn = new oClientLogIn();
                o_oClientLogIn.UserName = strUserName;
                o_oClientLogIn.Password = strPassword;
                o_oClientLogIn.ClientIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                o_oEnvelopMsgToServer.oMessages.Add(o_oClientLogIn);
                SendMsgToServer(o_oEnvelopMsgToServer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Listen()

        {
            try
            {
                oEnvelop o_oEnvelop;
                while (IsWorking)
                {
                    while (!o_NetworkStream.DataAvailable && IsWorking)
                        Thread.Sleep(100);
                    if (!IsWorking)
                        break;
                    try
                    {
                        o_oEnvelop = new oEnvelop().Deserialize(o_NetworkStream) as oEnvelop;
                        dtLastRecivedDMMessageTime = DateTime.Now;

                        ProcessMessage(o_oEnvelop);
                    }
                    catch (Exception gXp)
                    {

                    }
                }
            }
            catch (ThreadAbortException thr_Xp)
            {
            }
            catch (Exception gXp)
            {

            }
        }

        private void ProcessMessage(oEnvelop o_oEnvelop)
        {
            if (o_oEnvelop.MessageType == _MessageType.UserInfo)
            {
                try
                {
                    var o_oUserInfo = (oUserInfo)o_oEnvelop.oMessages[0];

                    this.Invoke((MethodInvoker)delegate
                    {
                        // Running on the UI thread
                        lblStatus.Text = $"Status: Connected Successfully ({o_oUserInfo.DMSessionID})";
                    });
                }
                catch (Exception exp)
                {

                }
                //MessageBox.Show("Login Successfully..."); 
            }
            else if (o_oEnvelop.MessageType == _MessageType.DataFeed)
            {
                // into grid
                var o_OrderData = (o_orderData)o_oEnvelop.oMessages[0];
                orderDataRow = orderTable.NewOrderRow();
                orderDataRow.Account = o_OrderData.Account;
                orderDataRow.Price = o_OrderData.Price;
                orderDataRow.Quantity = o_OrderData.Quantity;
                orderTable.AddOrderRow(orderDataRow);

                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = $"Status: Recived Messages From {o_OrderData}";
                });
            }
        }

        private async void btnOrder_Click(object sender, EventArgs e)
        {
            OrderServiceClient proxyClient = new OrderServiceClient("NetTcpBinding_IOrderService");
            string account = txtAccount.Text ?? "Na";
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
                price = 0m;
            if (!int.TryParse(txtQuantity.Text, out int quantity))
                quantity = 0;
            var order = new Orders { Account = account, Price = price, Quantity = quantity };
            await proxyClient.AddOrderAsync(order);
            proxyClient.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Status: Connecting ...";
            var username = txtAccount.Text;
            if (string.IsNullOrWhiteSpace(username))
                username = "omsadmin";
            Authantication(username, "1");
        }
    }
}
