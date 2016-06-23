using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelInfoXML" name="PanelInfo">
            <published>
              <method name="SiteName" readonly="False" type="UnicodeString"></method>
              <method name="MediaType" readonly="False" type="UnicodeString"></method>
              <method name="ProductID" readonly="False" type="Integer">0</method>
              <method name="Description" readonly="False" type="UnicodeString"></method>
              <method name="SerialNo" readonly="False" type="UnicodeString"></method>
              <method name="Version" readonly="False" type="UnicodeString"></method>
              <method name="SiteID" readonly="False" type="UnicodeString"></method>
              <method name="IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="IPPort" readonly="False" type="Integer">0</method>
              <method name="WebPort" readonly="False" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class PanelInfo : BasePanelModel<PanelInfo>
    {
        public string SiteName;
        public string MediaType;
        public Int32 ProductID;
        public string Description;
        public string SerialNo;
        public string SiteID;
        public string Version;
        public string IPAddress;
        public Int32 IPPort;
        public Int32 WebPort;

        public PanelInfo()
        {
            sObjectname = "TPanelInfoXML";
            sName = "PanelInfo";
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

                                if (sname == "SiteName") SiteName = svalue.Trim();
                                else if (sname == "MediaType") MediaType = svalue.Trim();
                                else if (sname == "ProductID") ProductID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "Description") Description = svalue.Trim();
                                else if (sname == "SiteID") Description = svalue.Trim();
                                else if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "Version") SerialNo = svalue.Trim();
                                else if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "IPPort") IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WebPort") WebPort = Convert.ToInt32(svalue.Trim());

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