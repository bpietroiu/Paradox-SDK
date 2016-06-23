using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
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
                bool found = false;

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
                obj.Name = string.Format("Area{0}", obj.AreaNo);
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}