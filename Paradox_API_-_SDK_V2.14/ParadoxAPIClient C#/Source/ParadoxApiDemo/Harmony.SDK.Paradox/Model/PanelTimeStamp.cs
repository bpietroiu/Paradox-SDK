using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelTimeStampXML" name="PanelTimeStamp">
            <published>
              <method name="TimeStamp" readonly="False" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class PanelTimeStamp : BasePanelModel<PanelTimeStamp>
    {
        public int TimeStamp;

        public PanelTimeStamp()
        {
            sObjectname = "TPanelTimeStampXML";
            sName = "PanelTimeStamp";
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

                                if (sname == "TimeStamp") TimeStamp = Convert.ToInt32(svalue.Trim());
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