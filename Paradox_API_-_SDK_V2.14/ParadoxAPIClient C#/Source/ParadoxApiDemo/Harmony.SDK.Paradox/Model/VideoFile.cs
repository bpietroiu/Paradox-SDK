using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*         
     <?xml version="1.0"?>
        <objects>
          <object objectname="TVideoFileXML" name="File1">
            <published>
              <method name="FileID" readonly="True" type="Integer">0</method>
              <method name="FileType" readonly="True" type="UnicodeString"></method>
              <method name="FileName" readonly="True" type="UnicodeString"></method>
              <method name="FilePath" readonly="True" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion  

    public class VideoFile : BasePanelModel<VideoFile>
    {
        public UInt32 FileID;
        public string FileType;
        public string FileName;
        public string FilePath;

        public VideoFile()
        {
            sObjectname = "TVideoFileXML";
            sName = "File";
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
                                if (sname == "FileID") FileID = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "FileType") FileType = svalue.Trim();
                                else if (sname == "FileName") FileName = svalue.Trim();
                                else if (sname == "FilePath") FilePath = svalue.Trim();
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