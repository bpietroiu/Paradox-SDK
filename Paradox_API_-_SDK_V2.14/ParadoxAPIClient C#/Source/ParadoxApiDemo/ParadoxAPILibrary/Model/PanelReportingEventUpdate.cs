using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingEventUpdateXML" name="PanelReportingEventUpdate">
            <published>
              <method name="EventID" readonly="False" type="Integer">0</method>
              <method name="EventStatus" readonly="False" type="UnicodeString"></method>
              <method name="EventDateTime" readonly="False" type="Double">0</method>
            </published>
          </object>
        </objects>
    */
    #endregion
    
    public class PanelReportingEventUpdate : BasePanelModel<PanelReportingEventUpdate>
    {
        public Int32 EventID;
        public string EventStatus;
        public DateTime EventDateTime;

        public PanelReportingEventUpdate()
        {
            sObjectname = "TPanelReportingEventUpdateXML";
            sName = "PanelReportingEventUpdate";
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
                                else if (sname == "EventStatus") EventStatus = svalue.Trim();
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