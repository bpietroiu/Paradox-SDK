using System;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Harmony.SDK.Paradox.Model
{
    public class BasePanelModel<T> : IDisposable
    {
        [XmlAttribute(AttributeName = "objectname")]
        public string ObjectName { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        protected void fullCopy(BasePanelModel<T> dest)
        {
            dest.Name = (string)Name.Clone();
            dest.ObjectName = (string)ObjectName.Clone();
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
            if (string.IsNullOrWhiteSpace(fldname))
                return string.Empty;
            if (fldname[0] == 'F')
                return fldname.Substring(1);
            return fldname;
        }

        //Example:  <object objectname="TPanelUserXML" name="User1">
        protected internal bool ParseXml(XmlReader reader)
        {
            try
            {
                reader.MoveToFirstAttribute();
                ObjectName = reader.Value;                
                reader.MoveToNextAttribute();
                Name = reader.Value;
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
                    writer.WriteStartElement("object"); writer.WriteStartAttribute("objectname"); writer.WriteValue(ObjectName); writer.WriteEndAttribute();
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
                    
                writer.WriteStartElement("object"); writer.WriteStartAttribute("objectname"); writer.WriteValue(ObjectName); writer.WriteEndAttribute();
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