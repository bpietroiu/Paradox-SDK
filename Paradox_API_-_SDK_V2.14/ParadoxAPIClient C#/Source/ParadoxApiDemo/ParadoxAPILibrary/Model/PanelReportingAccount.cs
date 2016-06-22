using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingAccountXML" name="PanelReportingAccount">
            <published>
              <method name="AccountNo" readonly="False" type="UnicodeString"></method>
              <method name="AccountStatus" readonly="False" type="UnicodeString"></method>
              <method name="MACAddress" readonly="False" type="UnicodeString"></method>
              <method name="ProfileID" readonly="False" type="Integer">0</method>
              <method name="ProtocolID" readonly="False" type="UnicodeString"></method>
              <method name="PanelType" readonly="False" type="UnicodeString"></method>
              <method name="PanelSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="PanelVersion" readonly="False" type="UnicodeString"></method>
              <method name="ModuleType" readonly="False" type="UnicodeString"></method>
              <method name="ModuleSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="ModuleVersion" readonly="False" type="UnicodeString"></method>
              <method name="RegistrationDate" readonly="False" type="Double">0</method>
              <method name="LastIPAddress" readonly="False" type="UnicodeString"></method>
              <method name="LastPollingTime" readonly="False" type="Double">0</method>
            </published>
          </object>
        </objects>
    */

    #endregion

    public class PanelReportingAccount : CustomXmlParser<PanelReportingAccount>
    {
        public string AccountNo;
        public string AccountStatus;
        public string MACAddress;
        public Int32 ProfileID;
        public string ProtocolID;
        public string PanelType;
        public string PanelSerialNo;
        public string PanelVersion;
        public string ModuleType;
        public string ModuleSerialNo;
        public string ModuleVersion;
        public DateTime RegistrationDate;
        public string LastIPAddress;
        public DateTime LastPollingTime;

        public PanelReportingAccount()
        {
            sObjectname = "TPanelReportingAccountXML";
            sName = "PanelReportingAccount";
        }
        
        protected internal bool parseXML(string xmlString)
        {
            string sname, svalue;
            if (xmlString != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    try
                    {
                        reader.ReadToFollowing("object");
                        base.parseXML(reader);

                        while (reader.ReadToFollowing("method"))
                        {
                            reader.MoveToFirstAttribute();
                            sname = reader.Value;

                            if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "method")
                            {
                                svalue = reader.ReadString();

                                if (sname == "AccountNo") AccountNo = svalue.Trim();
                                else if (sname == "AccountStatus") AccountStatus = svalue.Trim();
                                else if (sname == "MACAddress") MACAddress = svalue.Trim();
                                else if (sname == "ProfileID") ProfileID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ProtocolID") ProtocolID = svalue.Trim();
                                else if (sname == "PanelType") PanelType = svalue.Trim();
                                else if (sname == "PanelSerialNo") PanelSerialNo = svalue.Trim();
                                else if (sname == "PanelVersion") PanelVersion = svalue.Trim();
                                else if (sname == "ModuleType") ModuleType = svalue.Trim();
                                else if (sname == "ModuleSerialNo") ModuleSerialNo = svalue.Trim();
                                else if (sname == "ModuleVersion") ModuleVersion = svalue.Trim();
                                else if (sname == "RegistrationDate")
                                {
                                    try
                                    {
                                        RegistrationDate = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        RegistrationDate = System.DateTime.FromOADate(0.0);
                                    }

                                }
                                else if (sname == "LastIPAddress") LastIPAddress = svalue.Trim();
                                else if (sname == "LastPollingTime")
                                {
                                    try
                                    {
                                        LastPollingTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        LastPollingTime = System.DateTime.FromOADate(0.0);
                                    }
                                }
                            }
                        }

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}