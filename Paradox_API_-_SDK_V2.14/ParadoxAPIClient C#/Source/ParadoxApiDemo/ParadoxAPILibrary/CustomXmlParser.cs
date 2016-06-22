using System;
using System.Text;
using System.Xml;
using System.Reflection;

namespace ParadoxAPILibrary
{
    /// <summary>
    /// CustomXmlParser Base class 
    /// </summary>
    public class CustomXmlParser<T> : IDisposable
    {
        protected string sObjectname;
        protected string sName;

        public string Objectname 
        { 
            get 
            { 
                return sObjectname; 
            }
            set 
            { 
                sObjectname = value; 
            }
        }

        public string Name 
        { 
            get 
            { 
                return sName; 
            }
            set
            {
                sName = value;
            }
        }

        protected void fullCopy(CustomXmlParser<T> dest)
        {
            dest.sName = (string)sName.Clone();
            dest.sObjectname = (string)sObjectname.Clone();
        }
        
        // public-generic method for performing memberwise object copy
        public T FullCopy()
        {
            var cloned = (T)MemberwiseClone();
            return cloned;
        }

        public void Dispose()
        {           
            GC.SuppressFinalize(this);
        }   

        public bool IsBoolean(string svalue)
        {
            return (svalue.Trim().ToLower() == "true");
        }

        private string propname(string fldname)
        {
            if (fldname == null || fldname == "") return "";
            else if (fldname[0] == 'F') return fldname.Substring(1);
            else return fldname;
        }

        //Example:  <object objectname="TPanelUserXML" name="User1">
        protected internal bool parseXML(XmlReader reader)
        {
            try
            {
                reader.MoveToFirstAttribute();
                sObjectname = reader.Value;                
                reader.MoveToNextAttribute();
                sName = reader.Value;
                return true;
            }
            catch
            {                
                return false;
            }
        }

        public bool serializeXML(ref string xmlObj) 
        {             
            StringBuilder output = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;
            
            try
            {
                // create the Type object
                Type typeObj = this.GetType();

                // declare and populate the arrays to hold the information...
                FieldInfo[] fi = typeObj.GetFields(); //(BindingFlags.Default | BindingFlags.Static | BindingFlags.Public); // fields

                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {
                    writer.WriteProcessingInstruction("xml", "version='1.0'");
                    writer.WriteStartElement("objects");
                    writer.WriteStartElement("object"); writer.WriteStartAttribute("objectname"); writer.WriteValue(Objectname); writer.WriteEndAttribute();
                    writer.WriteStartAttribute("name"); writer.WriteValue(Name); writer.WriteEndAttribute();

                    writer.WriteStartElement("published");

                    // iterate through all the field members
                    foreach (FieldInfo f in fi)
                    {
                        if (f.GetValue(this) != null)
                        {
                            writer.WriteStartElement("method"); writer.WriteStartAttribute("name");
                            writer.WriteValue(propname(f.Name)); writer.WriteEndAttribute();

                            writer.WriteStartAttribute("readonly"); writer.WriteValue("True"); writer.WriteEndAttribute();
                            writer.WriteStartAttribute("type");
                            if (f.FieldType.Name == "string") writer.WriteValue("UnicodeString");
                            else if (f.FieldType.Name == "String") writer.WriteValue("UnicodeString");
                            else if (f.FieldType.Name == "int") writer.WriteValue("Integer");
                            else if (f.FieldType.Name == "UInt32") writer.WriteValue("Integer");
                            else if (f.FieldType.Name == "Int32") writer.WriteValue("Integer");
                            else if (f.FieldType.Name == "Boolean") writer.WriteValue("Boolean");
                            else if (f.FieldType.Name == "DateTime") writer.WriteValue("Double");                                                            
                            else
                                writer.WriteValue("UnknownType?");
                            
                            writer.WriteEndAttribute();

                            if (f.FieldType.Name == "DateTime")
                            {
                                DateTime dt = (DateTime)f.GetValue(this);
                                                                
                                writer.WriteString(Convert.ToString(dt.ToOADate()));                                
                            }
                            else   
                            {
                                writer.WriteString(f.GetValue(this).ToString());                                
                            }

                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();   // </published>
                    writer.WriteEndElement();   // "object"      
                    writer.WriteFullEndElement();   // "objects"                          
                    
                    writer.Flush();
                }

                xmlObj = output.ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool serializeXML(XmlWriterSettings ws, StringBuilder output, XmlWriter writer, ref UInt32 objectCount)
        {            
            try
            {
                // create the Type object
                Type typeObj = this.GetType();

                // declare and populate the arrays to hold the information...
                FieldInfo[] fi = typeObj.GetFields(); //(BindingFlags.Default | BindingFlags.Static | BindingFlags.Public); // fields
                                   
                writer.WriteStartElement(string.Format("objects{0}", objectCount));
                objectCount += 1;
                    
                writer.WriteStartElement("object"); writer.WriteStartAttribute("objectname"); writer.WriteValue(Objectname); writer.WriteEndAttribute();
                writer.WriteStartAttribute("name"); writer.WriteValue(Name); writer.WriteEndAttribute();

                writer.WriteStartElement("published");

                // iterate through all the field members
                foreach (FieldInfo f in fi)
                {
                    if (f.GetValue(this) != null)
                    {
                        writer.WriteStartElement("method"); writer.WriteStartAttribute("name");
                        writer.WriteValue(propname(f.Name)); writer.WriteEndAttribute();

                        writer.WriteStartAttribute("readonly"); writer.WriteValue("True"); writer.WriteEndAttribute();
                        writer.WriteStartAttribute("type");
                        if (f.FieldType.Name == "string") writer.WriteValue("UnicodeString");
                        else if (f.FieldType.Name == "String") writer.WriteValue("UnicodeString");
                        else if (f.FieldType.Name == "int") writer.WriteValue("Integer");
                        else if (f.FieldType.Name == "UInt32") writer.WriteValue("Integer");
                        else if (f.FieldType.Name == "Int32") writer.WriteValue("Integer");
                        else if (f.FieldType.Name == "Boolean") writer.WriteValue("Boolean");
                        else if (f.FieldType.Name == "DateTime") writer.WriteValue("Double");
                        else
                            writer.WriteValue("UnknownType?");

                        writer.WriteEndAttribute();

                        if (f.FieldType.Name == "DateTime")
                        {
                            DateTime dt = (DateTime)f.GetValue(this);

                            writer.WriteString(Convert.ToString(dt.ToOADate()));
                        }
                        else
                        {
                            writer.WriteString(f.GetValue(this).ToString());
                        }

                        writer.WriteEndElement();                        
                    }
                }
                writer.WriteEndElement();   // </published>
                writer.WriteEndElement();   // "object"
                writer.WriteEndElement();   // "objects"                                                  
                                                  
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}