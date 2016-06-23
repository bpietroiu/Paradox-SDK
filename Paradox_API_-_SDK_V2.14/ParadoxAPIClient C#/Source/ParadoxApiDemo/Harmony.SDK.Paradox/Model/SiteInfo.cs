using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TSiteInfoXML" name="Item1">
            <published>
              <method name="SerialNo" readonly="True" type="UnicodeString"></method>
              <method name="ItemType" readonly="True" type="UnicodeString"></method>
              <method name="IPAddress" readonly="True" type="UnicodeString"></method>
              <method name="HTTPPort" readonly="True" type="Integer">0</method>
              <method name="HTTPSPort" readonly="True" type="Integer">0</method>
              <method name="WebPort" readonly="True" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */

    #endregion

    public class SiteInfo : BasePanelModel<SiteInfo>
    {
        public string SerialNo;
        public string ItemType;
        public string IPAddress;
        public UInt32 HTTPPort;
        public UInt32 HTTPSPort;
        public UInt32 WebPort;

        public SiteInfo()
        {
            sObjectname = "TSiteInfoXML";
            sName = "Item";
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

                                if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "ItemType") ItemType = svalue.Trim();
                                else if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "HTTPPort") HTTPPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "HTTPSPort") HTTPSPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "WebPort") WebPort = Convert.ToUInt32(svalue.Trim());
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