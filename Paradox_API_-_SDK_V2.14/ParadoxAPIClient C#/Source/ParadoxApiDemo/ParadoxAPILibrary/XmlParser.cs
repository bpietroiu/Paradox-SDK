using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;


namespace ParadoxAPILibrary
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// CustomXmlParser Base class 
    /// 
    /// <summary>
    public class CustomXmlParser : IDisposable
    {
        protected string sObjectname;
        protected string sName;

        public string Objectname 
        { 
            get 
            { 
                return sObjectname; 
            }
            set 
            { 
                sObjectname = value; 
            }
        }
        public string Name 
        { 
            get 
            { 
                return sName; 
            }
            set
            {
                sName = value;
            }
        }

        protected void fullCopy(CustomXmlParser dest)
        {
            dest.sName = (string)sName.Clone();
            dest.sObjectname = (string)sObjectname.Clone();
        }

        public void Dispose()
        {           
            GC.SuppressFinalize(this);
        }   

        public Boolean checkBoolean(string svalue)
        {
            if (svalue.Trim().ToLower() == "true") return true;
            else return false;
        }

        private string propname(string fldname)
        {
            if (fldname == null || fldname == "") return "";
            else if (fldname[0] == 'F') return fldname.Substring(1);
            else return fldname;
        }

        //Example:  <object objectname="TPanelUserXML" name="User1">
        protected internal Boolean parseXML(XmlReader reader)
        {
            try
            {
                reader.MoveToFirstAttribute();
                sObjectname = reader.Value;                
                reader.MoveToNextAttribute();
                sName = reader.Value;
                return true;
            }
            catch (Exception e)
            {                
                return false;
            }
        }

        public Boolean serializeXML(ref String xmlObj) 
        {             
            StringBuilder output = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;
            
            try
            {
                // create the Type object
                Type typeObj = this.GetType();

                // declare and populate the arrays to hold the information...
                FieldInfo[] fi = typeObj.GetFields(); //(BindingFlags.Default | BindingFlags.Static | BindingFlags.Public); // fields

                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {
                    writer.WriteProcessingInstruction("xml", "version='1.0'");
                    writer.WriteStartElement("objects");
                    writer.WriteStartElement("object"); writer.WriteStartAttribute("objectname"); writer.WriteValue(Objectname); writer.WriteEndAttribute();
                    writer.WriteStartAttribute("name"); writer.WriteValue(Name); writer.WriteEndAttribute();

                    writer.WriteStartElement("published");

                    // iterate through all the field members
                    foreach (FieldInfo f in fi)
                    {
                        if (f.GetValue(this) != null)
                        {
                            writer.WriteStartElement("method"); writer.WriteStartAttribute("name");
                            writer.WriteValue(propname(f.Name)); writer.WriteEndAttribute();

                            writer.WriteStartAttribute("readonly"); writer.WriteValue("True"); writer.WriteEndAttribute();
                            writer.WriteStartAttribute("type");
                            if (f.FieldType.Name == "string") writer.WriteValue("UnicodeString");
                            else if (f.FieldType.Name == "String") writer.WriteValue("UnicodeString");
                            else if (f.FieldType.Name == "int") writer.WriteValue("Integer");
                            else if (f.FieldType.Name == "UInt32") writer.WriteValue("Integer");
                            else if (f.FieldType.Name == "Int32") writer.WriteValue("Integer");
                            else if (f.FieldType.Name == "Boolean") writer.WriteValue("Boolean");
                            else if (f.FieldType.Name == "DateTime") writer.WriteValue("Double");                                                            
                            else
                                writer.WriteValue("UnknownType?");
                            
                            writer.WriteEndAttribute();

                            if (f.FieldType.Name == "DateTime")
                            {
                                DateTime dt = (DateTime)f.GetValue(this);
                                                                
                                writer.WriteString(Convert.ToString(dt.ToOADate()));                                
                            }
                            else   
                            {
                                writer.WriteString(f.GetValue(this).ToString());                                
                            }

                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();   // </published>
                    writer.WriteEndElement();   // "object"      
                    writer.WriteFullEndElement();   // "objects"                          
                    
                    writer.Flush();
                }

                xmlObj = output.ToString();
                return true;
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception : {0}", e.Message);
                return false;
            }
        }// serializeXML

        public Boolean serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {            
            try
            {
                // create the Type object
                Type typeObj = this.GetType();

                // declare and populate the arrays to hold the information...
                FieldInfo[] fi = typeObj.GetFields(); //(BindingFlags.Default | BindingFlags.Static | BindingFlags.Public); // fields
                                   
                writer.WriteStartElement(String.Format("objects{0}", objectCount));
                objectCount += 1;
                    
                writer.WriteStartElement("object"); writer.WriteStartAttribute("objectname"); writer.WriteValue(Objectname); writer.WriteEndAttribute();
                writer.WriteStartAttribute("name"); writer.WriteValue(Name); writer.WriteEndAttribute();

                writer.WriteStartElement("published");

                // iterate through all the field members
                foreach (FieldInfo f in fi)
                {
                    if (f.GetValue(this) != null)
                    {
                        writer.WriteStartElement("method"); writer.WriteStartAttribute("name");
                        writer.WriteValue(propname(f.Name)); writer.WriteEndAttribute();

                        writer.WriteStartAttribute("readonly"); writer.WriteValue("True"); writer.WriteEndAttribute();
                        writer.WriteStartAttribute("type");
                        if (f.FieldType.Name == "string") writer.WriteValue("UnicodeString");
                        else if (f.FieldType.Name == "String") writer.WriteValue("UnicodeString");
                        else if (f.FieldType.Name == "int") writer.WriteValue("Integer");
                        else if (f.FieldType.Name == "UInt32") writer.WriteValue("Integer");
                        else if (f.FieldType.Name == "Int32") writer.WriteValue("Integer");
                        else if (f.FieldType.Name == "Boolean") writer.WriteValue("Boolean");
                        else if (f.FieldType.Name == "DateTime") writer.WriteValue("Double");
                        else
                            writer.WriteValue("UnknownType?");

                        writer.WriteEndAttribute();

                        if (f.FieldType.Name == "DateTime")
                        {
                            DateTime dt = (DateTime)f.GetValue(this);

                            writer.WriteString(Convert.ToString(dt.ToOADate()));
                        }
                        else
                        {
                            writer.WriteString(f.GetValue(this).ToString());
                        }

                        writer.WriteEndElement();                        
                    }
                }
                writer.WriteEndElement();   // </published>
                writer.WriteEndElement();   // "object"
                writer.WriteEndElement();   // "objects"                                                  
                                                  
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }// serializeXML

    }//class CustomXmlParser

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelUser
    /*         
     <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelUserXML" name="User1">
            <published>
              <method name="UserNo" readonly="False" type="Integer">0</method>
              <method name="UserName" readonly="False" type="UnicodeString"></method>
              <method name="UserUsed" readonly="False" type="Boolean">False</method>
              <method name="UserCode" readonly="False" type="UnicodeString"></method>
              <method name="UserCard" readonly="False" type="UnicodeString"></method>
              <method name="UserRemoteSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="UserPartitionAccess" readonly="False" type="UnicodeString"></method>
              <method name="UserCanBypass" readonly="False" type="Boolean">False</method>
              <method name="UserCanStaySleepArm" readonly="False" type="Boolean">False</method>
              <method name="UserCanForceArm" readonly="False" type="Boolean">False</method>
              <method name="UserCanArmOnly" readonly="False" type="Boolean">False</method>
              <method name="UserCanActivationPGMOnly" readonly="False" type="Boolean">False</method>
              <method name="UserCanDuress" readonly="False" type="Boolean">False</method>
              
              <method name="UserType" readonly="False" type="UnicodeString"></method>
              <method name="UserAccessControlEnabled" readonly="False" type="Boolean">False</method>
              <method name="UserAccessLevelNo" readonly="False" type="UnicodeString"></method>
              <method name="UserAccessScheduleNo" readonly="False" type="UnicodeString"></method>
              <method name="UserExtendedUnlockTime" readonly="False" type="Boolean">False</method>
              <method name="UserAddScheduleTolerance" readonly="False" type="Boolean">False</method>
              <method name="UserCodeFollowsSchedule" readonly="False" type="Boolean">False</method>
              <method name="UserArmWithCard" readonly="False" type="UnicodeString"></method>
              <method name="UserCardDisarmOnAccess" readonly="False" type="Boolean">False</method>
              <method name="UserCardAndPINDisarm" readonly="False" type="Boolean">False</method>
              <method name="UserCodeLength" readonly="False" type="UnicodeString">4 Digits</method> 
            </published>
          </object>
        </objects>     
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelUser : CustomXmlParser
    {        
        public UInt32 UserNo;
        public String UserName;
        public Boolean UserUsed;
        public String UserCode;
        public String UserCard;
        public String UserRemoteSerialNo;
        public String UserPartitionAccess;
        public Boolean UserCanBypass;
        public Boolean UserCanStaySleepArm;
        public Boolean UserCanForceArm;
        public Boolean UserCanArmOnly;
        public Boolean UserCanActivationPGMOnly;
        public Boolean UserCanDuress;
        //Evo
        public String UserType;
        public Boolean UserAccessControlEnabled;
        public String UserAccessLevelNo;
        public String UserAccessScheduleNo;        
        public Boolean UserExtendedUnlockTime;
        public Boolean UserAddScheduleTolerance;
        public Boolean UserCodeFollowsSchedule;
        public String UserArmWithCard;
        public Boolean UserCardDisarmOnAccess;
        public Boolean UserCardAndPINDisarm;
        public String UserCodeLength;
                
        public PanelUser() { sObjectname = "TPanelUserXML"; sName = "User"; }

        public PanelUser fullCopy()
        {
            PanelUser cloned = (PanelUser)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelUserList
    ///    
    /// </summary>
    public class PanelUserList
    {
        public ArrayList panelUsers;

        public PanelUserList()
        {
            panelUsers = new ArrayList();
        }

        ~PanelUserList()
        {
            panelUsers.Clear();
        }

        public PanelUserList fullCopy()
        {

            PanelUserList cloned = new PanelUserList();
            for (int i = 0; i < panelUsers.Count; i++)
                cloned.panelUsers.Add(((PanelUserList)panelUsers[i]).fullCopy());
            return cloned;
        }

        public PanelUser this[UInt32 index]
        {
            get
            {
                PanelUser panelUser = null;

                for (int i = 0; i < panelUsers.Count; i++)
                {
                    panelUser = (PanelUser)panelUsers[i];

                    if (panelUser.UserNo == index)
                    {
                        return panelUser;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelUser panelUser = null;

                for (int i = 0; i < panelUsers.Count; i++)
                {
                    panelUser = (PanelUser)panelUsers[i];

                    if (panelUser.UserNo == value.UserNo)
                    {
                        panelUser = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelUser pnlUser = new PanelUser();

                    pnlUser = value;

                    panelUsers.Add(pnlUser);
                }
            }
        }

        public void Clear()
        {
            panelUsers.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelUser obj in panelUsers)
            {
                obj.Name = String.Format("User{0}", obj.UserNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        public void serializeXML(ref String xmlObjs)
        {
            UInt32 objectCount = 1;
            StringBuilder output = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(output, ws);

            ws.Indent = true;

            writer.WriteProcessingInstruction("xml", "version='1.0'");
            writer.WriteStartElement("Data");

            foreach (PanelUser obj in panelUsers)
            {
                obj.Name = String.Format("User{0}", obj.UserNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }

            writer.WriteFullEndElement(); // "Data"
            writer.Flush();

            xmlObjs = output.ToString();
        }



        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {                   
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {                       
                        panelUsers.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelUserXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {                                    
                                    PanelUser obj = new PanelUser();
                                    obj.parseXML(sobject);
                                    panelUsers.Add(obj);                                                                                                                                                                                                                     
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }        
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelZone
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelZone : CustomXmlParser
    {
        public UInt32 ZoneNo;
        public Boolean ZoneEnabled;
        public String ZoneLabel;
        public String ZoneSerialNo;
        public UInt32 ZoneInputNo;
        public UInt32 ZonePartition;
        public String ZoneDefinition;
        public String ZoneAlarmType;
        public Boolean ZoneStay;
        public Boolean ZoneForce;
        public Boolean ZoneBypass;
        public Boolean ZoneAutoShutdown;
        public Boolean ZoneRFSupervision;
        public Boolean ZoneIntellizone;
        public Boolean ZoneDelayBeforeTransmission;
        //EVO
        public String ZoneTXSerialNo;
        public Boolean ZoneTamperFollowGlobalSetting;
        public String ZoneTamperSupervision;
        public Boolean ZoneAntiMaskFollowGlobalSetting;
        public String ZoneAntiMaskSupervision;
        public Boolean ZoneBuzzerAlarmWhenDisarm;
        public Boolean ZoneBuzzerAlarmReported;
        public String Status;
                      

        public PanelZone() { sObjectname = "TPanelZoneXML"; sName = "Zone"; }

        public PanelZone fullCopy()
        {
            PanelZone cloned = (PanelZone)this.MemberwiseClone();
            return cloned;
        }


        protected internal Boolean parseXML(string xmlString)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }                
            }
            else
                return false;                
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelZoneList
    ///    
    /// </summary>
    public class PanelZoneList
    {
        public ArrayList panelZones;

        public PanelZoneList()
        {
            panelZones = new ArrayList();
        }

        ~PanelZoneList()
        {
            panelZones.Clear();
        }

        public PanelZoneList fullCopy()
        {

            PanelZoneList cloned = new PanelZoneList();
            for (int i = 0; i < panelZones.Count; i++)
                cloned.panelZones.Add(((PanelZoneList)panelZones[i]).fullCopy());
            return cloned;
        }

        public PanelZone this[UInt32 index]
        {
            get
            {
                PanelZone panelZone = null;

                for (int i = 0; i < panelZones.Count; i++)
                {
                    panelZone = (PanelZone)panelZones[i];

                    if (panelZone.ZoneNo == index)
                    {
                        return panelZone;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelZone panelZone = null;

                for (int i = 0; i < panelZones.Count; i++)
                {
                    panelZone = (PanelZone)panelZones[i];

                    if (panelZone.ZoneNo == value.ZoneNo)
                    {
                        panelZone = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelZone pnlZone = new PanelZone();

                    pnlZone = value;

                    panelZones.Add(pnlZone);
                }
            }
        }

        public void Clear()
        {
            panelZones.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelZone obj in panelZones)
            {
                obj.Name = String.Format("Zone{0}", obj.ZoneNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }   
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        panelZones.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelZoneXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelZone obj = new PanelZone();
                                    obj.parseXML(sobject);
                                    panelZones.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelArea
    ///    
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelArea : CustomXmlParser
    {
        public UInt32 AreaNo;
        public String AreaLabel;        
        public Boolean AreaEnabled;
        public Int32 AreaExitDelay;
        public Int32 AreaBellCutOffTimer;
        public DateTime AreaAutoArmingTime;
        public Int32 AreaNoMovementTimer;
        public String Status;

        //EVO
        public UInt32 AreaEntryDelay1;
        public UInt32 AreaEntryDelay2;
        public UInt32 AreaSpecialExitDelay;
        public DateTime AreaNoMovementScheduleStartTimeA;
        public DateTime AreaNoMovementScheduleEndTimeA;
        public String AreaNoMovementScheduleDaysA;
        public DateTime AreaNoMovementScheduleStartTimeB;
        public DateTime AreaNoMovementScheduleEndTimeB;
        public String AreaNoMovementScheduleDaysB;
        public DateTime AreaArmingReportScheduleStartTimeA;
        public DateTime AreaArmingReportScheduleEndTimeA;
        public String AreaArmingReportScheduleDaysA;
        public DateTime AreaArmingReportScheduleStartTimeB;
        public DateTime AreaArmingReportScheduleEndTimeB;
        public String AreaArmingReportScheduleDaysB;
        public DateTime AreaDisarmingReportScheduleStartTimeA;
        public DateTime AreaDisarmingReportScheduleEndTimeA;
        public String AreaDisarmingReportScheduleDaysA;
        public DateTime AreaDisarmingReportScheduleStartTimeB;
        public DateTime AreaDisarmingReportScheduleEndTimeB;
        public String AreaDisarmingReportScheduleDaysB;
        public Boolean AreaExitDelayTermination;
        public Boolean AreaNoExitDelayViaRemote;
        public Boolean AreaBellSirenEnable;
        public Boolean AreaSquawkOnAutoArm;
        public String AreaSquawkOnArm;
        public String AreaSquawkOnDelay;
        public String AreaRingBackOption;
        public Boolean AreaSquawkOnRemoteArm;
        public Boolean AreaOneTouchRegularArm;
        public Boolean AreaOneTouchStayArm;
        public Boolean AreaOneTouchInstantArm;
        public Boolean AreaOneTouchForceArm;
        public Boolean AreaOneTouchInstantDisarm;
        public Boolean AreaBypassProgramming;
        public String AreaFollowAreas;
        public Boolean AreaSwitchToStayIfNoEntry;
        public Boolean AreaSwitchToForceIfRegularArm;
        public Boolean AreaSwitchToForceIfStayArm;
        public Boolean AreaSwitchToEntryDelay2AfterDelay;
        public UInt32 AreaClosingDelinquency;
        public UInt32 AreaMaxZoneBypassed;
        public Boolean AreaAutoArmEnabled;
        public UInt32 AreaPostPoneAutoArm;
        public String AreaAutoArmingMethod;
        public Boolean AreaNoMovementAutoArmEnabled;
        public String AreaPanic1;
        public String AreaPanic2;
        public String AreaPanic3;
        public UInt32 AreaAutoZoneShutdownCounter;
        public UInt32 AreaRecentClosing;
        public UInt32 AreaInvalidCodesBeforeLockout;
        public UInt32 AreaKeypadLockoutDuration;
        public UInt32 AreaRecycleAlarmDelay;
        public UInt32 AreaRecycleCount;
        public UInt32 AreaPoliceCodeDelay;
        public Boolean AreaPoliceCodeOnZoneClosingOnly;

        public PanelArea() { sObjectname = "TPanelAreaXML"; sName = "Area"; }

        public PanelArea fullCopy()
        {
            PanelArea cloned = (PanelArea)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelAreaList
    ///    
    /// </summary>
    public class PanelAreaList
    {
        public ArrayList panelAreas;

        public PanelAreaList()
        {
            panelAreas = new ArrayList();
        }

        ~PanelAreaList()
        {
            panelAreas.Clear();
        }

        public PanelAreaList fullCopy()
        {

            PanelAreaList cloned = new PanelAreaList();
            for (int i = 0; i < panelAreas.Count; i++)
                cloned.panelAreas.Add(((PanelAreaList)panelAreas[i]).fullCopy());
            return cloned;
        }

        public PanelArea this[UInt32 index]
        {
            get
            {
                PanelArea panelArea = null;

                for (int i = 0; i < panelAreas.Count; i++)
                {
                    panelArea = (PanelArea)panelAreas[i];
                    
                    if (panelArea.AreaNo == index)
                    {
                        return panelArea;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelArea panelArea = null;

                for (int i = 0; i < panelAreas.Count; i++)
                {
                    panelArea = (PanelArea)panelAreas[i];

                    if (panelArea.AreaNo == value.AreaNo)
                    {
                        panelArea = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelArea pnlArea = new PanelArea();

                    pnlArea = value;

                    panelAreas.Add(pnlArea);
                }
            }
        }

        public void Clear()
        {
            panelAreas.Clear();
        }


        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelArea obj in panelAreas)
            {
                obj.Name = String.Format("Area{0}", obj.AreaNo);
                obj.serializeXML(ws, output, writer, ref objectCount);  
            }   
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;

            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        panelAreas.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelAreaXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelArea obj = new PanelArea();
                                    obj.parseXML(sobject);
                                    panelAreas.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelDoor
    ///    
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelDoor : CustomXmlParser
    {
        public UInt32 DoorNo;
        public String DoorLabel;
        public Boolean DoorEnabled;
        public String DoorSerialNo;
        public String DoorAreaAssignment;
        public Boolean DoorAccessEnabled;
        public String DoorAccessOption;
        public Boolean DoorAccessCodeOnKeypad;
        public Boolean DoorAccessCardAndCode;
        public Boolean DoorAccessArmRestricted;
        public Boolean DoorAccessDisarmRestricted;
        public Boolean DoorBurglaryAlarmOnForced;
        public Boolean DoorSkipDelayOnArmWithCard;
        public Boolean DoorBurglaryAlarmOnLeftOpen;
        public Boolean DoorMasterOnlyOnClockLost;
        public Int32 DoorEntryToleranceWindow;
        public Boolean DoorReportOnRequestToExit;
        public Boolean DoorReportOnDoorCommandFromPC;
        public Boolean DoorReportOnUserAccessDenied;
        public Boolean DoorReportOnUserAccessGranted;
        public Boolean DoorReportOnLeftOpen;
        public Boolean DoorReportOnFocedOpen;               
        public DateTime DoorUnlockScheduleStartTimeA;
        public DateTime DoorUnlockScheduleEndTimeA;
        public String DoorUnlockScheduleDaysA;
        public DateTime DoorUnlockScheduleStartTimeB;
        public DateTime DoorUnlockScheduleEndTimeB;
        public String DoorUnlockScheduleDaysB;
        public Boolean DoorSafeModeEnabled;
        public String DoorSafeModeCard1;
        public String DoorSafeModeCard2;
        public String DoorSafeModeCard3;
        public String DoorSafeModeCard4;
        public Boolean DoorCardActivatesUnlockedSchedule;
        public Boolean DoorUnlockDoorOnFireAlarm;
        public Boolean DoorUnlockOnRequestForExit;
        public String Status;
               
        public PanelDoor() { sObjectname = "TPanelDoorXML"; sName = "Door"; }

        public PanelDoor fullCopy()
        {
            PanelDoor cloned = (PanelDoor)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                                    catch (Exception e)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelDoorList
    ///    
    /// </summary>
    public class PanelDoorList
    {
        public ArrayList panelDoors;

        public PanelDoorList()
        {
            panelDoors = new ArrayList();
        }

        ~PanelDoorList()
        {
            panelDoors.Clear();
        }

        public PanelDoorList fullCopy()
        {

            PanelDoorList cloned = new PanelDoorList();
            for (int i = 0; i < panelDoors.Count; i++)
                cloned.panelDoors.Add(((PanelDoorList)panelDoors[i]).fullCopy());
            return cloned;
        }

        public PanelDoor this[UInt32 index]
        {
            get
            {
                PanelDoor panelDoor = null;

                for (int i = 0; i < panelDoors.Count; i++)
                {
                    panelDoor = (PanelDoor)panelDoors[i];

                    if (panelDoor.DoorNo == index)
                    {
                        return panelDoor;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelDoor panelDoor = null;

                for (int i = 0; i < panelDoors.Count; i++)
                {
                    panelDoor = (PanelDoor)panelDoors[i];

                    if (panelDoor.DoorNo == value.DoorNo)
                    {
                        panelDoor = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelDoor pnlDoor = new PanelDoor();

                    pnlDoor = value;

                    panelDoors.Add(pnlDoor);
                }
            }
        }

        public void Clear()
        {
            panelDoors.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelDoor obj in panelDoors)
            {
                obj.Name = String.Format("Door{0}", obj.DoorNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }           
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        panelDoors.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelDoorXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelDoor obj = new PanelDoor();
                                    obj.parseXML(sobject);
                                    panelDoors.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelPGM
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelPGMXML" name="PGM1">
            <published>
              <method name="PGMNo" readonly="False" type="Integer">0</method>
              <method name="PGMLabel" readonly="False" type="UnicodeString"></method>
              <method name="PGMTimer" readonly="False" type="Integer">0</method>
              <method name="PGMSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="PGMInputNo" readonly="False" type="Integer">0</method>
              <method name="PGMActivationEvent" readonly="False" type="UnicodeString"></method>
              <method name="PGMDeactivationEvent" readonly="False" type="UnicodeString"></method>
              <method name="PGMActvationMode" readonly="False" type="UnicodeString"></method>
              <method name="PGMPulseEvery30Secs" readonly="False" type="Boolean">False</method>
              <method name="PGMPulseOnAnyAlarm" readonly="False" type="Boolean">False</method>
              <method name="PGMInitialState" readonly="False" type="UnicodeString"></method> 
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelPGM : CustomXmlParser
    {
        public UInt32 PGMNo;
        public String PGMLabel;
        public Int32 PGMTimer;
        public String PGMSerialNo;
        public Int32 PGMInputNo;
        public String PGMActivationEvent;
        public String PGMDeactivationEvent;
        public String PGMActvationMode;
        public Boolean PGMPulseEvery30Secs;
        public Boolean PGMPulseOnAnyAlarm;
        public String PGMInitialState;
        public String Status;

        public PanelPGM() { sObjectname = "TPanelPGMXML"; sName = "PGM"; }

        public PanelPGM fullCopy()
        {
            PanelPGM cloned = (PanelPGM)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                if (sname == "PGMNo") PGMNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "PGMLabel") PGMLabel = svalue.Trim();
                                else if (sname == "PGMTimer") PGMTimer = Convert.ToInt32(svalue.Trim());
                                else if (sname == "PGMSerialNo") PGMSerialNo = svalue.Trim();
                                else if (sname == "PGMInputNo") PGMInputNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "PGMActivationEvent") PGMActivationEvent = svalue.Trim();
                                else if (sname == "PGMDeactivationEvent") PGMDeactivationEvent = svalue.Trim();
                                else if (sname == "PGMActvationMode") PGMActvationMode = svalue.Trim();
                                else if (sname == "PGMPulseEvery30Secs") PGMPulseEvery30Secs = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "PGMPulseOnAnyAlarm") PGMPulseEvery30Secs = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "PGMInitialState") PGMInitialState = svalue.Trim();                                
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelPGMList
    ///    
    /// </summary>
    public class PanelPGMList
    {
        public ArrayList panelPGMs;

        public PanelPGMList()
        {
            panelPGMs = new ArrayList();
        }

        ~PanelPGMList()
        {
            panelPGMs.Clear();
        }

        public PanelPGMList fullCopy()
        {

            PanelPGMList cloned = new PanelPGMList();
            for (int i = 0; i < panelPGMs.Count; i++)
                cloned.panelPGMs.Add(((PanelPGMList)panelPGMs[i]).fullCopy());
            return cloned;
        }

        public PanelPGM this[UInt32 index]
        {
            get
            {
                PanelPGM panelPGM = null;

                for (int i = 0; i < panelPGMs.Count; i++)
                {
                    panelPGM = (PanelPGM)panelPGMs[i];

                    if (panelPGM.PGMNo == index)
                    {
                        return panelPGM;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelPGM panelPGM = null;

                for (int i = 0; i < panelPGMs.Count; i++)
                {
                    panelPGM = (PanelPGM)panelPGMs[i];

                    if (panelPGM.PGMNo == value.PGMNo)
                    {
                        panelPGM = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelPGM pnlPGM = new PanelPGM();

                    pnlPGM = value;

                    panelPGMs.Add(pnlPGM);
                }
            }
        }

        public void Clear()
        {
            panelPGMs.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelPGM obj in panelPGMs)
            {
                obj.Name = String.Format("PGM{0}", obj.PGMNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            } 
        }
        
        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        panelPGMs.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelPGMXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelPGM obj = new PanelPGM();
                                    obj.parseXML(sobject);
                                    panelPGMs.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// ModuleInfo
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TModuleInfoXML" name="Module1">
            <published>
              <method name="MacString" readonly="False" type="UnicodeString"></method>
              <method name="NetMaskString" readonly="False" type="UnicodeString"></method>
              <method name="DHCPString" readonly="False" type="UnicodeString"></method>
              <method name="TypeString" readonly="False" type="UnicodeString"></method>
              <method name="IPString" readonly="False" type="UnicodeString"></method>
              <method name="SiteNameString" readonly="False" type="UnicodeString"></method>
              <method name="SiteIDString" readonly="False" type="UnicodeString"></method>
              <method name="VersionString" readonly="False" type="UnicodeString"></method>
              <method name="IPPortString" readonly="False" type="UnicodeString"></method>
              <method name="WebPortString" readonly="False" type="UnicodeString"></method>
              <method name="LanguageString" readonly="False" type="UnicodeString"></method>
              <method name="SerialNoString" readonly="False" type="UnicodeString"></method>
              <method name="HTTPSPortString" readonly="False" type="UnicodeString"></method>
              <method name="DiscoverOnLAN" readonly="False" type="Boolean">False</method>
              <method name="UseHTTPSString" readonly="False" type="UnicodeString"></method>
              <method name="RegisteredToPMH" readonly="False" type="Boolean">False</method>
              <method name="NetworkInterfaceIpAddress" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class ModuleInfo : CustomXmlParser
    {
        public String MacString;
        public String NetMaskString;
        public String DHCPString;
        public String TypeString;
        public String IPString;
        public String SiteNameString;
        public String SiteIDString;
        public String VersionString;
        public String IPPortString;
        public String WebPortString;
        public String LanguageString;
        public String SerialNoString;
        public String HTTPSPortString;
        public Boolean DiscoverOnLAN;
        public String UseHTTPSString;
        public Boolean RegisteredToPMH;
        public String NetworkInterfaceIpAddress;


        public Boolean PGMEnabled;


        public ModuleInfo() { sObjectname = "TModuleInfoXML"; sName = "ModuleInfo"; }

        public ModuleInfo fullCopy()
        {
            ModuleInfo cloned = (ModuleInfo)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                if (sname == "MacString") MacString = svalue.Trim();
                                else if (sname == "NetMaskString") NetMaskString = svalue.Trim();
                                else if (sname == "DHCPString") DHCPString = svalue.Trim();
                                else if (sname == "TypeString") TypeString = svalue.Trim();
                                else if (sname == "IPString") IPString = svalue.Trim();
                                else if (sname == "SiteNameString") SiteNameString = svalue.Trim();
                                else if (sname == "SiteIDString") SiteIDString = svalue.Trim();
                                else if (sname == "VersionString") VersionString = svalue.Trim();
                                else if (sname == "IPPortString") IPPortString = svalue.Trim();
                                else if (sname == "WebPortString") WebPortString = svalue.Trim();
                                else if (sname == "LanguageString") LanguageString = svalue.Trim();
                                else if (sname == "SerialNoString") SerialNoString = svalue.Trim();
                                else if (sname == "HTTPSPortString") HTTPSPortString = svalue.Trim();
                                else if (sname == "DiscoverOnLAN") DiscoverOnLAN = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "UseHTTPSString") UseHTTPSString = svalue.Trim();
                                else if (sname == "RegisteredToPMH") RegisteredToPMH = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "NetworkInterfaceIpAddress") NetworkInterfaceIpAddress = svalue.Trim();
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// ModuleInfoList
    ///    
    /// </summary>
    public class ModuleInfoList
    {
        public ArrayList moduleInfos;

        public ModuleInfoList()
        {
            moduleInfos = new ArrayList();
        }

        ~ModuleInfoList()
        {
            moduleInfos.Clear();
        }

        public ModuleInfoList fullCopy()
        {

            ModuleInfoList cloned = new ModuleInfoList();
            for (int i = 0; i < moduleInfos.Count; i++)
                cloned.moduleInfos.Add(((ModuleInfoList)moduleInfos[i]).fullCopy());
            return cloned;
        }

        public ModuleInfo this[UInt32 index]
        {
            get
            {
                if (index < moduleInfos.Count)
                {
                    return (ModuleInfo)moduleInfos[(int)index];
                }
                else 
                {
                    return null;
                }                           
            }
            set
            {
                Boolean found = false;

                ModuleInfo moduleInfo = null;

                for (int i = 0; i < moduleInfos.Count; i++)
                {
                    moduleInfo = (ModuleInfo)moduleInfos[i];

                    if (moduleInfo.MacString == value.MacString)
                    {
                        moduleInfo = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ModuleInfo _ModuleInfo = new ModuleInfo();

                    _ModuleInfo = value;

                    moduleInfos.Add(_ModuleInfo);
                }
            }
        }

        public void Clear()
        {
            moduleInfos.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (ModuleInfo obj in moduleInfos)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        moduleInfos.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TModuleInfoXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    ModuleInfo obj = new ModuleInfo();
                                    obj.parseXML(sobject);
                                    moduleInfos.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelEvent
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelEventXML" name="PanelEvent">
            <published>
              <method name="DateTime" readonly="False" type="Double">42201.4059706366</method>
              <method name="SequenceNo" readonly="False" type="Int64">-1</method>
              <method name="EventDateTime" readonly="False" type="UnicodeString"></method>
              <method name="EventLabel" readonly="False" type="UnicodeString"></method>
              <method name="EventType" readonly="False" type="UnicodeString"></method>
              <method name="EventSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="EventDescription" readonly="False" type="UnicodeString"></method>
              <method name="EventAdditionalInfo" readonly="False" type="UnicodeString"></method>
              <method name="EventUserLabel" readonly="False" type="UnicodeString"></method>
              <method name="EventSequenceNo" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelEvent : CustomXmlParser
    {
        public DateTime dateTime;
        public Int64 SequenceNo;
        public String EventDateTime;
        public String EventLabel;
        public String EventType;
        public String EventSerialNo;
        public String EventDescription;
        public String EventAdditionalInfo;
        public String EventUserLabel;
        public String EventSequenceNo;

        public PanelEvent() { sObjectname = "TPanelEventXML"; sName = "PanelEvent"; }

        public PanelEvent fullCopy()
        {
            PanelEvent cloned = (PanelEvent)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                if (sname == "DateTime")
                                {
                                    try
                                    {
                                        dateTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        dateTime = System.DateTime.FromOADate(0.0);
                                    }                                    
                                }
                                else if (sname == "SequenceNo") SequenceNo = Convert.ToInt64(svalue.Trim());
                                else if (sname == "EventDateTime") EventDateTime = svalue.Trim();
                                else if (sname == "EventLabel") EventLabel = svalue.Trim();
                                else if (sname == "EventType") EventType = svalue.Trim();
                                else if (sname == "EventSerialNo") EventSerialNo = svalue.Trim();
                                else if (sname == "EventDescription") EventDescription = svalue.Trim();
                                else if (sname == "EventAdditionalInfo") EventAdditionalInfo = svalue.Trim();
                                else if (sname == "EventUserLabel") EventUserLabel = svalue.Trim();
                                else if (sname == "EventSequenceNo") EventSequenceNo = svalue.Trim();
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelEventList
    ///    
    /// </summary>
    public class PanelEventList
    {
        public ArrayList panelEvents;

        public PanelEventList()
        {
            panelEvents = new ArrayList();
        }

        ~PanelEventList()
        {
            panelEvents.Clear();
        }

        public PanelEventList fullCopy()
        {

            PanelEventList cloned = new PanelEventList();
            for (int i = 0; i < panelEvents.Count; i++)
                cloned.panelEvents.Add(((PanelEventList)panelEvents[i]).fullCopy());
            return cloned;
        }

        public PanelEvent this[UInt32 index]
        {
            get
            {
                if (index < panelEvents.Count)
                {
                    return (PanelEvent)panelEvents[(int)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                /*
                Boolean found = false;

                PanelEvent panelEvent = null;
                
                for (int i = 0; i < panelEvents.Count; i++)
                {
                    panelEvent = (PanelEvent)panelEvents[i];

                    if (panelEvent.SequenceNo == value.SequenceNo)
                    {
                        panelEvent = value;
                        found = true;
                        break;
                    }
                }
                

                if (!found)
                {
                    PanelEvent _PanelEvent = new PanelEvent();

                    _PanelEvent = value;

                    panelEvents.Add(_PanelEvent);
                }
                 */
            }
        }

        public void Clear()
        {
            panelEvents.Clear();
        }

        public void AddEvent(PanelEvent panelEvent)
        {
            PanelEvent obj = new PanelEvent();

            obj = panelEvent;
            panelEvents.Add(obj);
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelEvent obj in panelEvents)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }
        
        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelEventXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelEvent obj = new PanelEvent();
                                    obj.parseXML(sobject);
                                    panelEvents.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelMonitoring
    ///    
    /*                
        <?xml version="1.0"?>
         <PanelInfo>
            <ZoneStatus Zone3="Opened,Alarm In Memory"/>
        </PanelInfo>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelMonitoring : CustomXmlParser
    {
        public const String C_MONITORING_ZONE_ITEM_TYPE = "Zone";
        public const String C_MONITORING_AREA_ITEM_TYPE = "Area";
        public const String C_MONITORING_DOOR_ITEM_TYPE = "Door";
        public const String C_MONITORING_PGM_ITEM_TYPE = "PGM";

        public UInt32 ItemNo;
        public String ItemType;
        public String Status;

        public PanelMonitoring() { sObjectname = "PanelInfo"; sName = "Status"; }

        public PanelMonitoring fullCopy()
        {
            PanelMonitoring cloned = (PanelMonitoring)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sType, sname, svalue, sItemNo;
            if (xmlString != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelMonitoringList
    ///    
    /// </summary>
    public class PanelMonitoringList
    {
        public ArrayList panelMonitorings;

        public PanelMonitoringList()
        {
            panelMonitorings = new ArrayList();
        }

        ~PanelMonitoringList()
        {
            panelMonitorings.Clear();
        }

        public PanelMonitoringList fullCopy()
        {

            PanelMonitoringList cloned = new PanelMonitoringList();
            for (int i = 0; i < panelMonitorings.Count; i++)
                cloned.panelMonitorings.Add(((PanelMonitoringList)panelMonitorings[i]).fullCopy());
            return cloned;
        }

        public PanelMonitoring this[UInt32 index]
        {
            get
            {                
                if (index < panelMonitorings.Count)                
                {
                    return (PanelMonitoring)panelMonitorings[(Int32)index];                  
                }
                else
                    return null;
            }
            set
            {                             
                if (index < panelMonitorings.Count) 
                {
                    PanelMonitoring panelMonitoring = (PanelMonitoring)panelMonitorings[(Int32)index];

                    panelMonitoring = value;                                                    
                }
                else        
                {
                    PanelMonitoring panelMonitoring = new PanelMonitoring();

                    panelMonitoring = value;

                    panelMonitorings.Add(panelMonitoring);
                }
            }
        }

        public void Clear()
        {
            panelMonitorings.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelMonitoring obj in panelMonitorings)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }
        
        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelMonitoringXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelMonitoring obj = new PanelMonitoring();
                                    obj.parseXML(sobject);
                                    panelMonitorings.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;        
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelReportingEvent
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingEventXML" name="PanelReportingEvent">
            <published>
              <method name="EventID" readonly="False" type="Integer">0</method>
              <method name="EventAccountNo" readonly="False" type="UnicodeString"></method>
              <method name="EventDateTime" readonly="False" type="Double">0</method>
              <method name="EventProtocolID" readonly="False" type="UnicodeString"></method>
              <method name="EventCode" readonly="False" type="UnicodeString"></method>
              <method name="EventDescription" readonly="False" type="UnicodeString"></method>
              <method name="EventAreaDoorNo" readonly="False" type="UnicodeString"></method>
              <method name="EventZoneUserNo" readonly="False" type="UnicodeString"></method>
              <method name="EventMACAddress" readonly="False" type="UnicodeString"></method>
              <method name="EventStatus" readonly="False" type="UnicodeString"></method>
              <method name="VODIPPort" readonly="True" type="Integer">0</method>
              <method name="VODSessionKey" readonly="True" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelReportingEvent : CustomXmlParser
    {
        public Int32 EventID;
        public String EventAccountNo;                
        public DateTime EventDateTime;        
        public String EventProtocolID;        
        public String EventCode;        
        public String EventDescription;
        public String EventAreaDoorNo;
        public String EventZoneUserNo;
        public String EventMACAddress;
        public String EventStatus;
        public UInt32 VODIPPort;
        public String VODSessionKey;

        public PanelReportingEvent() { sObjectname = "TPanelReportingEventXML"; sName = "PanelReportingEvent"; }

        public PanelReportingEvent fullCopy()
        {
            PanelReportingEvent cloned = (PanelReportingEvent)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "EventID") EventID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "EventAccountNo") EventAccountNo = svalue.Trim();
                                else if (sname == "EventDateTime")
                                {
                                    try
                                    {
                                        EventDateTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        EventDateTime = System.DateTime.FromOADate(0.0);
                                    }                                      
                                }
                                else if (sname == "EventProtocolID") EventProtocolID = svalue.Trim();
                                else if (sname == "EventCode") EventCode = svalue.Trim();
                                else if (sname == "EventDescription") EventDescription = svalue.Trim();
                                else if (sname == "EventAreaDoorNo") EventAreaDoorNo = svalue.Trim();
                                else if (sname == "EventZoneUserNo") EventZoneUserNo = svalue.Trim();
                                else if (sname == "EventMACAddress") EventMACAddress = svalue.Trim();
                                else if (sname == "EventStatus") EventStatus = svalue.Trim();                       
                                else if (sname == "VODIPPort") VODIPPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "VODSessionKey") VODSessionKey = svalue.Trim(); 
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelSettings
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelSettingsXML" name="Settings">
            <published>
              <method name="PanelType" readonly="False" type="UnicodeString"></method>
              <method name="ComType" readonly="False" type="UnicodeString"></method>
              <method name="SiteID" readonly="False" type="UnicodeString"></method>
              <method name="SerialNo" readonly="False" type="UnicodeString"></method>
              <method name="IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="IPPort" readonly="False" type="Integer">0</method>
              <method name="ComPort" readonly="False" type="Integer">0</method>
              <method name="BaudRate" readonly="False" type="Integer">0</method>
              <method name="SMSCallback" readonly="False" type="Boolean">False</method>
              <method name="IPPassword" readonly="False" type="UnicodeString"></method>
              <method name="UserCode" readonly="False" type="UnicodeString"></method>
              <method name="SystemAlarmLanguage" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelSettings : CustomXmlParser
    {
        public String PanelType;
        public String ComType;
        public String SiteID;
        public String SerialNo;
        public String IPAddress;
        public Int32 IPPort;
        public Int32 ComPort;
        public Int32 BaudRate;
        public Boolean SMSCallback;
        public String IPPassword;
        public String UserCode;
        public String SystemAlarmLanguage;

        public PanelSettings() 
        { 
            sObjectname = "TPanelSettingsXML"; 
            sName = "Settings";
            IPPort = 10000;
            IPPassword = "paradox";
            UserCode = "1234";
        }

        public PanelSettings fullCopy()
        {
            PanelSettings cloned = (PanelSettings)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "PanelType") PanelType = svalue.Trim();
                                else if (sname == "ComType") ComType = svalue.Trim();
                                else if (sname == "SiteID") SiteID = svalue.Trim();
                                else if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "IPPort") IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ComPort") ComPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "BaudRate") BaudRate = Convert.ToInt32(svalue.Trim());
                                else if (sname == "SMSCallback") SMSCallback = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "IPPassword") IPPassword = svalue.Trim();
                                else if (sname == "UserCode") UserCode = svalue.Trim();
                                else if (sname == "SystemAlarmLanguage") SystemAlarmLanguage = svalue.Trim();
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelInfo
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelInfoXML" name="PanelInfo">
            <published>
              <method name="SiteName" readonly="False" type="UnicodeString"></method>
              <method name="MediaType" readonly="False" type="UnicodeString"></method>
              <method name="ProductID" readonly="False" type="Integer">0</method>
              <method name="Description" readonly="False" type="UnicodeString"></method>
              <method name="SerialNo" readonly="False" type="UnicodeString"></method>
              <method name="Version" readonly="False" type="UnicodeString"></method>
              <method name="SiteID" readonly="False" type="UnicodeString"></method>
              <method name="IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="IPPort" readonly="False" type="Integer">0</method>
              <method name="WebPort" readonly="False" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelInfo : CustomXmlParser
    {
        public String SiteName;
        public String MediaType;
        public Int32 ProductID;        
        public String Description;
        public String SerialNo;
        public String SiteID;
        public String Version;
        public String IPAddress;
        public Int32 IPPort;
        public Int32 WebPort;
        

        public PanelInfo() { sObjectname = "TPanelInfoXML"; sName = "PanelInfo"; }

        public PanelInfo fullCopy()
        {
            PanelInfo cloned = (PanelInfo)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "SiteName") SiteName = svalue.Trim();
                                else if (sname == "MediaType") MediaType = svalue.Trim();
                                else if (sname == "ProductID") ProductID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "Description") Description = svalue.Trim();
                                else if (sname == "SiteID") Description = svalue.Trim();
                                else if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "Version") SerialNo = svalue.Trim();
                                else if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "IPPort") IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WebPort") WebPort = Convert.ToInt32(svalue.Trim());
                                
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelInfoEx
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelInfoExXML" name="PanelInfoEx">
            <published>
              <method name="Description" readonly="False" type="UnicodeString"></method>
              <method name="ProductID" readonly="False" type="Integer">0</method>
              <method name="SerialNo" readonly="False" type="UnicodeString"></method>
              <method name="Version" readonly="False" type="Integer">0</method>
              <method name="Revision" readonly="False" type="Integer">0</method>
              <method name="Build" readonly="False" type="Integer">0</method>
              <method name="AreaCount" readonly="False" type="Integer">0</method>
              <method name="ZoneCount" readonly="False" type="Integer">0</method>
              <method name="PGMCount" readonly="False" type="Integer">0</method>
              <method name="UserCount" readonly="False" type="Integer">0</method>
              <method name="DoorCount" readonly="False" type="Integer">0</method>
              <method name="ReceiverCount" readonly="False" type="Integer">0</method>
              <method name="ScheduleCount" readonly="False" type="Integer">0</method>
              <method name="AccessLevelCount" readonly="False" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelInfoEx : CustomXmlParser
    {
        public String Description;// { get; set;} 
        public Int32 ProductID;        
        public String SerialNo;
        public Int32 Version;
        public Int32 Revision;
        public Int32 Build;
        public Int32 AreaCount;
        public Int32 ZoneCount;
        public Int32 PGMCount;
        public Int32 UserCount;
        public Int32 DoorCount;
        public Int32 ReceiverCount;
        public Int32 ScheduleCount;
        public Int32 AccessLevelCount;


        public PanelInfoEx() { sObjectname = "TPanelInfoExXML"; sName = "PanelInfoEx"; }

        public PanelInfoEx fullCopy()
        {
            PanelInfoEx cloned = (PanelInfoEx)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "Description") Description = svalue.Trim();
                                else if (sname == "ProductID") ProductID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "Version") Version = Convert.ToInt32(svalue.Trim());
                                else if (sname == "Revision") Revision = Convert.ToInt32(svalue.Trim());
                                else if (sname == "Build") Build = Convert.ToInt32(svalue.Trim());
                                else if (sname == "AreaCount") AreaCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ZoneCount") ZoneCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "PGMCount") PGMCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "UserCount") UserCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "DoorCount") DoorCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ReceiverCount") ReceiverCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ScheduleCount") ScheduleCount = Convert.ToInt32(svalue.Trim());
                                else if (sname == "AccessLevelCount") AccessLevelCount = Convert.ToInt32(svalue.Trim());
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelControl
    ///    
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelControl : CustomXmlParser
    {
        public String Command;
        public String Items;       
        public Int32 Timer;        

        public PanelControl() 
        { 
            sObjectname = "TPanelControlXML"; 
            sName = "Action";
            Command = "";
            Items = "";
            Timer = 0;
        }

        public PanelControl fullCopy()
        {
            PanelControl cloned = (PanelControl)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "Command") Command = svalue.Trim();
                                else if (sname == "Items") Items = svalue.Trim();
                                else if (sname == "Timer") Timer = Convert.ToInt32(svalue.Trim());
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelTrouble
    ///       
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelTrouble : CustomXmlParser
    {        
        public UInt32 ItemNo;        
        public String Status;
               
        public PanelTrouble fullCopy()
        {
            PanelTrouble cloned = (PanelTrouble)this.MemberwiseClone();
            return cloned;
        }       
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelTroubleList
    /// 
    /// <?xml version="1.0"?>
    ///    <PanelInfo>
    ///      <PanelTrouble Trouble1="Battery Failure" Trouble2="IP Receiver 1 Fail to comm" Trouble3="Zone Tampered:2907586A" Trouble4="Module Failed to communicate:1FF008CF"/>
    ///    </PanelInfo>
    ///
    ///    
    /// </summary>
    public class PanelTroubleList
    {
        public ArrayList panelTroubles;

        public PanelTroubleList()
        {
            panelTroubles = new ArrayList();
        }

        ~PanelTroubleList()
        {
            panelTroubles.Clear();
        }

        public PanelTroubleList fullCopy()
        {

            PanelTroubleList cloned = new PanelTroubleList();
            for (int i = 0; i < panelTroubles.Count; i++)
                cloned.panelTroubles.Add(((PanelTroubleList)panelTroubles[i]).fullCopy());
            return cloned;
        }

        public PanelTrouble this[UInt32 index]
         {
            get
            {                
                if (index < panelTroubles.Count)                
                {
                    return (PanelTrouble)panelTroubles[(Int32)index];                  
                }
                else
                    return null;
            }
            set
            {                             
                if (index < panelTroubles.Count) 
                {
                    PanelTrouble panelTrouble = (PanelTrouble)panelTroubles[(Int32)index];

                    panelTrouble = value;                                                    
                }
                else        
                {
                    PanelTrouble panelTrouble = new PanelTrouble();

                    panelTrouble = value;

                    panelTroubles.Add(panelTrouble);
                }
            }
        }
        
        public void Clear()
        {
            panelTroubles.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelTrouble obj in panelTroubles)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sType, sname, svalue, sItemNo;

            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        reader.ReadToFollowing("PanelInfo");
                        reader.Read();
                        reader.Read();
                        sType = reader.LocalName;

                        if (sType == "PanelTrouble")
                        {
                            reader.MoveToFirstAttribute();

                            sname = reader.Name;
                            svalue = reader.Value;

                            if (sname.Contains("Trouble"))
                            {
                                sItemNo = sname.Remove(0, 7);
                                PanelTrouble obj = new PanelTrouble();
                                obj.ItemNo = Convert.ToUInt32(sItemNo);
                                obj.Status = svalue;
                                panelTroubles.Add(obj);
                            }
                            
                            while (reader.MoveToNextAttribute())
                            {
                                sname = reader.Name;
                                svalue = reader.Value;
                                
                                if (sname.Contains("Trouble"))
                                {
                                    sItemNo = sname.Remove(0, 7);
                                    PanelTrouble obj = new PanelTrouble();
                                    obj.ItemNo = Convert.ToUInt32(sItemNo);
                                    obj.Status = svalue;
                                    panelTroubles.Add(obj);
                                }
                            }                            
                        }                    
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelIPReporting
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelIPReportingXML" name="IPReporting">
            <published>
              <method name="ReceiverNo" readonly="False" type="Integer">0</method>
              <method name="ReportingIPEnabled" readonly="False" type="Boolean">False</method>
              <method name="ReceiverIPPassword" readonly="False" type="UnicodeString"></method>
              <method name="ReceiverIPProfile" readonly="False" type="Integer">0</method>
              <method name="Area1AccountNo" readonly="False" type="UnicodeString"></method>
              <method name="Area2AccountNo" readonly="False" type="UnicodeString"></method>
              <method name="WAN1IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="WAN1IPPort" readonly="False" type="Integer">0</method>
              <method name="WAN2IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="WAN2IPPort" readonly="False" type="Integer">0</method>
              <method name="ParallelReporting" readonly="False" type="Boolean">False</method>
              <method name="ServiceFailureOptions" readonly="False" type="UnicodeString"></method>
              <method name="GPRSAccessPointName" readonly="False" type="UnicodeString"></method>
              <method name="GPRSUserName" readonly="False" type="UnicodeString"></method>
              <method name="GPRSPassword" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelIPReporting : CustomXmlParser
    {
        public Int32 ReceiverNo;
        public Boolean ReportingIPEnabled;
        public String ReceiverIPPassword;
        public Int32 ReceiverIPProfile;
        public String Area1AccountNo;
        public String Area2AccountNo;
        public String WAN1IPAddress;
        public Int32 WAN1IPPort;
        public String WAN2IPAddress;
        public Int32 WAN2IPPort;
        public Boolean ParallelReporting;
        public String ServiceFailureOptions;
        public String GPRSAccessPointName;
        public String GPRSUserName;
        public String GPRSPassword;
        public String Status;
               

        public PanelIPReporting() { sObjectname = "TPanelIPReportingXML"; sName = "IPReporting"; }

        public PanelIPReporting fullCopy()
        {
            PanelIPReporting cloned = (PanelIPReporting)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "ReceiverNo") ReceiverNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ReportingIPEnabled") ReportingIPEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ReceiverIPPassword") ReceiverIPPassword = svalue.Trim();
                                else if (sname == "ReceiverIPProfile") ReceiverIPProfile = Convert.ToInt32(svalue.Trim());
                                else if (sname == "Area1AccountNo") Area1AccountNo = svalue.Trim();
                                else if (sname == "Area2AccountNo") Area2AccountNo = svalue.Trim();
                                else if (sname == "WAN1IPAddress") WAN1IPAddress = svalue.Trim();
                                else if (sname == "WAN1IPPort") WAN1IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WAN2IPAddress") WAN2IPAddress = svalue.Trim();
                                else if (sname == "WAN2IPPort") WAN2IPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ParallelReporting") ParallelReporting = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "ServiceFailureOptions") ServiceFailureOptions = svalue.Trim();
                                else if (sname == "GPRSAccessPointName") GPRSAccessPointName = svalue.Trim();
                                else if (sname == "GPRSUserName") GPRSUserName = svalue.Trim();
                                else if (sname == "GPRSPassword") GPRSPassword = svalue.Trim();
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelIPReportingList
    ///    
    /// </summary>
    public class PanelIPReportingList
    {
        public ArrayList panelIPReportings;

        public PanelIPReportingList()
        {
            panelIPReportings = new ArrayList();
        }

        ~PanelIPReportingList()
        {
            panelIPReportings.Clear();
        }

        public PanelIPReportingList fullCopy()
        {

            PanelIPReportingList cloned = new PanelIPReportingList();
            for (int i = 0; i < panelIPReportings.Count; i++)
                cloned.panelIPReportings.Add(((PanelIPReportingList)panelIPReportings[i]).fullCopy());
            return cloned;
        }

        public PanelIPReporting this[UInt32 index]
        {
            get
            {
                if ((index > 0) && (index <= panelIPReportings.Count))
                {
                    return (PanelIPReporting)panelIPReportings[(Int32)(index-1)];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if ((index > 0) && (index <= panelIPReportings.Count))
                {
                    PanelIPReporting panelIPReporting = (PanelIPReporting)panelIPReportings[(Int32)index-1];
                    panelIPReporting.ReceiverNo = (Int32)index;
                    panelIPReporting = value;
                }
                else
                {
                    PanelIPReporting panelIPReporting = new PanelIPReporting();
                    panelIPReporting.ReceiverNo = (Int32)index;
                    panelIPReporting = value;
                    panelIPReportings.Add(panelIPReporting);
                }
            }
        }

        public void Clear()
        {
            panelIPReportings.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelIPReporting obj in panelIPReportings)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        //panelIPReportings.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelIPReportingXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelIPReporting obj = new PanelIPReporting();
                                    obj.parseXML(sobject);
                                    panelIPReportings.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelReportingAccount
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingAccountXML" name="PanelReportingAccount">
            <published>
              <method name="AccountNo" readonly="False" type="UnicodeString"></method>
              <method name="AccountStatus" readonly="False" type="UnicodeString"></method>
              <method name="MACAddress" readonly="False" type="UnicodeString"></method>
              <method name="ProfileID" readonly="False" type="Integer">0</method>
              <method name="ProtocolID" readonly="False" type="UnicodeString"></method>
              <method name="PanelType" readonly="False" type="UnicodeString"></method>
              <method name="PanelSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="PanelVersion" readonly="False" type="UnicodeString"></method>
              <method name="ModuleType" readonly="False" type="UnicodeString"></method>
              <method name="ModuleSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="ModuleVersion" readonly="False" type="UnicodeString"></method>
              <method name="RegistrationDate" readonly="False" type="Double">0</method>
              <method name="LastIPAddress" readonly="False" type="UnicodeString"></method>
              <method name="LastPollingTime" readonly="False" type="Double">0</method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelReportingAccount : CustomXmlParser
    {
        public String AccountNo;
        public String AccountStatus;
        public String MACAddress;
        public Int32 ProfileID;
        public String ProtocolID;
        public String PanelType;
        public String PanelSerialNo;
        public String PanelVersion;
        public String ModuleType;
        public String ModuleSerialNo;
        public String ModuleVersion;
        public DateTime RegistrationDate;
        public String LastIPAddress;
        public DateTime LastPollingTime;


        public PanelReportingAccount() { sObjectname = "TPanelReportingAccountXML"; sName = "PanelReportingAccount"; }

        public PanelReportingAccount fullCopy()
        {
            PanelReportingAccount cloned = (PanelReportingAccount)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "AccountNo") AccountNo = svalue.Trim();
                                else if (sname == "AccountStatus") AccountStatus = svalue.Trim();
                                else if (sname == "MACAddress") MACAddress = svalue.Trim();
                                else if (sname == "ProfileID") ProfileID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ProtocolID") ProtocolID = svalue.Trim();
                                else if (sname == "PanelType") PanelType = svalue.Trim();
                                else if (sname == "PanelSerialNo") PanelSerialNo = svalue.Trim();
                                else if (sname == "PanelVersion") PanelVersion = svalue.Trim();
                                else if (sname == "ModuleType") ModuleType = svalue.Trim();
                                else if (sname == "ModuleSerialNo") ModuleSerialNo = svalue.Trim();
                                else if (sname == "ModuleVersion") ModuleVersion = svalue.Trim();
                                else if (sname == "RegistrationDate")
                                {
                                    try
                                    {
                                        RegistrationDate = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        RegistrationDate = System.DateTime.FromOADate(0.0);
                                    }
                                    
                                }
                                else if (sname == "LastIPAddress") LastIPAddress = svalue.Trim();
                                else if (sname == "LastPollingTime")
                                {
                                    try
                                    {
                                        LastPollingTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        LastPollingTime = System.DateTime.FromOADate(0.0);
                                    }                                    
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelReportingAccountList
    ///    
    /// </summary>
    public class PanelReportingAccountList
    {
        public ArrayList panelReportingAccounts;

        public PanelReportingAccountList()
        {
            panelReportingAccounts = new ArrayList();
        }

        ~PanelReportingAccountList()
        {
            panelReportingAccounts.Clear();
        }

        public PanelReportingAccountList fullCopy()
        {

            PanelReportingAccountList cloned = new PanelReportingAccountList();
            for (int i = 0; i < panelReportingAccounts.Count; i++)
                cloned.panelReportingAccounts.Add(((PanelReportingAccountList)panelReportingAccounts[i]).fullCopy());
            return cloned;
        }

        public PanelReportingAccount this[UInt32 index]
        {
            get
            {
                if (index < panelReportingAccounts.Count)
                {                    
                    return (PanelReportingAccount)panelReportingAccounts[(Int32)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (index < panelReportingAccounts.Count)
                {
                    PanelReportingAccount panelReportingAccount = (PanelReportingAccount)panelReportingAccounts[(Int32)index];
                    panelReportingAccount = value;
                }
                else
                {
                    PanelReportingAccount panelReportingAccount = new PanelReportingAccount();

                    panelReportingAccount = value;

                    panelReportingAccounts.Add(panelReportingAccount);
                }                                
            }
        }

        public void Clear()
        {
            panelReportingAccounts.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelReportingAccount obj in panelReportingAccounts)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        //panelReportingAccounts.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelReportingAccountXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelReportingAccount obj = new PanelReportingAccount();
                                    obj.parseXML(sobject);
                                    panelReportingAccounts.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelReportingEventUpdate
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingEventUpdateXML" name="PanelReportingEventUpdate">
            <published>
              <method name="EventID" readonly="False" type="Integer">0</method>
              <method name="EventStatus" readonly="False" type="UnicodeString"></method>
              <method name="EventDateTime" readonly="False" type="Double">0</method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelReportingEventUpdate : CustomXmlParser
    {        
        public Int32 EventID;
        public String EventStatus;        
        public DateTime EventDateTime;        

        public PanelReportingEventUpdate() { sObjectname = "TPanelReportingEventUpdateXML"; sName = "PanelReportingEventUpdate"; }

        public PanelReportingEventUpdate fullCopy()
        {
            PanelReportingEventUpdate cloned = (PanelReportingEventUpdate)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                
                                if (sname == "EventID") EventID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "EventStatus") EventStatus = svalue.Trim();
                                else if (sname == "EventDateTime")
                                {
                                    try
                                    {
                                        EventDateTime = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        EventDateTime = System.DateTime.FromOADate(0.0);
                                    }                                     
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// IPDOXSettings
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TIPDOXSettingsXML" name="Settings">
            <published>
              <method name="IPPassword" readonly="False" type="UnicodeString"></method> **
              <method name="ACCFilePath" readonly="False" type="UnicodeString"></method>
              <method name="LogEnabled" readonly="False" type="Boolean">False</method>
              <method name="LogLevel" readonly="False" type="Integer">0</method>
              <method name="LogInterval" readonly="False" type="Integer">0</method>
              <method name="LogFileLifeTime" readonly="False" type="Integer">0</method>
              <method name="LogMaxDiskSize" readonly="False" type="Integer">0</method>
              <method name="LogFileDir" readonly="False" type="UnicodeString"></method>
              <method name="OutputEnabled" readonly="False" type="Boolean">False</method>
              <method name="OutputType" readonly="False" type="Integer">0</method>
              <method name="OutputCOMPort" readonly="False" type="Integer">0</method>
              <method name="OutputBaudRate" readonly="False" type="Integer">0</method>
              <method name="OutputUDP" readonly="False" type="Boolean">False</method>
              <method name="OutputIPPort" readonly="False" type="Integer">0</method>
              <method name="OutputIPAddress" readonly="False" type="UnicodeString"></method>
              <method name="OutputProtocolID" readonly="False" type="Integer">0</method>
              <method name="OutputReceiverNo" readonly="False" type="Integer">0</method>
              <method name="OutputLineNo" readonly="False" type="Integer">0</method>
              <method name="OutputHeaderID" readonly="False" type="Integer">0</method>
              <method name="OutputTrailerID" readonly="False" type="Integer">0</method>
              <method name="OutputAckNack" readonly="False" type="Boolean">False</method>
              <method name="OutputWaitForAck" readonly="False" type="Integer">0</method>
              <method name="OutputTestMessage" readonly="False" type="Boolean">False</method>
              <method name="OutputTestMsgDelay" readonly="False" type="Integer">0</method>
              <method name="OutputForcePartition" readonly="False" type="Boolean">False</method>
              <method name="OutputPartitionNo" readonly="False" type="Integer">0</method>
              <method name="OutputUseMACAddress" readonly="False" type="Boolean">False</method>
              <method name="MonitoringAccountNo" readonly="False" type="UnicodeString"></method>
              <method name="WANId" readonly="False" type="Integer">0</method>
              <method name="WANEnabled" readonly="False" type="Boolean">False</method> ***
              <method name="WANPort" readonly="False" type="Integer">0</method> ***
              <method name="WANAddress" readonly="False" type="UnicodeString"></method> ***
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class IPDOXSettings : CustomXmlParser
    {
        public String IPPassword;
        public String AccountFilePath;
        public Boolean LogEnabled;
        public Int32 LogLevel;
        public Int32 LogInterval;
        public Int32 LogFileLifeTime;
        public Int32 LogMaxDiskSize;
        public String LogFileDir;
        public Boolean OutputEnabled;
        public Int32 OutputType;
        public Int32 OutputCOMPort;
        public Int32 OutputBaudRate;
        public Boolean OutputUDP;
        public Int32 OutputIPPort;
        public String OutputIPAddress;
        public Int32 OutputProtocolID;
        public Int32 OutputReceiverNo;
        public Int32 OutputLineNo;
        public Int32 OutputHeaderID;
        public Int32 OutputTrailerID;
        public Boolean OutputAckNack;
        public Int32 OutputWaitForAck;
        public Boolean OutputTestMessage;
        public Int32 OutputTestMsgDelay;
        public Boolean OutputForcePartition;
        public Int32 OutputPartitionNo;
        public Boolean OutputUseMACAddress;
        public String MonitoringAccountNo;
        public Int32 WANId;
        public Boolean WANEnabled;
        public Int32 WANPort;
        public String WANAddress;


        public IPDOXSettings() { sObjectname = "TIPDOXSettingsXML"; sName = "Settings"; }

        public IPDOXSettings fullCopy()
        {
            IPDOXSettings cloned = (IPDOXSettings)this.MemberwiseClone();
            return cloned;
        }

        public Boolean parseXML(string xmlString)
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

                                if (sname == "IPPassword") IPPassword = svalue.Trim();
                                else if (sname == "ACCFilePath") AccountFilePath = svalue.Trim();

                                else if (sname == "LogEnabled") LogEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "LogLevel") LogLevel = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogInterval") LogInterval = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogFileLifeTime") LogFileLifeTime = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogMaxDiskSize") LogMaxDiskSize = Convert.ToInt32(svalue.Trim());
                                else if (sname == "LogFileDir") LogFileDir = svalue.Trim();

                                else if (sname == "OutputEnabled") OutputEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputType") OutputType = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputCOMPort") OutputCOMPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputBaudRate") OutputBaudRate = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputUDP") OutputUDP = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputIPPort") OutputIPPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputIPAddress") OutputIPAddress = svalue.Trim();
                                else if (sname == "OutputProtocolID") OutputProtocolID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputReceiverNo") OutputReceiverNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputLineNo") OutputLineNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputHeaderID") OutputHeaderID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputTrailerID") OutputTrailerID = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputAckNack") OutputAckNack = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputWaitForAck") OutputWaitForAck = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputTestMessage") OutputTestMessage = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputTestMsgDelay") OutputTestMsgDelay = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputForcePartition") OutputForcePartition = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "OutputPartitionNo") OutputPartitionNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "OutputUseMACAddress") OutputUseMACAddress = Convert.ToBoolean(svalue.Trim());

                                else if (sname == "MonitoringAccountNo") MonitoringAccountNo = svalue.Trim();
                                else if (sname == "WANId") WANId = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WANEnabled") WANEnabled = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "WANPort") WANPort = Convert.ToInt32(svalue.Trim());
                                else if (sname == "WANAddress") WANAddress = svalue.Trim();
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// IPReportingStatus
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelReportingStatusXML" name="IPReportingStatus">
            <published>
              <method name="Registered" readonly="False" type="Boolean">False</method>
              <method name="RegistrationStatus" readonly="False" type="UnicodeString"></method>
              <method name="RegistrationError" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class IPReportingStatus : CustomXmlParser
    {
        public Boolean Registered;
        public String RegistrationStatus;
        public String RegistrationError;

        public IPReportingStatus() { sObjectname = "TPanelReportingStatusXML"; sName = "IPReportingStatus"; }

        public IPReportingStatus fullCopy()
        {
            IPReportingStatus cloned = (IPReportingStatus)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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

                                if (sname == "Registered") Registered = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "RegistrationStatus") RegistrationStatus = svalue.Trim();
                                else if (sname == "RegistrationError") RegistrationError = svalue.Trim();
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelIPReportingStatusList
    ///    
    /// </summary>
    public class PanelIPReportingStatusList
    {
        public ArrayList panelIPReportingStatusList;

        public PanelIPReportingStatusList()
        {
            panelIPReportingStatusList = new ArrayList();
        }

        ~PanelIPReportingStatusList()
        {
            panelIPReportingStatusList.Clear();
        }

        public PanelIPReportingStatusList fullCopy()
        {

            PanelIPReportingStatusList cloned = new PanelIPReportingStatusList();
            for (int i = 0; i < panelIPReportingStatusList.Count; i++)
                cloned.panelIPReportingStatusList.Add(((PanelIPReportingStatusList)panelIPReportingStatusList[i]).fullCopy());
            return cloned;
        }

        public IPReportingStatus this[UInt32 index]
        {
            get
            {
                if (index < panelIPReportingStatusList.Count)
                {
                    return (IPReportingStatus)panelIPReportingStatusList[(Int32)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (index < panelIPReportingStatusList.Count)
                {
                    IPReportingStatus panelIPReportingStatus = (IPReportingStatus)panelIPReportingStatusList[(Int32)index];
                    panelIPReportingStatus = value;
                }
                else
                {
                    IPReportingStatus panelIPReportingStatus = new IPReportingStatus();

                    panelIPReportingStatus = value;

                    panelIPReportingStatusList.Add(panelIPReportingStatus);
                }
            }
        }

        public void Clear()
        {
            panelIPReportingStatusList.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (IPReportingStatus obj in panelIPReportingStatusList)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        //panelIPReportingStatusList.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelReportingStatusXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    IPReportingStatus obj = new IPReportingStatus();
                                    obj.parseXML(sobject);
                                    panelIPReportingStatusList.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelTimeStamp
    ///    
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelTimeStamp : CustomXmlParser
    {
        public Int32 TimeStamp;

        public PanelTimeStamp() { sObjectname = "TPanelTimeStampXML"; sName = "PanelTimeStamp"; }

        public PanelTimeStamp fullCopy()
        {
            PanelTimeStamp cloned = (PanelTimeStamp)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelSchedule
    /*         
     <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelScheduleXML" name="Schedule1">
            <published>
              <method name="ScheduleNo" readonly="False" type="Integer">0</method>
              <method name="ScheduleLabel" readonly="False" type="UnicodeString"></method>
              <method name="ScheduleBackupNo" readonly="False" type="Integer">0</method>
              <method name="ScheduleStartTimeIntervalA" readonly="False" type="Double">0</method>
              <method name="ScheduleEndTimeIntervalA" readonly="False" type="Double">0</method>
              <method name="ScheduleDaysIntervalA" readonly="False" type="UnicodeString"></method>
              <method name="ScheduleStartTimeIntervalB" readonly="False" type="Double">0</method>
              <method name="ScheduleEndTimeIntervalB" readonly="False" type="Double">0</method>
              <method name="ScheduleDaysIntervalB" readonly="False" type="UnicodeString"></method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelSchedule : CustomXmlParser
    {
        public UInt32 ScheduleNo;
        public String ScheduleLabel;
        public Int32 ScheduleBackupNo;
        public DateTime ScheduleStartTimeIntervalA;
        public DateTime ScheduleEndTimeIntervalA;
        public String ScheduleDaysIntervalA;
        public DateTime ScheduleStartTimeIntervalB;
        public DateTime ScheduleEndTimeIntervalB;
        public String ScheduleDaysIntervalB;

        public PanelSchedule() { sObjectname = "TPanelScheduleXML"; sName = "Schedule"; }

        public PanelSchedule fullCopy()
        {
            PanelSchedule cloned = (PanelSchedule)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                if (sname == "ScheduleNo") ScheduleNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ScheduleLabel") ScheduleLabel = svalue.Trim();
                                else if (sname == "ScheduleBackupNo") ScheduleBackupNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ScheduleStartTimeIntervalA")
                                {
                                    try
                                    {
                                        ScheduleStartTimeIntervalA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        ScheduleStartTimeIntervalA = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "ScheduleEndTimeIntervalA")
                                {
                                    try
                                    {
                                        ScheduleEndTimeIntervalA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        ScheduleEndTimeIntervalA = System.DateTime.FromOADate(0.0);
                                    }
                                }                                
                                else if (sname == "ScheduleDaysIntervalA") ScheduleDaysIntervalA = svalue.Trim();
                                else if (sname == "ScheduleStartTimeIntervalB")
                                {
                                    try
                                    {
                                        ScheduleStartTimeIntervalB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        ScheduleStartTimeIntervalB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "ScheduleEndTimeIntervalB")
                                {
                                    try
                                    {
                                        ScheduleEndTimeIntervalB = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch (Exception e)
                                    {
                                        ScheduleEndTimeIntervalB = System.DateTime.FromOADate(0.0);
                                    }
                                }                                        
                                else if (sname == "ScheduleDaysIntervalB") ScheduleDaysIntervalB = svalue.Trim();                                
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelScheduleList
    ///    
    /// </summary>
    public class PanelScheduleList
    {
        public ArrayList panelSchedules;

        public PanelScheduleList()
        {
            panelSchedules = new ArrayList();
        }

        ~PanelScheduleList()
        {
            panelSchedules.Clear();
        }

        public PanelScheduleList fullCopy()
        {

            PanelScheduleList cloned = new PanelScheduleList();
            for (int i = 0; i < panelSchedules.Count; i++)
                cloned.panelSchedules.Add(((PanelScheduleList)panelSchedules[i]).fullCopy());
            return cloned;
        }

        public PanelSchedule this[UInt32 index]
        {
            get
            {
                PanelSchedule panelSchedule = null;

                for (int i = 0; i < panelSchedules.Count; i++)
                {
                    panelSchedule = (PanelSchedule)panelSchedules[i];

                    if (panelSchedule.ScheduleNo == index)
                    {
                        return panelSchedule;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelSchedule panelSchedule = null;

                for (int i = 0; i < panelSchedules.Count; i++)
                {
                    panelSchedule = (PanelSchedule)panelSchedules[i];

                    if (panelSchedule.ScheduleNo == value.ScheduleNo)
                    {
                        panelSchedule = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelSchedule pnlSchedule = new PanelSchedule();

                    pnlSchedule = value;

                    panelSchedules.Add(pnlSchedule);
                }
            }
        }

        public void Clear()
        {
            panelSchedules.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelSchedule obj in panelSchedules)
            {
                obj.Name = String.Format("Schedule{0}", obj.ScheduleNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        panelSchedules.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelScheduleXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelSchedule obj = new PanelSchedule();
                                    obj.parseXML(sobject);
                                    panelSchedules.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelAccessLevel
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public class Door
    {
        public Int32 No;
    }

    public class PanelAccessLevel : CustomXmlParser
    {
        protected string sAccessLevelDoors;

        public ArrayList Doors;

        public UInt32 AccessLevelNo;
        public String AccessLevelLabel;       
        public String AccessLevelDoors
        {
            get 
            { 
                return sAccessLevelDoors; 
            }
            set 
            { 
                sAccessLevelDoors = value;
                String[] strValues = sAccessLevelDoors.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);                

                foreach (string strValue in strValues)                                
                {
                    Door obj = new Door();
                    obj.No = Convert.ToInt32(strValue);
                    Doors.Add(obj);                                                                           
                }
            }
        }               

        public PanelAccessLevel() 
        { 
            sObjectname = "TPanelAccessLevelXML"; 
            sName = "AccessLevel";

            Doors = new ArrayList();
        }

        ~PanelAccessLevel()
        {
            Doors.Clear();
        }

        public PanelAccessLevel fullCopy()
        {
            PanelAccessLevel cloned = (PanelAccessLevel)this.MemberwiseClone();
            return cloned;
        }                       
    

        protected internal Boolean parseXML(string xmlString)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelAccessLevelList
    ///    
    /// </summary>
    public class PanelAccessLevelList
    {
        public ArrayList panelAccessLevels;

        public PanelAccessLevelList()
        {
            panelAccessLevels = new ArrayList();
        }

        ~PanelAccessLevelList()
        {
            panelAccessLevels.Clear();
        }

        public PanelAccessLevelList fullCopy()
        {

            PanelAccessLevelList cloned = new PanelAccessLevelList();
            for (int i = 0; i < panelAccessLevels.Count; i++)
                cloned.panelAccessLevels.Add(((PanelAccessLevelList)panelAccessLevels[i]).fullCopy());
            return cloned;
        }

        public PanelAccessLevel this[UInt32 index]
        {
            get
            {
                PanelAccessLevel panelAccessLevel = null;

                for (int i = 0; i < panelAccessLevels.Count; i++)
                {
                    panelAccessLevel = (PanelAccessLevel)panelAccessLevels[i];

                    if (panelAccessLevel.AccessLevelNo == index)
                    {
                        return panelAccessLevel;
                    }
                }

                return null;
            }
            set
            {
                Boolean found = false;

                PanelAccessLevel panelAccessLevel = null;

                for (int i = 0; i < panelAccessLevels.Count; i++)
                {
                    panelAccessLevel = (PanelAccessLevel)panelAccessLevels[i];

                    if (panelAccessLevel.AccessLevelNo == value.AccessLevelNo)
                    {
                        panelAccessLevel = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelAccessLevel pnlAccessLevel = new PanelAccessLevel();

                    pnlAccessLevel = value;

                    panelAccessLevels.Add(pnlAccessLevel);
                }
            }
        }

        public void Clear()
        {
            panelAccessLevels.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelAccessLevel obj in panelAccessLevels)
            {
                obj.Name = String.Format("AccessLevel{0}", obj.AccessLevelNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        panelAccessLevels.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelAccessLevelXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelAccessLevel obj = new PanelAccessLevel();
                                    obj.parseXML(sobject);
                                    panelAccessLevels.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelHolidays
    ///       
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class PanelHoliday
    {
        public UInt32 ItemNo;
        public UInt32 Day;
        public UInt32 Month;

        public PanelHoliday fullCopy()
        {
            PanelHoliday cloned = (PanelHoliday)this.MemberwiseClone();
            return cloned;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// PanelHolidayList
    /// 
    /// <?xml version="1.0"?>
    ///    <PanelInfo>
    ///      <PanelHolidays Holidays="01/10, 05/12"/>
    ///    </PanelInfo>
    ///
    ///    
    /// </summary>
    public class PanelHolidayList : CustomXmlParser
    {
        public ArrayList panelHolidays;
        public String Holidays;

        public PanelHolidayList()
        {
            panelHolidays = new ArrayList();

            sObjectname = "PanelHolidays"; 
            sName = "Holidays";
        }

        ~PanelHolidayList()
        {
            panelHolidays.Clear();
        }
               

        public PanelHolidayList fullCopy()
        {

            PanelHolidayList cloned = new PanelHolidayList();
            for (int i = 0; i < panelHolidays.Count; i++)
                cloned.panelHolidays.Add(((PanelHolidayList)panelHolidays[i]).fullCopy());
            return cloned;
        }

        public PanelHoliday this[UInt32 index]
        {
            get
            {
                if (index < panelHolidays.Count)
                {
                    return (PanelHoliday)panelHolidays[(Int32)index];
                }
                else
                {
                    PanelHoliday panelHoliday = new PanelHoliday();
                    panelHolidays.Add(panelHoliday);
                    return panelHoliday;
                }
            }
            set
            {
                if (index < panelHolidays.Count)
                {
                    PanelHoliday panelHoliday = (PanelHoliday)panelHolidays[(Int32)index];

                    panelHoliday = value;
                }
                else
                {
                    PanelHoliday panelHoliday = new PanelHoliday();

                    panelHoliday = value;

                    panelHolidays.Add(panelHoliday);
                }
            }
        }

        public void Clear()
        {
            panelHolidays.Clear();
        }

        public void serializeXML(ref String xmlObj)
        {
            Holidays = "";

            foreach (PanelHoliday obj in panelHolidays)
            {
                Holidays = Holidays + Convert.ToString(obj.Day) + "/" + Convert.ToString(obj.Month) + ",";
            }
            
            //Remove last comma
            if (Holidays.Length > 0) 
            {
                Holidays = Holidays.Remove(Holidays.Length - 1, 1);
            }                       
            
            StringBuilder output = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            try
            {
                
                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {
                    writer.WriteProcessingInstruction("xml", "version='1.0'");
                    writer.WriteStartElement("PanelInfo");
                    writer.WriteStartElement(sObjectname);
                    writer.WriteStartAttribute(sName); writer.WriteValue(Holidays); writer.WriteEndAttribute();
                    writer.WriteEndElement();
                    writer.WriteFullEndElement();

                    writer.Flush();
                }

                xmlObj = output.ToString();
                
            }
            catch (Exception e)
            {
                
            }                        
        }

        protected internal Boolean parseXML(string xmlString)
        {
            String sType, sname, svalue;

            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        reader.ReadToFollowing("PanelInfo");
                        reader.Read();
                        reader.Read();
                        sType = reader.LocalName;

                        if (sType == "PanelHolidays")
                        {
                            reader.MoveToFirstAttribute();

                            sname = reader.Name;
                            svalue = reader.Value;

                            if (sname.Contains("Holidays"))
                            {
                                //loking for this string format  "01/10, 05/12, 06/30"

                                String[] strValues = svalue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                String[] strValue;
                                int i;

                                foreach (string value in strValues)                                
                                {
                                    PanelHoliday obj = new PanelHoliday();
                                    obj.ItemNo = 1;
                                    panelHolidays.Add(obj);

                                    strValue = value.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                    if (strValue.Length == 2)
                                    {
                                        obj.Day = Convert.ToUInt32(strValue[0].Trim());
                                        obj.Month = Convert.ToUInt32(strValue[1].Trim());
                                    }                                    
                                }
                                                                                                                                                             
                            }                            
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// SiteInfo
    ///    
    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TSiteInfoXML" name="Item1">
            <published>
              <method name="SerialNo" readonly="True" type="UnicodeString"></method>
              <method name="ItemType" readonly="True" type="UnicodeString"></method>
              <method name="IPAddress" readonly="True" type="UnicodeString"></method>
              <method name="HTTPPort" readonly="True" type="Integer">0</method>
              <method name="HTTPSPort" readonly="True" type="Integer">0</method>
              <method name="WebPort" readonly="True" type="Integer">0</method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class SiteInfo : CustomXmlParser
    {
        public String SerialNo;
        public String ItemType;
        public String IPAddress;
        public UInt32 HTTPPort;
        public UInt32 HTTPSPort;
        public UInt32 WebPort;

        public SiteInfo() { sObjectname = "TSiteInfoXML"; sName = "Item"; }

        public SiteInfo fullCopy()
        {
            SiteInfo cloned = (SiteInfo)this.MemberwiseClone();
            return cloned;
        }

        public Boolean parseXML(string xmlString)
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

                                if (sname == "SerialNo") SerialNo = svalue.Trim();
                                else if (sname == "ItemType") ItemType = svalue.Trim();
                                else if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "HTTPPort") HTTPPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "HTTPSPort") HTTPSPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "WebPort") WebPort = Convert.ToUInt32(svalue.Trim());
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// VideoSettings
    ///    
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class VideoSettings : CustomXmlParser
    {
        public String VideoFileDir;       
        public UInt32 VideoFileLifeTime; //Days        

        public VideoSettings() { sObjectname = "TVideoSettingsXML"; sName = "VideoSettings"; }

        public SiteInfo fullCopy()
        {
            SiteInfo cloned = (SiteInfo)this.MemberwiseClone();
            return cloned;
        }

        public Boolean parseXML(string xmlString)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// VideoFile
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
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class VideoFile : CustomXmlParser
    {
        public UInt32 FileID;
        public String FileType;
        public String FileName;
        public String FilePath;
        
        public VideoFile() { sObjectname = "TVideoFileXML"; sName = "File"; }

        public VideoFile fullCopy()
        {
            VideoFile cloned = (VideoFile)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// VideoFileList
    ///    
    /// </summary>
    public class VideoFileList
    {
        public ArrayList videoFiles;

        public VideoFileList()
        {
            videoFiles = new ArrayList();
        }

        ~VideoFileList()
        {
            videoFiles.Clear();
        }

        public VideoFileList fullCopy()
        {

            VideoFileList cloned = new VideoFileList();
            for (int i = 0; i < videoFiles.Count; i++)
                cloned.videoFiles.Add(((VideoFileList)videoFiles[i]).fullCopy());
            return cloned;
        }

        public VideoFile this[UInt32 index]
        {
            get
            {                
                if (index < videoFiles.Count)
                {
                    return (VideoFile)videoFiles[(Int32)index];                    
                }
                else
                    return null;                                
            }
            set
            {
                if (index < videoFiles.Count)
                {
                    VideoFile videoFile = (VideoFile)videoFiles[(Int32)index];

                    videoFile = value;
                }                                
            }
        }

        public void Clear()
        {
            videoFiles.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            VideoFile videoFile = null;

            for (int i = 0; i < videoFiles.Count; i++)
            {
                videoFile = (VideoFile)videoFiles[i];
                videoFile.Name = String.Format("File{0}", i + 1);
                videoFile.serializeXML(ws, output, writer, ref objectCount);
            }            
        }

        protected internal Boolean parseXML(string xmlString)
        {
            string sobject;
            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        videoFiles.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TVideoFileXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    VideoFile obj = new VideoFile();
                                    obj.parseXML(sobject);
                                    videoFiles.Add(obj);
                                }
                            }
                        }
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 
    /// VODSettings
    /*         
     <?xml version="1.0"?>
        <objects>
          <object objectname="TVODSettingsXML" name="Settings">
            <published>
              <method name="IPAddress" readonly="False" type="UnicodeString"></method>
              <method name="IPPort" readonly="False" type="Integer">0</method>
              <method name="ServerPassword" readonly="False" type="UnicodeString">paradox</method>
              <method name="UserName" readonly="False" type="UnicodeString">master</method>
              <method name="VideoFormat" readonly="False" type="UnicodeString">360P256K</method>
            </published>
          </object>
        </objects>
    */
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class VODSettings : CustomXmlParser
    {        
        public String IPAddress;
        public UInt32 IPPort;
        public String ServerPassword;
        public String UserName;
        public String VideoFormat;

        public VODSettings() { sObjectname = "TVODSettingsXML"; sName = "Settings"; }

        public VODSettings fullCopy()
        {
            VODSettings cloned = (VODSettings)this.MemberwiseClone();
            return cloned;
        }

        protected internal Boolean parseXML(string xmlString)
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
                                if (sname == "IPAddress") IPAddress = svalue.Trim();
                                else if (sname == "IPPort") IPPort = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ServerPassword") ServerPassword = svalue.Trim();
                                else if (sname == "UserName") UserName = svalue.Trim();
                                else if (sname == "VideoFormat") VideoFormat = svalue.Trim();                                
                            }
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }

    }

}