using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    public class PanelUserList
    {
        public ArrayList panelUsers;

        public PanelUserList()
        {
            panelUsers = new ArrayList();
        }

        ~PanelUserList()
        {
            panelUsers.Clear();
        }

        public PanelUserList fullCopy()
        {
            PanelUserList cloned = new PanelUserList();
            for (int i = 0; i < panelUsers.Count; i++)
                cloned.panelUsers.Add(((PanelUserList)panelUsers[i]).fullCopy());
            return cloned;
        }

        public PanelUser this[UInt32 index]
        {
            get
            {
                PanelUser panelUser = null;

                for (int i = 0; i < panelUsers.Count; i++)
                {
                    panelUser = (PanelUser)panelUsers[i];

                    if (panelUser.UserNo == index)
                    {
                        return panelUser;
                    }
                }

                return null;
            }
            set
            {
                bool found = false;

                PanelUser panelUser = null;

                for (int i = 0; i < panelUsers.Count; i++)
                {
                    panelUser = (PanelUser)panelUsers[i];

                    if (panelUser.UserNo == value.UserNo)
                    {
                        panelUser = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    PanelUser pnlUser = new PanelUser();

                    pnlUser = value;

                    panelUsers.Add(pnlUser);
                }
            }
        }

        public void Clear()
        {
            panelUsers.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelUser obj in panelUsers)
            {
                obj.Name = string.Format("User{0}", obj.UserNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        public void serializeXML(ref string xmlObjs)
        {
            UInt32 objectCount = 1;
            StringBuilder output = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(output, ws);

            ws.Indent = true;

            writer.WriteProcessingInstruction("xml", "version='1.0'");
            writer.WriteStartElement("Data");

            foreach (PanelUser obj in panelUsers)
            {
                obj.Name = string.Format("User{0}", obj.UserNo);
                obj.serializeXML(ws, output, writer, ref objectCount);
            }

            writer.WriteFullEndElement(); // "Data"
            writer.Flush();

            xmlObjs = output.ToString();
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
                        panelUsers.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TPanelUserXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelUser obj = new PanelUser();
                                    obj.parseXML(sobject);
                                    panelUsers.Add(obj);
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