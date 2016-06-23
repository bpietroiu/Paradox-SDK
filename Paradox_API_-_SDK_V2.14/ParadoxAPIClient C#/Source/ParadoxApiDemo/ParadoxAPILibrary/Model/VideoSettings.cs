using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TVideoSettingsXML" name="VideoSettings">
            <published>
              <method name="VideoFileDir" readonly="False" type="UnicodeString"></method>
              <method name="VideoFileLifeTime" readonly="False" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class VideoSettings : BasePanelModel<VideoSettings>
    {
        public string VideoFileDir;
        public UInt32 VideoFileLifeTime; //Days        

        public VideoSettings()
        {
            sObjectname = "TVideoSettingsXML";
            sName = "VideoSettings";
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

                                if (sname == "VideoFileDir") VideoFileDir = svalue.Trim();
                                else if (sname == "VideoFileLifeTime") VideoFileLifeTime = Convert.ToUInt32(svalue.Trim());
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