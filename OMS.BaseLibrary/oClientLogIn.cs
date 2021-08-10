using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.BaseLibrary
{
    [Serializable()]
    public class oClientLogIn
    {
        public string UserName;
        public string Password;
        public string ClientIP = string.Empty;

        #region Custom Serialization

        public void Serialize(System.IO.Stream stream)
        {
            SerializationWriter wr = SerializationWriter.GetWriter(stream);
            wr.Write(this.UserName);
            wr.Write(this.Password);
            wr.Write(this.ClientIP);
        }

        public object Deserialize(System.IO.Stream stream)
        {
            SerializationReader rd = SerializationReader.GetReader(stream);
            this.UserName = rd.ReadString();
            this.Password = rd.ReadString();
            this.ClientIP = rd.ReadString();

            return this;
        }

        #endregion
    }
}
