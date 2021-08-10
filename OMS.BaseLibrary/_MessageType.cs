using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.BaseLibrary
{
    public enum _MessageType:short
    {
        NullType = 0,
        ClientLogIn = 202,
        UserInfo = 204,
        DataFeed = 205,
    }
}
