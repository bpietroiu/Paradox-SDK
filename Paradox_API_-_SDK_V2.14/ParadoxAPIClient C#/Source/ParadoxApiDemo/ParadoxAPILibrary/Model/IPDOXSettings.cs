using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TIPDOXSettingsXML" name="Settings">
            <published>
              <method name="IPPassword" readonly="False" type="UnicodeString"></method> **
              <method name="ACCFilePath" readonly="False" type="UnicodeString"></method>
              <method name="LogEnabled" readonly="False" type="Boolean">False</method>
              <method name="LogLevel" readonly="False" type="Integer">0</method>
              <method name="LogInterval" readonly="False" type="Integer">0</method>
              <method name="LogFileLifeTime" readonly="False" type="Integer">0</method>
              <method name="LogMaxDiskSize" readonly="False" type="Integer">0</method>
              <method name="LogFileDir" readonly="False" type="UnicodeString"></method>
              <method name="OutputEnabled" readonly="False" type="Boolean">False</method>
              <method name="OutputType" readonly="False" type="Integer">0</method>
              <method name="OutputCOMPort" readonly="False" type="Integer">0</method>
              <method name="OutputBaudRate" readonly="False" type="Integer">0</method>
              <method name="OutputUDP" readonly="False" type="Boolean">False</method>
              <method name="OutputIPPort" readonly="False" type="Integer">0</method>
              <method name="OutputIPAddress" readonly="False" type="UnicodeString"></method>
              <method name="OutputProtocolID" readonly="False" type="Integer">0</method>
              <method name="OutputReceiverNo" readonly="False" type="Integer">0</method>
              <method name="OutputLineNo" readonly="False" type="Integer">0</method>
              <method name="OutputHeaderID" readonly="False" type="Integer">0</method>
              <method name="OutputTrailerID" readonly="False" type="Integer">0</method>
              <method name="OutputAckNack" readonly="False" type="Boolean">False</method>
              <method name="OutputWaitForAck" readonly="False" type="Integer">0</method>
              <method name="OutputTestMessage" readonly="False" type="Boolean">False</method>
              <method name="OutputTestMsgDelay" readonly="False" type="Integer">0</method>
              <method name="OutputForcePartition" readonly="False" type="Boolean">False</method>
              <method name="OutputPartitionNo" readonly="False" type="Integer">0</method>
              <method name="OutputUseMACAddress" readonly="False" type="Boolean">False</method>
              <method name="MonitoringAccountNo" readonly="False" type="UnicodeString"></method>
              <method name="WANId" readonly="False" type="Integer">0</method>
              <method name="WANEnabled" readonly="False" type="Boolean">False</method> ***
              <method name="WANPort" readonly="False" type="Integer">0</method> ***
              <method name="WANAddress" readonly="False" type="UnicodeString"></method> ***
            </published>
          </object>
        </objects>
    */
    
    #endregion
    
    public class IPDOXSettings : BasePanelModel<IPDOXSettings>
    {
        public string IPPassword;
        public string AccountFilePath;
        public bool LogEnabled;
        public Int32 LogLevel;
        public Int32 LogInterval;
        public Int32 LogFileLifeTime;
        public Int32 LogMaxDiskSize;
        public string LogFileDir;
        public bool OutputEnabled;
        public Int32 OutputType;
        public Int32 OutputCOMPort;
        public Int32 OutputBaudRate;
        public bool OutputUDP;
        public Int32 OutputIPPort;
        public string OutputIPAddress;
        public Int32 OutputProtocolID;
        public Int32 OutputReceiverNo;
        public Int32 OutputLineNo;
        public Int32 OutputHeaderID;
        public Int32 OutputTrailerID;
        public bool OutputAckNack;
        public Int32 OutputWaitForAck;
        public bool OutputTestMessage;
        public Int32 OutputTestMsgDelay;
        public bool OutputForcePartition;
        public Int32 OutputPartitionNo;
        public bool OutputUseMACAddress;
        public string MonitoringAccountNo;
        public Int32 WANId;
        public bool WANEnabled;
        public Int32 WANPort;
        public string WANAddress;

        public IPDOXSettings()
        {
            sObjectname = "TIPDOXSettingsXML";
            sName = "Settings";
        }
        
        public bool parseXML(string xmlString)
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

                                if (sname == "IPPassword") IPPassword = svalue.Trim();
                                else if (sname == "ACCFilePath") AccountFilePath = svalue.Trim();

                                else if (sname == "LogEnabled") LogEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "LogLevel") LogLevel = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogInterval") LogInterval = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogFileLifeTime") LogFileLifeTime = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogMaxDiskSize") LogMaxDiskSize = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogFileDir") LogFileDir = svalue.Trim();

                                else if (sname == "OutputEnabled") OutputEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputType") OutputType = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputCOMPort") OutputCOMPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputBaudRate") OutputBaudRate = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputUDP") OutputUDP = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputIPPort") OutputIPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputIPAddress") OutputIPAddress = svalue.Trim();
                                else if (sname == "OutputProtocolID") OutputProtocolID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputReceiverNo") OutputReceiverNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputLineNo") OutputLineNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputHeaderID") OutputHeaderID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputTrailerID") OutputTrailerID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputAckNack") OutputAckNack = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputWaitForAck") OutputWaitForAck = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputTestMessage") OutputTestMessage = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputTestMsgDelay") OutputTestMsgDelay = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputForcePartition") OutputForcePartition = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputPartitionNo") OutputPartitionNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputUseMACAddress") OutputUseMACAddress = Convert.ToBoolean(svalue.Trim());

                                else if (sname == "MonitoringAccountNo") MonitoringAccountNo = svalue.Trim();
                                else if (sname == "WANId") WANId = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WANEnabled") WANEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "WANPort") WANPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WANAddress") WANAddress = svalue.Trim();
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