using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
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
                bool found = false;

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
                obj.Name = string.Format("Door{0}", obj.DoorNo);
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}