using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*
    <?xml version="1.0"?>
    <objects>
        <object objectname="TPanelDoorXML" name="Door1">
        <published>
            <method name="DoorNo" readonly="False" type="Integer">0</method>
            <method name="DoorLabel" readonly="False" type="UnicodeString"></method>
            <method name="DoorEnabled" readonly="False" type="Boolean">False</method>
            <method name="DoorSerialNo" readonly="False" type="UnicodeString"></method>
            <method name="DoorAreaAssignment" readonly="False" type="UnicodeString"></method>
            <method name="DoorAccessEnabled" readonly="False" type="Boolean">False</method>
            <method name="DoorAccessOption" readonly="False" type="UnicodeString"></method>
            <method name="DoorAccessCodeOnKeypad" readonly="False" type="Boolean">False</method>
            <method name="DoorAccessCardAndCode" readonly="False" type="Boolean">False</method>
            <method name="DoorAccessArmRestricted" readonly="False" type="Boolean">False</method>
            <method name="DoorAccessDisarmRestricted" readonly="False" type="Boolean">False</method>
            <method name="DoorBurglaryAlarmOnForced" readonly="False" type="Boolean">False</method>
            <method name="DoorSkipDelayOnArmWithCard" readonly="False" type="Boolean">False</method>
            <method name="DoorBurglaryAlarmOnLeftOpen" readonly="False" type="Boolean">False</method>
            <method name="DoorMasterOnlyOnClockLost" readonly="False" type="Boolean">False</method>
            <method name="DoorEntryToleranceWindow" readonly="False" type="Integer">0</method>
            <method name="DoorReportOnRequestToExit" readonly="False" type="Boolean">False</method>
            <method name="DoorReportOnDoorCommandFromPC" readonly="False" type="Boolean">False</method>
            <method name="DoorReportOnUserAccessDenied" readonly="False" type="Boolean">False</method>
            <method name="DoorReportOnUserAccessGranted" readonly="False" type="Boolean">False</method>
            <method name="DoorReportOnLeftOpen" readonly="False" type="Boolean">False</method>
            <method name="DoorReportOnFocedOpen" readonly="False" type="Boolean">False</method>
            <method name="DoorUnlockScheduleStartTimeA" readonly="False" type="Double">0</method>
            <method name="DoorUnlockScheduleEndTimeA" readonly="False" type="Double">0</method>
            <method name="DoorUnlockScheduleDaysA" readonly="False" type="UnicodeString"></method>
            <method name="DoorUnlockScheduleStartTimeB" readonly="False" type="Double">0</method>
            <method name="DoorUnlockScheduleEndTimeB" readonly="False" type="Double">0</method>
            <method name="DoorUnlockScheduleDaysB" readonly="False" type="UnicodeString"></method>
            <method name="DoorSafeModeEnabled" readonly="False" type="Boolean">False</method>
            <method name="DoorSafeModeCard1" readonly="False" type="UnicodeString"></method>
            <method name="DoorSafeModeCard2" readonly="False" type="UnicodeString"></method>
            <method name="DoorSafeModeCard3" readonly="False" type="UnicodeString"></method>
            <method name="DoorSafeModeCard4" readonly="False" type="UnicodeString"></method>
            <method name="DoorCardActivatesUnlockedSchedule" readonly="False" type="Boolean">False</method>
            <method name="DoorUnlockDoorOnFireAlarm" readonly="False" type="Boolean">False</method>
            <method name="DoorUnlockOnRequestForExit" readonly="False" type="Boolean">False</method>
        </published>
        </object>
    </objects>
    */

    #endregion
    
    public class PanelDoor : BasePanelModel<PanelDoor>
    {
        public UInt32 DoorNo;
        public string DoorLabel;
        public bool DoorEnabled;
        public string DoorSerialNo;
        public string DoorAreaAssignment;
        public bool DoorAccessEnabled;
        public string DoorAccessOption;
        public bool DoorAccessCodeOnKeypad;
        public bool DoorAccessCardAndCode;
        public bool DoorAccessArmRestricted;
        public bool DoorAccessDisarmRestricted;
        public bool DoorBurglaryAlarmOnForced;
        public bool DoorSkipDelayOnArmWithCard;
        public bool DoorBurglaryAlarmOnLeftOpen;
        public bool DoorMasterOnlyOnClockLost;
        public Int32 DoorEntryToleranceWindow;
        public bool DoorReportOnRequestToExit;
        public bool DoorReportOnDoorCommandFromPC;
        public bool DoorReportOnUserAccessDenied;
        public bool DoorReportOnUserAccessGranted;
        public bool DoorReportOnLeftOpen;
        public bool DoorReportOnFocedOpen;
        public DateTime DoorUnlockScheduleStartTimeA;
        public DateTime DoorUnlockScheduleEndTimeA;
        public string DoorUnlockScheduleDaysA;
        public DateTime DoorUnlockScheduleStartTimeB;
        public DateTime DoorUnlockScheduleEndTimeB;
        public string DoorUnlockScheduleDaysB;
        public bool DoorSafeModeEnabled;
        public string DoorSafeModeCard1;
        public string DoorSafeModeCard2;
        public string DoorSafeModeCard3;
        public string DoorSafeModeCard4;
        public bool DoorCardActivatesUnlockedSchedule;
        public bool DoorUnlockDoorOnFireAlarm;
        public bool DoorUnlockOnRequestForExit;
        public string Status;

        public PanelDoor()
        {
            sObjectname = "TPanelDoorXML";
            sName = "Door";
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
                                if (sname == "DoorNo") DoorNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "DoorLabel") DoorLabel = svalue.Trim();
                                else if (sname == "DoorEnabled") DoorEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorSerialNo") DoorSerialNo = svalue.Trim();
                                else if (sname == "DoorAreaAssignment") DoorAreaAssignment = svalue.Trim();
                                else if (sname == "DoorAccessEnabled") DoorAccessEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorAccessOption") DoorAccessOption = svalue.Trim();
                                else if (sname == "DoorAccessCodeOnKeypad") DoorAccessCodeOnKeypad = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorAccessCardAndCode") DoorAccessCardAndCode = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorAccessArmRestricted") DoorAccessArmRestricted = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorAccessDisarmRestricted") DoorAccessDisarmRestricted = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorBurglaryAlarmOnForced") DoorBurglaryAlarmOnForced = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorSkipDelayOnArmWithCard") DoorSkipDelayOnArmWithCard = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorBurglaryAlarmOnLeftOpen") DoorBurglaryAlarmOnLeftOpen = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorMasterOnlyOnClockLost") DoorMasterOnlyOnClockLost = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorEntryToleranceWindow") DoorEntryToleranceWindow = Convert.ToInt32(svalue.Trim());
                                else if (sname == "DoorReportOnRequestToExit") DoorReportOnRequestToExit = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorReportOnDoorCommandFromPC") DoorReportOnDoorCommandFromPC = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorReportOnUserAccessDenied") DoorReportOnUserAccessDenied = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorReportOnUserAccessGranted") DoorReportOnUserAccessGranted = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorReportOnLeftOpen") DoorReportOnLeftOpen = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorReportOnFocedOpen") DoorReportOnFocedOpen = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorUnlockScheduleStartTimeA")
                                {
                                    try
                                    {
                                        DoorUnlockScheduleStartTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        DoorUnlockScheduleStartTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "DoorUnlockScheduleEndTimeA")
                                {
                                    try
                                    {
                                        DoorUnlockScheduleEndTimeA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        DoorUnlockScheduleEndTimeA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "DoorUnlockScheduleDaysA") DoorUnlockScheduleDaysA = svalue.Trim();
                                else if (sname == "DoorUnlockScheduleStartTimeB")
                                {
                                    try
                                    {
                                        DoorUnlockScheduleStartTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        DoorUnlockScheduleStartTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "DoorUnlockScheduleEndTimeB")
                                {
                                    try
                                    {
                                        DoorUnlockScheduleEndTimeB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
                                    {
                                        DoorUnlockScheduleEndTimeB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "DoorUnlockScheduleDaysB") DoorUnlockScheduleDaysB = svalue.Trim();
                                else if (sname == "DoorSafeModeEnabled") DoorSafeModeEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorSafeModeCard1") DoorSafeModeCard1 = svalue.Trim();
                                else if (sname == "DoorSafeModeCard2") DoorSafeModeCard2 = svalue.Trim();
                                else if (sname == "DoorSafeModeCard3") DoorSafeModeCard3 = svalue.Trim();
                                else if (sname == "DoorSafeModeCard4") DoorSafeModeCard4 = svalue.Trim();
                                else if (sname == "DoorCardActivatesUnlockedSchedule") DoorCardActivatesUnlockedSchedule = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorUnlockDoorOnFireAlarm") DoorUnlockDoorOnFireAlarm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "DoorUnlockOnRequestForExit") DoorUnlockOnRequestForExit = Convert.ToBoolean(svalue.Trim());
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