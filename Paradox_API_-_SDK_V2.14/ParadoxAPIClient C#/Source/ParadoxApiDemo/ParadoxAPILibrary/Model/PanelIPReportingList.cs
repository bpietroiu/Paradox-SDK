using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
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
                    return (PanelIPReporting)panelIPReportings[(int)(index - 1)];
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
                    PanelIPReporting panelIPReporting = (PanelIPReporting)panelIPReportings[(int)index - 1];
                    panelIPReporting.ReceiverNo = (int)index;
                    panelIPReporting = value;
                }
                else
                {
                    PanelIPReporting panelIPReporting = new PanelIPReporting();
                    panelIPReporting.ReceiverNo = (int)index;
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

        protected internal bool parseXML(string xmlString)
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
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}