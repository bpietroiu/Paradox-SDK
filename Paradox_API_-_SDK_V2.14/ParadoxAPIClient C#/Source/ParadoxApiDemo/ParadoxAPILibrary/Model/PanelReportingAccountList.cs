using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    public class PanelReportingAccountList
    {
        public ArrayList panelReportingAccounts;

        public PanelReportingAccountList()
        {
            panelReportingAccounts = new ArrayList();
        }

        ~PanelReportingAccountList()
        {
            panelReportingAccounts.Clear();
        }

        public PanelReportingAccountList fullCopy()
        {
            PanelReportingAccountList cloned = new PanelReportingAccountList();
            for (int i = 0; i < panelReportingAccounts.Count; i++)
                cloned.panelReportingAccounts.Add(((PanelReportingAccountList)panelReportingAccounts[i]).fullCopy());
            return cloned;
        }

        public PanelReportingAccount this[UInt32 index]
        {
            get
            {
                if (index < panelReportingAccounts.Count)
                {
                    return (PanelReportingAccount)panelReportingAccounts[(int)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (index < panelReportingAccounts.Count)
                {
                    PanelReportingAccount panelReportingAccount = (PanelReportingAccount)panelReportingAccounts[(int)index];
                    panelReportingAccount = value;
                }
                else
                {
                    PanelReportingAccount panelReportingAccount = new PanelReportingAccount();

                    panelReportingAccount = value;

                    panelReportingAccounts.Add(panelReportingAccount);
                }
            }
        }

        public void Clear()
        {
            panelReportingAccounts.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelReportingAccount obj in panelReportingAccounts)
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
                        //panelReportingAccounts.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelReportingAccountXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelReportingAccount obj = new PanelReportingAccount();
                                    obj.parseXML(sobject);
                                    panelReportingAccounts.Add(obj);
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