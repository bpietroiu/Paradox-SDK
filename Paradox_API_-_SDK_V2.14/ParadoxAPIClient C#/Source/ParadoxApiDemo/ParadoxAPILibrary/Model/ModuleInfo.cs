using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*
       <?xml version="1.0"?>
        <objects>
          <object objectname="TModuleInfoXML" name="Module1">
            <published>
              <method name="MacString" readonly="False" type="UnicodeString"></method>
              <method name="NetMaskString" readonly="False" type="UnicodeString"></method>
              <method name="DHCPString" readonly="False" type="UnicodeString"></method>
              <method name="TypeString" readonly="False" type="UnicodeString"></method>
              <method name="IPString" readonly="False" type="UnicodeString"></method>
              <method name="SiteNameString" readonly="False" type="UnicodeString"></method>
              <method name="SiteIDString" readonly="False" type="UnicodeString"></method>
              <method name="VersionString" readonly="False" type="UnicodeString"></method>
              <method name="IPPortString" readonly="False" type="UnicodeString"></method>
              <method name="WebPortString" readonly="False" type="UnicodeString"></method>
              <method name="LanguageString" readonly="False" type="UnicodeString"></method>
              <method name="SerialNoString" readonly="False" type="UnicodeString"></method>
              <method name="HTTPSPortString" readonly="False" type="UnicodeString"></method>
              <method name="DiscoverOnLAN" readonly="False" type="Boolean">False</method>
              <method name="UseHTTPSString" readonly="False" type="UnicodeString"></method>
              <method name="RegisteredToPMH" readonly="False" type="Boolean">False</method>
              <method name="NetworkInterfaceIpAddress" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class ModuleInfo : CustomXmlParser<ModuleInfo>
    {
        public string MacString;
        public string NetMaskString;
        public string DHCPString;
        public string TypeString;
        public string IPString;
        public string SiteNameString;
        public string SiteIDString;
        public string VersionString;
        public string IPPortString;
        public string WebPortString;
        public string LanguageString;
        public string SerialNoString;
        public string HTTPSPortString;
        public bool DiscoverOnLAN;
        public string UseHTTPSString;
        public bool RegisteredToPMH;
        public string NetworkInterfaceIpAddress;
        public bool PGMEnabled;

        public ModuleInfo()
        {
            sObjectname = "TModuleInfoXML";
            sName = "ModuleInfo";
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
                                if (sname == "MacString") MacString = svalue.Trim();
                                else if (sname == "NetMaskString") NetMaskString = svalue.Trim();
                                else if (sname == "DHCPString") DHCPString = svalue.Trim();
                                else if (sname == "TypeString") TypeString = svalue.Trim();
                                else if (sname == "IPString") IPString = svalue.Trim();
                                else if (sname == "SiteNameString") SiteNameString = svalue.Trim();
                                else if (sname == "SiteIDString") SiteIDString = svalue.Trim();
                                else if (sname == "VersionString") VersionString = svalue.Trim();
                                else if (sname == "IPPortString") IPPortString = svalue.Trim();
                                else if (sname == "WebPortString") WebPortString = svalue.Trim();
                                else if (sname == "LanguageString") LanguageString = svalue.Trim();
                                else if (sname == "SerialNoString") SerialNoString = svalue.Trim();
                                else if (sname == "HTTPSPortString") HTTPSPortString = svalue.Trim();
                                else if (sname == "DiscoverOnLAN") DiscoverOnLAN = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UseHTTPSString") UseHTTPSString = svalue.Trim();
                                else if (sname == "RegisteredToPMH") RegisteredToPMH = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "NetworkInterfaceIpAddress") NetworkInterfaceIpAddress = svalue.Trim();
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