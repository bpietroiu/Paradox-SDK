using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
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
                bool found = false;

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
                obj.Name = string.Format("PGM{0}", obj.PGMNo);
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}