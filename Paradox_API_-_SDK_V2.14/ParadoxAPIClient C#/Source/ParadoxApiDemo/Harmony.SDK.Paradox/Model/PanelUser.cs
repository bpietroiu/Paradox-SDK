using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*
    <? xml version="1.0"?>
    <objects>
        <object objectname = "TPanelUserXML" name="User1">
            <published>
                <method name = "UserNo" readonly="False" type="Integer">0</method>
                <method name = "UserName" readonly="False" type="UnicodeString"></method>
                <method name = "UserUsed" readonly="False" type="Boolean">False</method>
                <method name = "UserCode" readonly="False" type="UnicodeString"></method>
                <method name = "UserCard" readonly="False" type="UnicodeString"></method>
                <method name = "UserRemoteSerialNo" readonly="False" type="UnicodeString"></method>
                <method name = "UserPartitionAccess" readonly="False" type="UnicodeString"></method>
                <method name = "UserCanBypass" readonly="False" type="Boolean">False</method>
                <method name = "UserCanStaySleepArm" readonly="False" type="Boolean">False</method>
                <method name = "UserCanForceArm" readonly="False" type="Boolean">False</method>
                <method name = "UserCanArmOnly" readonly="False" type="Boolean">False</method>
                <method name = "UserCanActivationPGMOnly" readonly="False" type="Boolean">False</method>
                <method name = "UserCanDuress" readonly="False" type="Boolean">False</method>
                <method name = "UserType" readonly="False" type="UnicodeString"></method>
                <method name = "UserAccessControlEnabled" readonly="False" type="Boolean">False</method>
                <method name = "UserAccessLevelNo" readonly="False" type="UnicodeString"></method>
                <method name = "UserAccessScheduleNo" readonly="False" type="UnicodeString"></method>
                <method name = "UserExtendedUnlockTime" readonly="False" type="Boolean">False</method>
                <method name = "UserAddScheduleTolerance" readonly="False" type="Boolean">False</method>
                <method name = "UserCodeFollowsSchedule" readonly="False" type="Boolean">False</method>
                <method name = "UserArmWithCard" readonly="False" type="UnicodeString"></method>
                <method name = "UserCardDisarmOnAccess" readonly="False" type="Boolean">False</method>
                <method name = "UserCardAndPINDisarm" readonly="False" type="Boolean">False</method>
                <method name = "UserCodeLength" readonly="False" type="UnicodeString">4 Digits</method>
            </published>
        </object>
    </objects>
    */

    #endregion
    
    public class PanelUser : BasePanelModel<PanelUser>
    {
        public UInt32 UserNo;
        public string UserName;
        public bool UserUsed;
        public string UserCode;
        public string UserCard;
        public string UserRemoteSerialNo;
        public string UserPartitionAccess;
        public bool UserCanBypass;
        public bool UserCanStaySleepArm;
        public bool UserCanForceArm;
        public bool UserCanArmOnly;
        public bool UserCanActivationPGMOnly;
        public bool UserCanDuress;
        //Evo
        public string UserType;
        public bool UserAccessControlEnabled;
        public string UserAccessLevelNo;
        public string UserAccessScheduleNo;
        public bool UserExtendedUnlockTime;
        public bool UserAddScheduleTolerance;
        public bool UserCodeFollowsSchedule;
        public string UserArmWithCard;
        public bool UserCardDisarmOnAccess;
        public bool UserCardAndPINDisarm;
        public string UserCodeLength;

        public PanelUser()
        {
            sObjectname = "TPanelUserXML";
            sName = "User";
        }
        
        protected internal bool parseXML(string xmlString)
        {
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
                            var sname = reader.Value;

                            if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "method")
                            {
                                var svalue = reader.ReadString();
                                if (sname == "UserNo") UserNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "UserName") UserName = svalue.Trim();
                                else if (sname == "UserUsed") UserUsed = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCode") UserCode = svalue.Trim();
                                else if (sname == "UserCard") UserCard = svalue.Trim();
                                else if (sname == "UserRemoteSerialNo") UserRemoteSerialNo = svalue.Trim();
                                else if (sname == "UserPartitionAccess") UserPartitionAccess = svalue.Trim();
                                else if (sname == "UserCanBypass") UserCanBypass = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCanStaySleepArm") UserCanStaySleepArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCanForceArm") UserCanForceArm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCanArmOnly") UserCanArmOnly = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCanActivationPGMOnly") UserCanActivationPGMOnly = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCanDuress") UserCanDuress = Convert.ToBoolean(svalue.Trim());

                                //EVO
                                else if (sname == "UserType") UserType = svalue.Trim();
                                else if (sname == "UserAccessControlEnabled") UserAccessControlEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserAccessLevelNo") UserAccessLevelNo = svalue.Trim();
                                else if (sname == "UserAccessScheduleNo") UserAccessScheduleNo = svalue.Trim();
                                else if (sname == "UserExtendedUnlockTime") UserExtendedUnlockTime = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserAddScheduleTolerance") UserAddScheduleTolerance = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCodeFollowsSchedule") UserCodeFollowsSchedule = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserArmWithCard") UserArmWithCard = svalue.Trim();
                                else if (sname == "UserCardDisarmOnAccess") UserCardDisarmOnAccess = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCardAndPINDisarm") UserCardAndPINDisarm = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UserCodeLength") UserCodeLength = svalue.Trim();
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