using System;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

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

    #endregion

    public class PanelSchedule : CustomXmlParser<PanelSchedule>
    {
        public UInt32 ScheduleNo;
        public string ScheduleLabel;
        public Int32 ScheduleBackupNo;
        public DateTime ScheduleStartTimeIntervalA;
        public DateTime ScheduleEndTimeIntervalA;
        public string ScheduleDaysIntervalA;
        public DateTime ScheduleStartTimeIntervalB;
        public DateTime ScheduleEndTimeIntervalB;
        public string ScheduleDaysIntervalB;

        public PanelSchedule()
        {
            sObjectname = "TPanelScheduleXML";
            sName = "Schedule";
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
                                if (sname == "ScheduleNo") ScheduleNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "ScheduleLabel") ScheduleLabel = svalue.Trim();
                                else if (sname == "ScheduleBackupNo") ScheduleBackupNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "ScheduleStartTimeIntervalA")
                                {
                                    try
                                    {
                                        ScheduleStartTimeIntervalA = System.DateTime.FromOADate(Convert.ToDouble(svalue.Trim()));
                                    }
                                    catch
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
                                    catch
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
                                    catch
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
                                    catch
                                    {
                                        ScheduleEndTimeIntervalB = System.DateTime.FromOADate(0.0);
                                    }
                                }
                                else if (sname == "ScheduleDaysIntervalB") ScheduleDaysIntervalB = svalue.Trim();
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