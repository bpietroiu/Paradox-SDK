using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    public class PanelEventList
    {
        public ArrayList panelEvents;

        public PanelEventList()
        {
            panelEvents = new ArrayList();
        }

        ~PanelEventList()
        {
            panelEvents.Clear();
        }

        public PanelEventList fullCopy()
        {
            PanelEventList cloned = new PanelEventList();
            for (int i = 0; i < panelEvents.Count; i++)
                cloned.panelEvents.Add(((PanelEventList)panelEvents[i]).fullCopy());
            return cloned;
        }

        public PanelEvent this[UInt32 index]
        {
            get
            {
                if (index < panelEvents.Count)
                {
                    return (PanelEvent)panelEvents[(int)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                /*
                Boolean found = false;

                PanelEvent panelEvent = null;
                
                for (int i = 0; i < panelEvents.Count; i++)
                {
                    panelEvent = (PanelEvent)panelEvents[i];

                    if (panelEvent.SequenceNo == value.SequenceNo)
                    {
                        panelEvent = value;
                        found = true;
                        break;
                    }
                }
                

                if (!found)
                {
                    PanelEvent _PanelEvent = new PanelEvent();

                    _PanelEvent = value;

                    panelEvents.Add(_PanelEvent);
                }
                 */
            }
        }

        public void Clear()
        {
            panelEvents.Clear();
        }

        public void AddEvent(PanelEvent panelEvent)
        {
            PanelEvent obj = new PanelEvent();

            obj = panelEvent;
            panelEvents.Add(obj);
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelEvent obj in panelEvents)
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
                            if (reader.Value == "TPanelEventXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    PanelEvent obj = new PanelEvent();
                                    obj.parseXML(sobject);
                                    panelEvents.Add(obj);
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