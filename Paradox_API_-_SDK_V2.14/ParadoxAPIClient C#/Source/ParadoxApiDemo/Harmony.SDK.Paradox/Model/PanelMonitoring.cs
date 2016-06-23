using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*                
        <?xml version="1.0"?>
         <PanelInfo>
            <ZoneStatus Zone3="Opened,Alarm In Memory"/>
        </PanelInfo>
    */

    #endregion

    public class PanelMonitoring : BasePanelModel<PanelMonitoring>
    {
        public const string C_MONITORING_ZONE_ITEM_TYPE = "Zone";
        public const string C_MONITORING_AREA_ITEM_TYPE = "Area";
        public const string C_MONITORING_DOOR_ITEM_TYPE = "Door";
        public const string C_MONITORING_PGM_ITEM_TYPE = "PGM";

        public UInt32 ItemNo;
        public string ItemType;
        public string Status;

        public PanelMonitoring()
        {
            ObjectName = "PanelInfo";
            Name = "Status";
        }
        
        protected internal bool parseXML(string xmlString)
        {
            string sType, sname, svalue, sItemNo;
            if (xmlString != null)
            {
                using (var reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    try
                    {
                        reader.ReadToFollowing("PanelInfo");
                        reader.Read();
                        reader.Read();
                        sType = reader.LocalName;
                        reader.MoveToFirstAttribute();
                        sname = reader.Name;
                        svalue = reader.Value;

                        if (sType == "ZoneStatus")
                        {
                            if (sname.Contains("Zone"))
                            {
                                sItemNo = sname.Remove(0, 4);
                                ItemNo = Convert.ToUInt32(sItemNo);
                                ItemType = C_MONITORING_ZONE_ITEM_TYPE;
                                Status = svalue;
                            }
                        }
                        else if (sType == "AreaStatus")
                        {
                            if (sname.Contains("Area"))
                            {
                                sItemNo = sname.Remove(0, 4);
                                ItemNo = Convert.ToUInt32(sItemNo);
                                ItemType = C_MONITORING_AREA_ITEM_TYPE;
                                Status = svalue;
                            }
                        }
                        else if (sType == "DoorStatus")
                        {
                            if (sname.Contains("Door"))
                            {
                                sItemNo = sname.Remove(0, 4);
                                ItemNo = Convert.ToUInt32(sItemNo);
                                ItemType = C_MONITORING_DOOR_ITEM_TYPE;
                                Status = svalue;
                            }
                        }
                        else if (sType == "PGMStatus")
                        {
                            if (sname.Contains("PGM"))
                            {
                                sItemNo = sname.Remove(0, 3);
                                ItemNo = Convert.ToUInt32(sItemNo);
                                ItemType = C_MONITORING_PGM_ITEM_TYPE;
                                Status = svalue;
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