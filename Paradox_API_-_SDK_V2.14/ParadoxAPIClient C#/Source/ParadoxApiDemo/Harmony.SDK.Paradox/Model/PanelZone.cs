using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*    
    <?xml version="1.0"?>
    <objects>
        <object objectname="TPanelZoneXML" name="Zone1">
            <published>
                <method name="ZoneNo" readonly="False" type="Integer">0</method>
                <method name="ZoneEnabled" readonly="False" type="Boolean">False</method>
                <method name="ZoneLabel" readonly="False" type="UnicodeString"></method>
                <method name="ZoneSerialNo" readonly="False" type="UnicodeString"></method>
                <method name="ZoneInputNo" readonly="False" type="Integer">0</method>
                <method name="ZonePartition" readonly="False" type="Integer">0</method>
                <method name="ZoneDefinition" readonly="False" type="UnicodeString"></method>
                <method name="ZoneAlarmType" readonly="False" type="UnicodeString"></method>
                <method name="ZoneStay" readonly="False" type="Boolean">False</method>
                <method name="ZoneForce" readonly="False" type="Boolean">False</method>
                <method name="ZoneBypass" readonly="False" type="Boolean">False</method>
                <method name="ZoneAutoShutdown" readonly="False" type="Boolean">False</method>
                <method name="ZoneRFSupervision" readonly="False" type="Boolean">False</method>
                <method name="ZoneIntellizone" readonly="False" type="Boolean">False</method>
                <method name="ZoneDelayBeforeTransmission" readonly="False" type="Boolean">False</method>
                <method name="ZoneTXSerialNo" readonly="False" type="UnicodeString"></method>
                <method name="ZoneTamperFollowGlobalSetting" readonly="False" type="Boolean">False</method>
                <method name="ZoneTamperSupervision" readonly="False" type="UnicodeString"></method>
                <method name="ZoneAntiMaskFollowGlobalSetting" readonly="False" type="Boolean">False</method>
                <method name="ZoneAntiMaskSupervision" readonly="False" type="UnicodeString"></method>
                <method name="ZoneBuzzerAlarmWhenDisarm" readonly="False" type="Boolean">False</method>
                <method name="ZoneBuzzerAlarmReported" readonly="False" type="Boolean">False</method>
            </published>
        </object>
    </objects>
    */

    #endregion

    public class PanelZone : BasePanelModel<PanelZone>
    {
        public UInt32 ZoneNo;
        public bool ZoneEnabled;
        public string ZoneLabel;
        public string ZoneSerialNo;
        public UInt32 ZoneInputNo;
        public UInt32 ZonePartition;
        public string ZoneDefinition;
        public string ZoneAlarmType;
        public bool ZoneStay;
        public bool ZoneForce;
        public bool ZoneBypass;
        public bool ZoneAutoShutdown;
        public bool ZoneRFSupervision;
        public bool ZoneIntellizone;
        public bool ZoneDelayBeforeTransmission;
        //EVO
        public string ZoneTXSerialNo;
        public bool ZoneTamperFollowGlobalSetting;
        public string ZoneTamperSupervision;
        public bool ZoneAntiMaskFollowGlobalSetting;
        public string ZoneAntiMaskSupervision;
        public bool ZoneBuzzerAlarmWhenDisarm;
        public bool ZoneBuzzerAlarmReported;
        public string Status;

        public PanelZone()
        {
            sObjectname = "TPanelZoneXML";
            sName = "Zone";
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
                                if (sname == "ZoneNo") ZoneNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ZoneEnabled") ZoneEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneLabel") ZoneLabel = svalue.Trim();
                                else if (sname == "ZoneSerialNo") ZoneSerialNo = svalue.Trim();
                                else if (sname == "ZoneInputNo") ZoneInputNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ZonePartition") ZonePartition = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ZoneDefinition") ZoneDefinition = svalue.Trim();
                                else if (sname == "ZoneAlarmType") ZoneAlarmType = svalue.Trim();
                                else if (sname == "ZoneStay") ZoneStay = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneForce") ZoneForce = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneBypass") ZoneBypass = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneAutoShutdown") ZoneAutoShutdown = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneRFSupervision") ZoneRFSupervision = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneIntellizone") ZoneIntellizone = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneDelayBeforeTransmission") ZoneDelayBeforeTransmission = Convert.ToBoolean(svalue.Trim());
                                //EVO 
                                else if (sname == "ZoneTXSerialNo") ZoneTXSerialNo = svalue.Trim();
                                else if (sname == "ZoneTamperFollowGlobalSetting") ZoneTamperFollowGlobalSetting = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneTamperSupervision") ZoneTamperSupervision = svalue.Trim();
                                else if (sname == "ZoneAntiMaskFollowGlobalSetting") ZoneAntiMaskFollowGlobalSetting = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneAntiMaskSupervision") ZoneAntiMaskSupervision = svalue.Trim();
                                else if (sname == "ZoneBuzzerAlarmWhenDisarm") ZoneBuzzerAlarmWhenDisarm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ZoneBuzzerAlarmReported") ZoneBuzzerAlarmReported = Convert.ToBoolean(svalue.Trim());
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