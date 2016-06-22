using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
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

        protected internal bool parseXML(string xmlString)
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}