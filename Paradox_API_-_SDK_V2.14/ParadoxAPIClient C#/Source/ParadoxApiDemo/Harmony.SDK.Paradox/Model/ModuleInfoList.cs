using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    public class ModuleInfoList
    {
        public ArrayList moduleInfos;

        public ModuleInfoList()
        {
            moduleInfos = new ArrayList();
        }

        ~ModuleInfoList()
        {
            moduleInfos.Clear();
        }

        public ModuleInfoList fullCopy()
        {
            ModuleInfoList cloned = new ModuleInfoList();
            for (int i = 0; i < moduleInfos.Count; i++)
                cloned.moduleInfos.Add(((ModuleInfoList)moduleInfos[i]).fullCopy());
            return cloned;
        }

        public ModuleInfo this[UInt32 index]
        {
            get
            {
                if (index < moduleInfos.Count)
                {
                    return (ModuleInfo)moduleInfos[(int)index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                bool found = false;

                ModuleInfo moduleInfo = null;

                for (int i = 0; i < moduleInfos.Count; i++)
                {
                    moduleInfo = (ModuleInfo)moduleInfos[i];

                    if (moduleInfo.MacString == value.MacString)
                    {
                        moduleInfo = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ModuleInfo _ModuleInfo = new ModuleInfo();

                    _ModuleInfo = value;

                    moduleInfos.Add(_ModuleInfo);
                }
            }
        }

        public void Clear()
        {
            moduleInfos.Clear();
        }

        public void serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {
            foreach (ModuleInfo obj in moduleInfos)
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
                        moduleInfos.Clear();

                        while (reader.ReadToFollowing("object"))
                        {
                            reader.MoveToAttribute("objectname");
                            if (reader.Value == "TModuleInfoXML")
                            {
                                reader.MoveToElement();
                                sobject = reader.ReadOuterXml();
                                if (sobject != "")
                                {
                                    ModuleInfo obj = new ModuleInfo();
                                    obj.parseXML(sobject);
                                    moduleInfos.Add(obj);
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