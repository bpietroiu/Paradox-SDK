using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
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
                bool found = false;

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
                obj.Name = string.Format("Schedule{0}", obj.ScheduleNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal bool parseXML(string xmlString)
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}