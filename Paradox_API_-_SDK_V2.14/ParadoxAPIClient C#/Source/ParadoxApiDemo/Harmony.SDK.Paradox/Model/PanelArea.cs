using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*
        <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelAreaXML" name="Area1">
            <published>
              <method name="AreaNo" readonly="False" type="Integer">0</method>
              <method name="AreaLabel" readonly="False" type="UnicodeString"></method>
              <method name="AreaEnabled" readonly="False" type="Boolean">False</method>
              <method name="AreaExitDelay" readonly="False" type="Integer">0</method>
              <method name="AreaBellCutOffTimer" readonly="False" type="Integer">0</method>
              <method name="AreaAutoArmingTime" readonly="False" type="Double">0</method>
              <method name="AreaNoMovementTimer" readonly="False" type="Integer">0</method>
      
              <method name="AreaEntryDelay1" readonly="False" type="Integer">0</method>
              <method name="AreaEntryDelay2" readonly="False" type="Integer">0</method>
              <method name="AreaSpecialExitDelay" readonly="False" type="Integer">0</method>
              <method name="AreaNoMovementScheduleStartTimeA" readonly="False" type="Double">0</method>
              <method name="AreaNoMovementScheduleEndTimeA" readonly="False" type="Double">0</method>
              <method name="AreaNoMovementScheduleDaysA" readonly="False" type="UnicodeString"></method>
              <method name="AreaNoMovementScheduleStartTimeB" readonly="False" type="Double">0</method>
              <method name="AreaNoMovementScheduleEndTimeB" readonly="False" type="Double">0</method>
              <method name="AreaNoMovementScheduleDaysB" readonly="False" type="UnicodeString"></method>
              <method name="AreaArmingReportScheduleStartTimeA" readonly="False" type="Double">0</method>
              <method name="AreaArmingReportScheduleEndTimeA" readonly="False" type="Double">0</method>
              <method name="AreaArmingReportScheduleDaysA" readonly="False" type="UnicodeString"></method>
              <method name="AreaArmingReportScheduleStartTimeB" readonly="False" type="Double">0</method>
              <method name="AreaArmingReportScheduleEndTimeB" readonly="False" type="Double">0</method>
              <method name="AreaArmingReportScheduleDaysB" readonly="False" type="UnicodeString"></method>
              <method name="AreaDisarmingReportScheduleStartTimeA" readonly="False" type="Double">0</method>
              <method name="AreaDisarmingReportScheduleEndTimeA" readonly="False" type="Double">0</method>
              <method name="AreaDisarmingReportScheduleDaysA" readonly="False" type="UnicodeString"></method>
              <method name="AreaDisarmingReportScheduleStartTimeB" readonly="False" type="Double">0</method>
              <method name="AreaDisarmingReportScheduleEndTimeB" readonly="False" type="Double">0</method>
              <method name="AreaDisarmingReportScheduleDaysB" readonly="False" type="UnicodeString"></method>
              <method name="AreaExitDelayTermination" readonly="False" type="Boolean">False</method>
              <method name="AreaNoExitDelayViaRemote" readonly="False" type="Boolean">False</method>
              <method name="AreaBellSirenEnable" readonly="False" type="Boolean">False</method>
              <method name="AreaSquawkOnAutoArm" readonly="False" type="Boolean">False</method>
              <method name="AreaSquawkOnArm" readonly="False" type="UnicodeString"></method>
              <method name="AreaSquawkOnDelay" readonly="False" type="UnicodeString"></method>
              <method name="AreaRingBackOption" readonly="False" type="UnicodeString"></method>
              <method name="AreaSquawkOnRemoteArm" readonly="False" type="Boolean">False</method>
              <method name="AreaOneTouchRegularArm" readonly="False" type="Boolean">False</method>
              <method name="AreaOneTouchStayArm" readonly="False" type="Boolean">False</method>
              <method name="AreaOneTouchInstantArm" readonly="False" type="Boolean">False</method>
              <method name="AreaOneTouchForceArm" readonly="False" type="Boolean">False</method>
              <method name="AreaOneTouchInstantDisarm" readonly="False" type="Boolean">False</method>
              <method name="AreaBypassProgramming" readonly="False" type="Boolean">False</method>
              <method name="AreaFollowAreas" readonly="False" type="UnicodeString"></method>
              <method name="AreaSwitchToStayIfNoEntry" readonly="False" type="Boolean">False</method>
              <method name="AreaSwitchToForceIfRegularArm" readonly="False" type="Boolean">False</method>
              <method name="AreaSwitchToForceIfStayArm" readonly="False" type="Boolean">False</method>
              <method name="AreaSwitchToEntryDelay2AfterDelay" readonly="False" type="Boolean">False</method>
              <method name="AreaClosingDelinquency" readonly="False" type="Integer">0</method>
              <method name="AreaMaxZoneBypassed" readonly="False" type="Integer">0</method>
              <method name="AreaAutoArmEnabled" readonly="False" type="Boolean">False</method>
              <method name="AreaPostPoneAutoArm" readonly="False" type="Integer">0</method>
              <method name="AreaAutoArmingMethod" readonly="False" type="UnicodeString"></method>
              <method name="AreaNoMovementAutoArmEnabled" readonly="False" type="Boolean">False</method>
              <method name="AreaPanic1" readonly="False" type="UnicodeString"></method>
              <method name="AreaPanic2" readonly="False" type="UnicodeString"></method>
              <method name="AreaPanic3" readonly="False" type="UnicodeString"></method>
              <method name="AreaAutoZoneShutdownCounter" readonly="False" type="Integer">0</method>
              <method name="AreaRecentClosing" readonly="False" type="Integer">0</method>
              <method name="AreaInvalidCodesBeforeLockout" readonly="False" type="Integer">0</method>
              <method name="AreaKeypadLockoutDuration" readonly="False" type="Integer">0</method>
              <method name="AreaRecycleAlarmDelay" readonly="False" type="Integer">0</method>
              <method name="AreaRecycleCount" readonly="False" type="Integer">0</method>
              <method name="AreaPoliceCodeDelay" readonly="False" type="Integer">0</method>
              <method name="AreaPoliceCodeOnZoneClosingOnly" readonly="False" type="Boolean">False</method>      
            </published>
          </object>
        </objects>
    */

    #endregion

    public class PanelArea : BasePanelModel<PanelArea>
    {
        public UInt32 AreaNo;
        public string AreaLabel;
        public bool AreaEnabled;
        public int AreaExitDelay;
        public int AreaBellCutOffTimer;
        public DateTime AreaAutoArmingTime;
        public int AreaNoMovementTimer;
        public string Status;

        //EVO
        public UInt32 AreaEntryDelay1;
        public UInt32 AreaEntryDelay2;
        public UInt32 AreaSpecialExitDelay;
        public DateTime AreaNoMovementScheduleStartTimeA;
        public DateTime AreaNoMovementScheduleEndTimeA;
        public string AreaNoMovementScheduleDaysA;
        public DateTime AreaNoMovementScheduleStartTimeB;
        public DateTime AreaNoMovementScheduleEndTimeB;
        public string AreaNoMovementScheduleDaysB;
        public DateTime AreaArmingReportScheduleStartTimeA;
        public DateTime AreaArmingReportScheduleEndTimeA;
        public string AreaArmingReportScheduleDaysA;
        public DateTime AreaArmingReportScheduleStartTimeB;
        public DateTime AreaArmingReportScheduleEndTimeB;
        public string AreaArmingReportScheduleDaysB;
        public DateTime AreaDisarmingReportScheduleStartTimeA;
        public DateTime AreaDisarmingReportScheduleEndTimeA;
        public string AreaDisarmingReportScheduleDaysA;
        public DateTime AreaDisarmingReportScheduleStartTimeB;
        public DateTime AreaDisarmingReportScheduleEndTimeB;
        public string AreaDisarmingReportScheduleDaysB;
        public bool AreaExitDelayTermination;
        public bool AreaNoExitDelayViaRemote;
        public bool AreaBellSirenEnable;
        public bool AreaSquawkOnAutoArm;
        public string AreaSquawkOnArm;
        public string AreaSquawkOnDelay;
        public string AreaRingBackOption;
        public bool AreaSquawkOnRemoteArm;
        public bool AreaOneTouchRegularArm;
        public bool AreaOneTouchStayArm;
        public bool AreaOneTouchInstantArm;
        public bool AreaOneTouchForceArm;
        public bool AreaOneTouchInstantDisarm;
        public bool AreaBypassProgramming;
        public string AreaFollowAreas;
        public bool AreaSwitchToStayIfNoEntry;
        public bool AreaSwitchToForceIfRegularArm;
        public bool AreaSwitchToForceIfStayArm;
        public bool AreaSwitchToEntryDelay2AfterDelay;
        public UInt32 AreaClosingDelinquency;
        public UInt32 AreaMaxZoneBypassed;
        public bool AreaAutoArmEnabled;
        public UInt32 AreaPostPoneAutoArm;
        public string AreaAutoArmingMethod;
        public bool AreaNoMovementAutoArmEnabled;
        public string AreaPanic1;
        public string AreaPanic2;
        public string AreaPanic3;
        public UInt32 AreaAutoZoneShutdownCounter;
        public UInt32 AreaRecentClosing;
        public UInt32 AreaInvalidCodesBeforeLockout;
        public UInt32 AreaKeypadLockoutDuration;
        public UInt32 AreaRecycleAlarmDelay;
        public UInt32 AreaRecycleCount;
        public UInt32 AreaPoliceCodeDelay;
        public bool AreaPoliceCodeOnZoneClosingOnly;

        public PanelArea()
        {
            ObjectName = "TPanelAreaXML";
            Name = "Area";
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
                                if (sname == "AreaNo") AreaNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaLabel") AreaLabel = svalue.Trim();
                                else if (sname == "AreaEnabled") AreaEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaExitDelay") AreaExitDelay = Convert.ToInt32(svalue.Trim());
                                else if (sname == "AreaBellCutOffTimer") AreaBellCutOffTimer = Convert.ToInt32(svalue.Trim());
                                else if (sname == "AreaAutoArmingTime")
                                {
                                    try
                                    {
                                        AreaAutoArmingTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaAutoArmingTime = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaNoMovementTimer") AreaNoMovementTimer = Convert.ToInt32(svalue.Trim());
                                //EVO
                                else if (sname == "AreaEntryDelay1") AreaEntryDelay1 = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaEntryDelay2") AreaEntryDelay2 = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaSpecialExitDelay") AreaSpecialExitDelay = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaNoMovementScheduleStartTimeA")
                                {
                                    try
                                    {
                                        AreaNoMovementScheduleStartTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaNoMovementScheduleStartTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaNoMovementScheduleEndTimeA")
                                {
                                    try
                                    {
                                        AreaNoMovementScheduleEndTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaNoMovementScheduleEndTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaNoMovementScheduleDaysA") AreaNoMovementScheduleDaysA = svalue.Trim();
                                else if (sname == "AreaNoMovementScheduleStartTimeB")
                                {
                                    try
                                    {
                                        AreaNoMovementScheduleStartTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaNoMovementScheduleStartTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaNoMovementScheduleEndTimeB")
                                {
                                    try
                                    {
                                        AreaNoMovementScheduleEndTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaNoMovementScheduleEndTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaNoMovementScheduleDaysB") AreaNoMovementScheduleDaysB = svalue.Trim();
                                else if (sname == "AreaArmingReportScheduleStartTimeA")
                                {
                                    try
                                    {
                                        AreaArmingReportScheduleStartTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaArmingReportScheduleStartTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaArmingReportScheduleEndTimeA")
                                {
                                    try
                                    {
                                        AreaArmingReportScheduleEndTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaArmingReportScheduleEndTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaArmingReportScheduleDaysA") AreaArmingReportScheduleDaysA = svalue.Trim();
                                else if (sname == "AreaArmingReportScheduleStartTimeB")
                                {
                                    try
                                    {
                                        AreaArmingReportScheduleStartTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaArmingReportScheduleStartTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaArmingReportScheduleEndTimeB")
                                {
                                    try
                                    {
                                        AreaArmingReportScheduleEndTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaArmingReportScheduleEndTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaArmingReportScheduleDaysB") AreaArmingReportScheduleDaysB = svalue.Trim();
                                else if (sname == "AreaDisarmingReportScheduleStartTimeA")
                                {
                                    try
                                    {
                                        AreaDisarmingReportScheduleStartTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaDisarmingReportScheduleStartTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaDisarmingReportScheduleEndTimeA")
                                {
                                    try
                                    {
                                        AreaDisarmingReportScheduleEndTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaDisarmingReportScheduleEndTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaDisarmingReportScheduleDaysA") AreaDisarmingReportScheduleDaysA = svalue.Trim();
                                else if (sname == "AreaDisarmingReportScheduleStartTimeB")
                                {
                                    try
                                    {
                                        AreaDisarmingReportScheduleStartTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaDisarmingReportScheduleStartTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaDisarmingReportScheduleEndTimeB")
                                {
                                    try
                                    {
                                        AreaDisarmingReportScheduleEndTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        AreaDisarmingReportScheduleEndTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "AreaDisarmingReportScheduleDaysB") AreaDisarmingReportScheduleDaysB = svalue.Trim();
                                else if (sname == "AreaExitDelayTermination") AreaExitDelayTermination = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaNoExitDelayViaRemote") AreaNoExitDelayViaRemote = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaBellSirenEnable") AreaBellSirenEnable = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaSquawkOnAutoArm") AreaSquawkOnAutoArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaSquawkOnArm") AreaSquawkOnArm = svalue.Trim();
                                else if (sname == "AreaSquawkOnDelay") AreaSquawkOnDelay = svalue.Trim();
                                else if (sname == "AreaRingBackOption") AreaRingBackOption = svalue.Trim();
                                else if (sname == "AreaSquawkOnRemoteArm") AreaSquawkOnRemoteArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaOneTouchRegularArm") AreaOneTouchRegularArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaOneTouchStayArm") AreaOneTouchStayArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaOneTouchInstantArm") AreaOneTouchInstantArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaOneTouchForceArm") AreaOneTouchForceArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaOneTouchInstantDisarm") AreaOneTouchInstantDisarm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaBypassProgramming") AreaBypassProgramming = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaFollowAreas") AreaFollowAreas = svalue.Trim();
                                else if (sname == "AreaSwitchToStayIfNoEntry") AreaSwitchToStayIfNoEntry = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaSwitchToForceIfRegularArm") AreaSwitchToForceIfRegularArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaSwitchToForceIfStayArm") AreaSwitchToForceIfStayArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaSwitchToEntryDelay2AfterDelay") AreaSwitchToEntryDelay2AfterDelay = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaClosingDelinquency") AreaClosingDelinquency = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaMaxZoneBypassed") AreaMaxZoneBypassed = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaAutoArmEnabled") AreaAutoArmEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaPostPoneAutoArm") AreaPostPoneAutoArm = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaAutoArmingMethod") AreaAutoArmingMethod = svalue.Trim();
                                else if (sname == "AreaNoMovementAutoArmEnabled") AreaNoMovementAutoArmEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "AreaPanic1") AreaPanic1 = svalue.Trim();
                                else if (sname == "AreaPanic2") AreaPanic2 = svalue.Trim();
                                else if (sname == "AreaPanic3") AreaPanic3 = svalue.Trim();
                                else if (sname == "AreaAutoZoneShutdownCounter") AreaAutoZoneShutdownCounter = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaRecentClosing") AreaRecentClosing = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaInvalidCodesBeforeLockout") AreaInvalidCodesBeforeLockout = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaKeypadLockoutDuration") AreaKeypadLockoutDuration = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaRecycleAlarmDelay") AreaRecycleAlarmDelay = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaRecycleCount") AreaRecycleCount = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaPoliceCodeDelay") AreaPoliceCodeDelay = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "AreaPoliceCodeOnZoneClosingOnly") AreaPoliceCodeOnZoneClosingOnly = Convert.ToBoolean(svalue.Trim());
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