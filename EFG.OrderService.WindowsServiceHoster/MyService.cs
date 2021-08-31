using NLog;
using System;
using System.ServiceModel;
using System.ServiceProcess;

namespace EFG.OrderService.WindowsServiceHoster
{
    public partial class MyService : ServiceBase
    {
        private ServiceHost host;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public MyService()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            try
            {
                host = new ServiceHost(typeof(OrderService));
                host.Open();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Info($"Servie has been started");
        }


        protected override void OnStop()
        {            
            host.Close();
            Logger.Info($"Servie has been Stopped");
        }
    }
}
