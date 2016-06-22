using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
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
                bool found = false;

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
                obj.Name = string.Format("AccessLevel{0}", obj.AccessLevelNo);
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}