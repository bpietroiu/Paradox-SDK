using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelEventXML" name="PanelEvent">
            <published>
              <method name="DateTime" readonly="False" type="Double">42201.4059706366</method>
              <method name="SequenceNo" readonly="False" type="Int64">-1</method>
              <method name="EventDateTime" readonly="False" type="UnicodeString"></method>
              <method name="EventLabel" readonly="False" type="UnicodeString"></method>
              <method name="EventType" readonly="False" type="UnicodeString"></method>
              <method name="EventSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="EventDescription" readonly="False" type="UnicodeString"></method>
              <method name="EventAdditionalInfo" readonly="False" type="UnicodeString"></method>
              <method name="EventUserLabel" readonly="False" type="UnicodeString"></method>
              <method name="EventSequenceNo" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion

    public class PanelEvent : BasePanelModel<PanelEvent>
    {
        public DateTime DateTime;
        public Int64 SequenceNo;
        public string EventDateTime;
        public string EventLabel;
        public string EventType;
        public string EventSerialNo;
        public string EventDescription;
        public string EventAdditionalInfo;
        public string EventUserLabel;
        public string EventSequenceNo;

        public PanelEvent()
        {
            ObjectName = "TPanelEventXML";
            Name = "PanelEvent";
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
                        base.ParseXml(reader);

                        while (reader.ReadToFollowing("method"))
                        {
                            reader.MoveToFirstAttribute();
                            sname = reader.Value;

                            if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "method")
                            {
                                svalue = reader.ReadString();
                                if (sname == "DateTime")
                                {
                                    try
                                    {
                                        DateTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        DateTime = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "SequenceNo") SequenceNo = Convert.ToInt64(svalue.Trim());
                                else if (sname == "EventDateTime") EventDateTime = svalue.Trim();
                                else if (sname == "EventLabel") EventLabel = svalue.Trim();
                                else if (sname == "EventType") EventType = svalue.Trim();
                                else if (sname == "EventSerialNo") EventSerialNo = svalue.Trim();
                                else if (sname == "EventDescription") EventDescription = svalue.Trim();
                                else if (sname == "EventAdditionalInfo") EventAdditionalInfo = svalue.Trim();
                                else if (sname == "EventUserLabel") EventUserLabel = svalue.Trim();
                                else if (sname == "EventSequenceNo") EventSequenceNo = svalue.Trim();
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