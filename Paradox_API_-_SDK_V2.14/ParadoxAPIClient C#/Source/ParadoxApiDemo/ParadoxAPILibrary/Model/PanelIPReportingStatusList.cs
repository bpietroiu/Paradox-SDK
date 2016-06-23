using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    public class PanelIPReportingStatusList
    {
        public ArrayList panelIPReportingStatusList;

        public PanelIPReportingStatusList()
        {
            panelIPReportingStatusList = new ArrayList();
        }

        ~PanelIPReportingStatusList()
        {
            panelIPReportingStatusList.Clear();
        }

        public PanelIPReportingStatusList fullCopy()
        {
            PanelIPReportingStatusList cloned = new PanelIPReportingStatusList();
            for (int i = 0; i < panelIPReportingStatusList.Count; i++)
                cloned.panelIPReportingStatusList.Add(((PanelIPReportingStatusList)panelIPReportingStatusList[i]).fullCopy());
            return cloned;
        }

        public IPReportingStatus this[UInt32 index]
        {
            get
            {
                if (index < panelIPReportingStatusList.Count)
                {
                    return (IPReportingStatus)panelIPReportingStatusList[(int)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (index < panelIPReportingStatusList.Count)
                {
                    IPReportingStatus panelIPReportingStatus = (IPReportingStatus)panelIPReportingStatusList[(int)index];
                    panelIPReportingStatus = value;
                }
                else
                {
                    IPReportingStatus panelIPReportingStatus = new IPReportingStatus();

                    panelIPReportingStatus = value;

                    panelIPReportingStatusList.Add(panelIPReportingStatus);
                }
            }
        }

        public void Clear()
        {
            panelIPReportingStatusList.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (IPReportingStatus obj in panelIPReportingStatusList)
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
                        //panelIPReportingStatusList.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelReportingStatusXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    IPReportingStatus obj = new IPReportingStatus();
                                    obj.parseXML(sobject);
                                    panelIPReportingStatusList.Add(obj);
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