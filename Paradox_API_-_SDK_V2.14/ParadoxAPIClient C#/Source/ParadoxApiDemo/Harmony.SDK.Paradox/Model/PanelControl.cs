using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelControlXML" name="Action">
            <published>
              <method name="Command" readonly="False" type="UnicodeString"></method>
              <method name="Items" readonly="False" type="UnicodeString"></method>
              <method name="Timer" readonly="False" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    [XmlRoot("object")]
    public class PanelControl : BasePanelModel<PanelControl>
    {
        public string Command;
        public string Items;
        public int Timer;

        public PanelControl()
        {
            ObjectName = "TPanelControlXML";
            Name = "Action";
            Command = "";
            Items = "";
            Timer = 0;
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

                                if (sname == "Command") Command = svalue.Trim();
                                else if (sname == "Items") Items = svalue.Trim();
                                else if (sname == "Timer") Timer = Convert.ToInt32(svalue.Trim());
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