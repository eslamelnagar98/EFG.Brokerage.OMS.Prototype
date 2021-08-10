using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.BaseLibrary
{
    [Serializable()]
    public class oUserInfo
    {
        public bool IsClientAuthanticated = false;
        public long UserID;
        public string UserNameA = string.Empty;
        public string UserNameE = string.Empty;
        //Session IDs
        public string DMSessionID;
        public string UserSessionID;

        //Resources
        public bool rsAdmin = false;
        public bool rsOrderFeed = false;
        public bool rsOrder = false;
        public bool rsUpdateWithOverride = false;
        public bool IsSendFixOrders = false;
        public bool IsOnVacation = false;

        //Default Company & Market
        public string DefaultCompany = string.Empty;
        public short DefaultMarket = -1;

        public short UserTypes = (short)0;
        public string ClientIP = string.Empty;
        public ArrayList UserCompanies = new ArrayList();
        public ArrayList UserMarkets = new ArrayList();
        public ArrayList UserAssistances = new ArrayList();

        #region Methods
        public string UserCompaniesList
        {
            get
            {
                string strCompaniesList = string.Empty;
                if (UserCompanies != null)
                {
                    foreach (object oCompanyCode in UserCompanies)
                    {
                        strCompaniesList += oCompanyCode.ToString() + ",";
                    }
                    if (strCompaniesList.Length > 0)
                    {
                        strCompaniesList = strCompaniesList.Substring(0, strCompaniesList.Length - 1);
                    }
                    else
                    {
                        strCompaniesList = "-1";
                    }
                }
                return strCompaniesList;
            }
        }

        public string UserMarketList
        {
            get
            {
                string strMarketList = string.Empty;
                if (UserMarkets != null)
                {
                    foreach (object oMarketID in UserMarkets)
                    {
                        strMarketList += oMarketID.ToString() + ",";
                    }
                    if (strMarketList.Length > 0)
                    {
                        strMarketList = strMarketList.Substring(0, strMarketList.Length - 1);
                    }
                    else
                    {
                        strMarketList = "-1";
                    }
                }
                return strMarketList;
            }
        }

        public string UserAssistanceList
        {
            get
            {
                string strUserAssistanceList = string.Empty;
                if (UserAssistances != null)
                {
                    foreach (object UserAssistance in UserAssistances)
                    {
                        strUserAssistanceList += UserAssistance.ToString() + ",";
                    }
                    if (strUserAssistanceList.Length > 0)
                    {
                        strUserAssistanceList = strUserAssistanceList.Substring(0, strUserAssistanceList.Length - 1);
                    }
                    else
                    {
                        strUserAssistanceList = "-1";
                    }
                }
                return strUserAssistanceList;
            }
        }
        #endregion

        #region Custom Serialization

        public void Serialize(System.IO.Stream stream)
        {
            SerializationWriter wr = SerializationWriter.GetWriter(stream);
            wr.Write(this.IsClientAuthanticated);
            wr.Write(this.UserID);
            wr.Write(this.UserNameA);
            wr.Write(this.UserNameE);
            wr.Write(this.DMSessionID);
            wr.Write(this.UserSessionID);
            wr.Write(this.rsAdmin);
            wr.Write(this.rsOrderFeed);
            wr.Write(this.rsOrder);
            wr.Write(this.rsUpdateWithOverride);
            wr.Write(this.IsSendFixOrders);
            wr.Write(this.IsOnVacation);
            wr.Write(this.DefaultCompany);
            wr.Write((short)this.DefaultMarket);
            wr.Write((short)this.UserTypes);
            wr.Write(this.ClientIP);
            wr.Write(this.UserCompanies.ToArray());
            wr.Write(this.UserMarkets.ToArray());
            wr.Write(this.UserAssistances.ToArray());
        }

        public object Deserialize(System.IO.Stream stream)
        {
            SerializationReader rd = SerializationReader.GetReader(stream);
            this.IsClientAuthanticated = rd.ReadBoolean();
            this.UserID = rd.ReadInt64();
            this.UserNameA = rd.ReadString();
            this.UserNameE = rd.ReadString();
            this.DMSessionID = rd.ReadString();
            this.UserSessionID = rd.ReadString();
            this.rsAdmin = rd.ReadBoolean();
            this.rsOrderFeed = rd.ReadBoolean();
            this.rsOrder = rd.ReadBoolean();
            this.rsUpdateWithOverride = rd.ReadBoolean();
            this.IsSendFixOrders = rd.ReadBoolean();
            this.IsOnVacation = rd.ReadBoolean();
            this.DefaultCompany = rd.ReadString();
            this.DefaultMarket = rd.ReadInt16();
            this.UserTypes = rd.ReadInt16();
            this.ClientIP = rd.ReadString();
            this.UserCompanies = rd.ReadList();
            this.UserMarkets = rd.ReadList();
            this.UserAssistances = rd.ReadList();

            return this;
        }

        #endregion

    }
}
