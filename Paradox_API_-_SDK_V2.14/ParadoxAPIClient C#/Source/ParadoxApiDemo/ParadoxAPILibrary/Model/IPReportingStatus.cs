using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingStatusXML" name="IPReportingStatus">
            <published>
              <method name="Registered" readonly="False" type="Boolean">False</method>
              <method name="RegistrationStatus" readonly="False" type="UnicodeString"></method>
              <method name="RegistrationError" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion

    public class IPReportingStatus : CustomXmlParser<IPReportingStatus>
    {
        public bool Registered;
        public string RegistrationStatus;
        public string RegistrationError;

        public IPReportingStatus()
        {
            sObjectname = "TPanelReportingStatusXML";
            sName = "IPReportingStatus";
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

                                if (sname == "Registered") Registered = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "RegistrationStatus") RegistrationStatus = svalue.Trim();
                                else if (sname == "RegistrationError") RegistrationError = svalue.Trim();
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