using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace ParadoxAPILibrary.Model
{
    #region XML Example

    /*
    <? xml version="1.0"?>
    <PanelInfo>
        <PanelTrouble Trouble1 = "Battery Failure" Trouble2="IP Receiver 1 Fail to comm" Trouble3="Zone Tampered:2907586A" Trouble4="Module Failed to communicate:1FF008CF"/>
    </PanelInfo>
    */

    #endregion
    
    public class PanelTroubleList
    {
        public ArrayList panelTroubles;

        public PanelTroubleList()
        {
            panelTroubles = new ArrayList();
        }

        ~PanelTroubleList()
        {
            panelTroubles.Clear();
        }

        public PanelTroubleList fullCopy()
        {
            PanelTroubleList cloned = new PanelTroubleList();
            for (int i = 0; i < panelTroubles.Count; i++)
                cloned.panelTroubles.Add(((PanelTroubleList)panelTroubles[i]).fullCopy());
            return cloned;
        }

        public PanelTrouble this[UInt32 index]
        {
            get
            {
                if (index < panelTroubles.Count)
                {
                    return (PanelTrouble)panelTroubles[(Int32)index];
                }
                else
                    return null;
            }
            set
            {
                if (index < panelTroubles.Count)
                {
                    PanelTrouble panelTrouble = (PanelTrouble)panelTroubles[(Int32)index];

                    panelTrouble = value;
                }
                else
                {
                    PanelTrouble panelTrouble = new PanelTrouble();

                    panelTrouble = value;

                    panelTroubles.Add(panelTrouble);
                }
            }
        }

        public void Clear()
        {
            panelTroubles.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (PanelTrouble obj in panelTroubles)
            {
                obj.serializeXML(ws, output, writer, ref objectCount);
            }
        }

        protected internal bool parseXML(string xmlString)
        {
            string sType, sname, svalue, sItemNo;

            if (xmlString != null)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        reader.ReadToFollowing("PanelInfo");
                        reader.Read();
                        reader.Read();
                        sType = reader.LocalName;

                        if (sType == "PanelTrouble")
                        {
                            reader.MoveToFirstAttribute();

                            sname = reader.Name;
                            svalue = reader.Value;

                            if (sname.Contains("Trouble"))
                            {
                                sItemNo = sname.Remove(0, 7);
                                PanelTrouble obj = new PanelTrouble();
                                obj.ItemNo = Convert.ToUInt32(sItemNo);
                                obj.Status = svalue;
                                panelTroubles.Add(obj);
                            }

                            while (reader.MoveToNextAttribute())
                            {
                                sname = reader.Name;
                                svalue = reader.Value;

                                if (sname.Contains("Trouble"))
                                {
                                    sItemNo = sname.Remove(0, 7);
                                    PanelTrouble obj = new PanelTrouble();
                                    obj.ItemNo = Convert.ToUInt32(sItemNo);
                                    obj.Status = svalue;
                                    panelTroubles.Add(obj);
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