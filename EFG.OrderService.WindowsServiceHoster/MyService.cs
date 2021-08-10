using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EFG.OrderService.WindowsServiceHoster
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }
        ServiceHost host;
        protected override void OnStart(string[] args)
        {
            try
            {
                host = new ServiceHost(typeof(OrderService));
                host.Open();
            }
            catch (Exception ex)
            {

            }
        }


        protected override void OnStop()
        {
            host.Close();
        }
    }
}
