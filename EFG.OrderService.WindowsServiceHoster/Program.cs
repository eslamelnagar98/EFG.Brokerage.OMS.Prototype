using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EFG.OrderService.WindowsServiceHoster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string [] args)
        {
            //var service = new MyService();
            //ServiceBase.Run(service);


            //ServiceHost host;
            //host = new ServiceHost(typeof(OrderService));
            //host.Open();
            //Console.ReadLine();
            //host.Close();

            //using (var service = new MyService())
            //{
            //    if (Environment.UserInteractive)
            //    {
            //        service.StartupAndStop(args);
            //    }
            //    else
            //    {
            //        ServiceBase.Run(service);
            //    }
            //}

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MyService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
