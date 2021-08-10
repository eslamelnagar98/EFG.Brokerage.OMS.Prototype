using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.BaseLibrary
{
    public enum _MessageDestination:short
    {
        NotSet = 0,
        OMSServer = 1,
        OMSDistrubutionManager = 2,
        OMSClient = 3,
    }
}
