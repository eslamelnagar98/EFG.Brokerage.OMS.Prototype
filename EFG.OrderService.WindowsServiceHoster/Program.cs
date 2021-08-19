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
        static void Main(string [] args)
        {
#if DEBUG
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MyService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
