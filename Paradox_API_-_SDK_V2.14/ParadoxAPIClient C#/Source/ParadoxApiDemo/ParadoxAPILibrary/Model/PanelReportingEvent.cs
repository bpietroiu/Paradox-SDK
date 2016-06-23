using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingEventXML" name="PanelReportingEvent">
            <published>
              <method name="EventID" readonly="False" type="Integer">0</method>
              <method name="EventAccountNo" readonly="False" type="UnicodeString"></method>
              <method name="EventDateTime" readonly="False" type="Double">0</method>
              <method name="EventProtocolID" readonly="False" type="UnicodeString"></method>
              <method name="EventCode" readonly="False" type="UnicodeString"></method>
              <method name="EventDescription" readonly="False" type="UnicodeString"></method>
              <method name="EventAreaDoorNo" readonly="False" type="UnicodeString"></method>
              <method name="EventZoneUserNo" readonly="False" type="UnicodeString"></method>
              <method name="EventMACAddress" readonly="False" type="UnicodeString"></method>
              <method name="EventStatus" readonly="False" type="UnicodeString"></method>
              <method name="VODIPPort" readonly="True" type="Integer">0</method>
              <method name="VODSessionKey" readonly="True" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion

    public class PanelReportingEvent : BasePanelModel<PanelReportingEvent>
    {
        public Int32 EventID;
        public string EventAccountNo;
        public DateTime EventDateTime;
        public string EventProtocolID;
        public string EventCode;
        public string EventDescription;
        public string EventAreaDoorNo;
        public string EventZoneUserNo;
        public string EventMACAddress;
        public string EventStatus;
        public UInt32 VODIPPort;
        public string VODSessionKey;

        public PanelReportingEvent()
        {
            sObjectname = "TPanelReportingEventXML";
            sName = "PanelReportingEvent";
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

                                if (sname == "EventID") EventID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "EventAccountNo") EventAccountNo = svalue.Trim();
                                else if (sname == "EventDateTime")
                                {
                                    try
                                    {
                                        EventDateTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        EventDateTime = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "EventProtocolID") EventProtocolID = svalue.Trim();
                                else if (sname == "EventCode") EventCode = svalue.Trim();
                                else if (sname == "EventDescription") EventDescription = svalue.Trim();
                                else if (sname == "EventAreaDoorNo") EventAreaDoorNo = svalue.Trim();
                                else if (sname == "EventZoneUserNo") EventZoneUserNo = svalue.Trim();
                                else if (sname == "EventMACAddress") EventMACAddress = svalue.Trim();
                                else if (sname == "EventStatus") EventStatus = svalue.Trim();
                                else if (sname == "VODIPPort") VODIPPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "VODSessionKey") VODSessionKey = svalue.Trim();
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