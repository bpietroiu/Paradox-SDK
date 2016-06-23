using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    public class PanelZoneList
    {
        public ArrayList PanelZones;

        public PanelZoneList()
        {
            PanelZones = new ArrayList();
        }

        ~PanelZoneList()
        {
            PanelZones.Clear();
        }

        public PanelZoneList fullCopy()
        {
            PanelZoneList cloned = new PanelZoneList();
            for (int i = 0; i < PanelZones.Count; i++)
                cloned.PanelZones.Add(((PanelZoneList)PanelZones[i]).fullCopy());
            return cloned;
        }

        public PanelZone this[UInt32 index]
        {
            get
            {
                PanelZone panelZone = null;

                for (int i = 0; i < PanelZones.Count; i++)
                {
                    panelZone = (PanelZone)PanelZones[i];

                    if (panelZone.ZoneNo == index)
                    {
                        return panelZone;
                    }
                }

                return null;
            }
            set
            {
                bool found = false;

                PanelZone panelZone = null;

                for (int i = 0; i < PanelZones.Count; i++)
                {
                    panelZone = (PanelZone)PanelZones[i];

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

                    PanelZones.Add(pnlZone);
                }
            }
        }

        public void Clear()
        {
            PanelZones.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelZone obj in PanelZones)
            {
                obj.Name = string.Format("Zone{0}", obj.ZoneNo);
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
                        PanelZones.Clear();

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
                                    PanelZones.Add(obj);
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