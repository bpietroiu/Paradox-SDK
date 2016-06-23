using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*         
     <?xml version="1.0"?>
        <objects>
          <object objectname="TVODSettingsXML" name="Settings">
            <published>
              <method name="IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="IPPort" readonly="False" type="Integer">0</method>
              <method name="ServerPassword" readonly="False" type="UnicodeString">paradox</method>
              <method name="UserName" readonly="False" type="UnicodeString">master</method>
              <method name="VideoFormat" readonly="False" type="UnicodeString">360P256K</method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class VODSettings : BasePanelModel<VODSettings>
    {
        public string IPAddress;
        public UInt32 IPPort;
        public string ServerPassword;
        public string UserName;
        public string VideoFormat;

        public VODSettings()
        {
            sObjectname = "TVODSettingsXML";
            sName = "Settings";
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
                                if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "IPPort") IPPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ServerPassword") ServerPassword = svalue.Trim();
                                else if (sname == "UserName") UserName = svalue.Trim();
                                else if (sname == "VideoFormat") VideoFormat = svalue.Trim();
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