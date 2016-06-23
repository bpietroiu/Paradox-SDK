using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*         
        <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelAccessLevelXML" name="AccessLevel1">
            <published>
              <method name="AccessLevelNo" readonly="False" type="Integer">0</method>
              <method name="AccessLevelLabel" readonly="False" type="UnicodeString"></method>
              <method name="AccessLevelDoors" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class PanelAccessLevel : BasePanelModel<PanelAccessLevel>
    {
        protected string sAccessLevelDoors;

        public List<int> Doors { get; set; }

        public UInt32 AccessLevelNo;
        public string AccessLevelLabel;

        public string AccessLevelDoors
        {
            get { return sAccessLevelDoors; }
            set
            {
                sAccessLevelDoors = value;
                string[] strValues = sAccessLevelDoors.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var strValue in strValues)
                {
                    int doorNo = Convert.ToInt32(strValue);
                    Doors.Add(doorNo);
                }
            }
        }

        public PanelAccessLevel()
        {
            sObjectname = "TPanelAccessLevelXML";
            sName = "AccessLevel";
            Doors = new List<int>();
        }

        ~PanelAccessLevel()
        {
            Doors.Clear();
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
                                if (sname == "AccessLevelNo") AccessLevelNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AccessLevelLabel") AccessLevelLabel = svalue.Trim();
                                else if (sname == "AccessLevelDoors") AccessLevelDoors = svalue.Trim();
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