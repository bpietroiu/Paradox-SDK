using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
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
        public int LogLevel;
        public int LogInterval;
        public int LogFileLifeTime;
        public int LogMaxDiskSize;
        public string LogFileDir;
        public bool OutputEnabled;
        public int OutputType;
        public int OutputCOMPort;
        public int OutputBaudRate;
        public bool OutputUDP;
        public int OutputIPPort;
        public string OutputIPAddress;
        public int OutputProtocolID;
        public int OutputReceiverNo;
        public int OutputLineNo;
        public int OutputHeaderID;
        public int OutputTrailerID;
        public bool OutputAckNack;
        public int OutputWaitForAck;
        public bool OutputTestMessage;
        public int OutputTestMsgDelay;
        public bool OutputForcePartition;
        public int OutputPartitionNo;
        public bool OutputUseMACAddress;
        public string MonitoringAccountNo;
        public int WANId;
        public bool WANEnabled;
        public int WANPort;
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