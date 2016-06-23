using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelIPReportingXML" name="IPReporting">
            <published>
              <method name="ReceiverNo" readonly="False" type="Integer">0</method>
              <method name="ReportingIPEnabled" readonly="False" type="Boolean">False</method>
              <method name="ReceiverIPPassword" readonly="False" type="UnicodeString"></method>
              <method name="ReceiverIPProfile" readonly="False" type="Integer">0</method>
              <method name="Area1AccountNo" readonly="False" type="UnicodeString"></method>
              <method name="Area2AccountNo" readonly="False" type="UnicodeString"></method>
              <method name="WAN1IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="WAN1IPPort" readonly="False" type="Integer">0</method>
              <method name="WAN2IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="WAN2IPPort" readonly="False" type="Integer">0</method>
              <method name="ParallelReporting" readonly="False" type="Boolean">False</method>
              <method name="ServiceFailureOptions" readonly="False" type="UnicodeString"></method>
              <method name="GPRSAccessPointName" readonly="False" type="UnicodeString"></method>
              <method name="GPRSUserName" readonly="False" type="UnicodeString"></method>
              <method name="GPRSPassword" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class PanelIPReporting : BasePanelModel<PanelIPReporting>
    {
        public Int32 ReceiverNo;
        public bool ReportingIPEnabled;
        public string ReceiverIPPassword;
        public Int32 ReceiverIPProfile;
        public string Area1AccountNo;
        public string Area2AccountNo;
        public string WAN1IPAddress;
        public Int32 WAN1IPPort;
        public string WAN2IPAddress;
        public Int32 WAN2IPPort;
        public bool ParallelReporting;
        public string ServiceFailureOptions;
        public string GPRSAccessPointName;
        public string GPRSUserName;
        public string GPRSPassword;
        public string Status;

        public PanelIPReporting()
        {
            sObjectname = "TPanelIPReportingXML";
            sName = "IPReporting";
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

                                if (sname == "ReceiverNo") ReceiverNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ReportingIPEnabled") ReportingIPEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ReceiverIPPassword") ReceiverIPPassword = svalue.Trim();
                                else if (sname == "ReceiverIPProfile") ReceiverIPProfile = Convert.ToInt32(svalue.Trim());
                                else if (sname == "Area1AccountNo") Area1AccountNo = svalue.Trim();
                                else if (sname == "Area2AccountNo") Area2AccountNo = svalue.Trim();
                                else if (sname == "WAN1IPAddress") WAN1IPAddress = svalue.Trim();
                                else if (sname == "WAN1IPPort") WAN1IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WAN2IPAddress") WAN2IPAddress = svalue.Trim();
                                else if (sname == "WAN2IPPort") WAN2IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ParallelReporting") ParallelReporting = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ServiceFailureOptions") ServiceFailureOptions = svalue.Trim();
                                else if (sname == "GPRSAccessPointName") GPRSAccessPointName = svalue.Trim();
                                else if (sname == "GPRSUserName") GPRSUserName = svalue.Trim();
                                else if (sname == "GPRSPassword") GPRSPassword = svalue.Trim();
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