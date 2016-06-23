using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelSettingsXML" name="Settings">
            <published>
              <method name="PanelType" readonly="False" type="UnicodeString"></method>
              <method name="ComType" readonly="False" type="UnicodeString"></method>
              <method name="SiteID" readonly="False" type="UnicodeString"></method>
              <method name="SerialNo" readonly="False" type="UnicodeString"></method>
              <method name="IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="IPPort" readonly="False" type="Integer">0</method>
              <method name="ComPort" readonly="False" type="Integer">0</method>
              <method name="BaudRate" readonly="False" type="Integer">0</method>
              <method name="SMSCallback" readonly="False" type="Boolean">False</method>
              <method name="IPPassword" readonly="False" type="UnicodeString"></method>
              <method name="UserCode" readonly="False" type="UnicodeString"></method>
              <method name="SystemAlarmLanguage" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion

    public class PanelSettings : BasePanelModel<PanelSettings>
    {
        public string PanelType;
        public string ComType;
        public string SiteID;
        public string SerialNo;
        public string IPAddress;
        public int IPPort;
        public int ComPort;
        public int BaudRate;
        public bool SMSCallback;
        public string IPPassword;
        public string UserCode;
        public string SystemAlarmLanguage;

        public PanelSettings()
        {
            sObjectname = "TPanelSettingsXML";
            sName = "Settings";
            IPPort = 10000;
            IPPassword = "paradox";
            UserCode = "1234";
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

                                if (sname == "PanelType") PanelType = svalue.Trim();
                                else if (sname == "ComType") ComType = svalue.Trim();
                                else if (sname == "SiteID") SiteID = svalue.Trim();
                                else if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "IPPort") IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ComPort") ComPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "BaudRate") BaudRate = Convert.ToInt32(svalue.Trim());
                                else if (sname == "SMSCallback") SMSCallback = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "IPPassword") IPPassword = svalue.Trim();
                                else if (sname == "UserCode") UserCode = svalue.Trim();
                                else if (sname == "SystemAlarmLanguage") SystemAlarmLanguage = svalue.Trim();
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

        public bool LoadFromXmlString(string xmlString)
        {
            return parseXML(xmlString);
        }
    }
}