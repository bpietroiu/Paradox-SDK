using System;
using System.IO;
using System.Xml;

namespace Harmony.SDK.Paradox.Model
{
    #region XML Example

    /*   
       <?xml version="1.0"?>
        <objects>
          <object objectname="TPanelPGMXML" name="PGM1">
            <published>
              <method name="PGMNo" readonly="False" type="Integer">0</method>
              <method name="PGMLabel" readonly="False" type="UnicodeString"></method>
              <method name="PGMTimer" readonly="False" type="Integer">0</method>
              <method name="PGMSerialNo" readonly="False" type="UnicodeString"></method>
              <method name="PGMInputNo" readonly="False" type="Integer">0</method>
              <method name="PGMActivationEvent" readonly="False" type="UnicodeString"></method>
              <method name="PGMDeactivationEvent" readonly="False" type="UnicodeString"></method>
              <method name="PGMActvationMode" readonly="False" type="UnicodeString"></method>
              <method name="PGMPulseEvery30Secs" readonly="False" type="Boolean">False</method>
              <method name="PGMPulseOnAnyAlarm" readonly="False" type="Boolean">False</method>
              <method name="PGMInitialState" readonly="False" type="UnicodeString"></method> 
            </published>
          </object>
        </objects>
    */

    #endregion
    
    public class PanelPGM : BasePanelModel<PanelPGM>
    {
        public UInt32 PGMNo;
        public string PGMLabel;
        public int PGMTimer;
        public string PGMSerialNo;
        public int PGMInputNo;
        public string PGMActivationEvent;
        public string PGMDeactivationEvent;
        public string PGMActvationMode;
        public bool PGMPulseEvery30Secs;
        public bool PGMPulseOnAnyAlarm;
        public string PGMInitialState;
        public string Status;

        public PanelPGM()
        {
            sObjectname = "TPanelPGMXML";
            sName = "PGM";
        }
        
        protected internal bool parseXML(string xmlString)
        {
            string sname, svalue;
            if (xmlString != null)
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    try
                    {
                        reader.ReadToFollowing("object");
                        base.parseXML(reader);

                        while (reader.ReadToFollowing("method"))
                        {
                            reader.MoveToFirstAttribute();
                            sname = reader.Value;

                            if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "method")
                            {
                                svalue = reader.ReadString();
                                if (sname == "PGMNo") PGMNo = Convert.ToUInt32(svalue.Trim());
                                else if (sname == "PGMLabel") PGMLabel = svalue.Trim();
                                else if (sname == "PGMTimer") PGMTimer = Convert.ToInt32(svalue.Trim());
                                else if (sname == "PGMSerialNo") PGMSerialNo = svalue.Trim();
                                else if (sname == "PGMInputNo") PGMInputNo = Convert.ToInt32(svalue.Trim());
                                else if (sname == "PGMActivationEvent") PGMActivationEvent = svalue.Trim();
                                else if (sname == "PGMDeactivationEvent") PGMDeactivationEvent = svalue.Trim();
                                else if (sname == "PGMActvationMode") PGMActvationMode = svalue.Trim();
                                else if (sname == "PGMPulseEvery30Secs") PGMPulseEvery30Secs = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "PGMPulseOnAnyAlarm") PGMPulseEvery30Secs = Convert.ToBoolean(svalue.Trim());
                                else if (sname == "PGMInitialState") PGMInitialState = svalue.Trim();
                            }
                        }

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}