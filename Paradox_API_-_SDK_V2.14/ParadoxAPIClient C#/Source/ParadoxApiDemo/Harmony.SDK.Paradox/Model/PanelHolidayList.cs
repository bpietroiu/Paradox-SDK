using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*
    <? xml version="1.0"?>
    <PanelInfo>
        <PanelHolidays Holidays = "01/10, 05/12" />
    </ PanelInfo >
    */

    #endregion
    
    public class PanelHolidayList : BasePanelModel<PanelHolidayList>
    {
        public ArrayList panelHolidays;
        public string Holidays;

        public PanelHolidayList()
        {
            panelHolidays = new ArrayList();

            ObjectName = "PanelHolidays";
            Name = "Holidays";
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
                    return (PanelHoliday)panelHolidays[(int)index];
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
                    PanelHoliday panelHoliday = (PanelHoliday)panelHolidays[(int)index];

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

        public void serializeXML(ref string xmlObj)
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
                    writer.WriteStartElement(ObjectName);
                    writer.WriteStartAttribute(Name); writer.WriteValue(Holidays); writer.WriteEndAttribute();
                    writer.WriteEndElement();
                    writer.WriteFullEndElement();

                    writer.Flush();
                }

                xmlObj = output.ToString();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }
        }

        protected internal bool parseXML(string xmlString)
        {
            string sType, sname, svalue;

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
                                string[] strValues = svalue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                foreach (string value in strValues)
                                {
                                    PanelHoliday obj = new PanelHoliday();
                                    obj.ItemNo = 1;
                                    panelHolidays.Add(obj);

                                    var strValue = value.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}