using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace ParadoxAPILibrary
{
    public class ControlPanel
    {
        public UInt32 panelID = 0;
        public System.DateTime dateTime = System.DateTime.FromOADate(0.0);
        public String ConnectionStatus = "DISCONNECTED";
        public String ConnectionStatusInfo = "";
        public Int32 ConnectionProgress = 0;
        public String lastErrorMsg = "";
        public String smsMessageToSend = ""; //received from the API and needed to initiate a GPRS callback communication by sending this message 
        public PanelSettings Settings = new PanelSettings();        
        public PanelInfoEx InfoEx = new PanelInfoEx();
        public PanelAreaList Areas = new PanelAreaList();
        public PanelZoneList Zones = new PanelZoneList();
        public PanelPGMList PGMs = new PanelPGMList();
        public PanelDoorList Doors = new PanelDoorList();
        public PanelUserList Users = new PanelUserList();
        public PanelTroubleList Troubles = new PanelTroubleList(); 
        public PanelMonitoringList PanelMonitoringList = new PanelMonitoringList(); //To keep Monitoring Status history
        public PanelEventList BufferedEvents = new PanelEventList();        
        public PanelIPReportingList PanelIPReportingReceiver = new PanelIPReportingList();
        public PanelAccessLevelList PanelAccessLevels = new PanelAccessLevelList();
        public PanelHolidayList Holidays = new PanelHolidayList();
        public PanelScheduleList PanelSchedules = new PanelScheduleList();

        public ControlPanel()
        {            
            for (Int32 i = 0; i < 2; i++)
            {
                PanelIPReporting panelIPReporting = new PanelIPReporting();
                panelIPReporting.ReceiverNo = i + 1;
                PanelIPReportingReceiver.panelIPReportings.Add(panelIPReporting);
            }
        }
                        
        public void Clear()
        {           
            Areas.Clear();
            Zones.Clear();
            PGMs.Clear();
            Doors.Clear();
            Users.Clear();
            Troubles.Clear();
            PanelMonitoringList.Clear();
            BufferedEvents.Clear();
        }

        public UInt32 GetTotalBufferedEvent()
        {
            if (
                (InfoEx.Description == ParadoxAPI.CP_PROD_NAME_DGP_EVO_192) ||
                (InfoEx.Description == ParadoxAPI.CP_PROD_NAME_DGP_EVO_VHD)  
                )
            {
                return 2048;
            }
            else
            {
                return 256;
            }
        }

        public Boolean SupportAccessControl()
        {
            if (
                (InfoEx.Description == ParadoxAPI.CP_PROD_NAME_DGP_EVO_192) ||
                (InfoEx.Description == ParadoxAPI.CP_PROD_NAME_DGP_EVO_VHD)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int32 SaveToFile(String fileName, Boolean overWrite)
        {
            if (File.Exists(fileName) && !overWrite)
            {
                return -1;                
            }
            else 
            {
                UInt32 objectCount = 1;
                XmlDocument doc = new XmlDocument();
                StringBuilder output = new StringBuilder();
                XmlWriterSettings ws = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(output, ws) ;
                
                try
                {
                    ws.Indent = true;

                    writer.WriteProcessingInstruction("xml", "version='1.0'");
                    writer.WriteStartElement("Data");

                    Settings.serializeXML(ws, output, writer, ref objectCount);
                    InfoEx.serializeXML(ws, output, writer, ref objectCount);                    

                    writer.WriteFullEndElement(); // "Data"
                    writer.Flush();

                    doc.InnerXml = output.ToString();
                    doc.Save(fileName);
                    return 0;
                }
                catch (Exception e)
                {
                    return -1;
                }                
            }    
        }
               

        public Int32 LoadFromFile(String fileName)
        {
            if (!File.Exists(fileName))
            {
                return -1;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                String xml = doc.InnerXml;
                Clear();
                Settings.parseXML(xml);
                InfoEx.parseXML(xml);                
                return 0;
            }                       
        }

        public Int32 DeleteFile(String fileName)
        {
            if (!File.Exists(fileName))
            {
                return -1;
            }
            else 
            {
                File.Delete(fileName);                
                
                return 0;                
            }
        }                

        public void UpdateStatus(PanelMonitoring panelMonitoring)
        {
            //To keep Monitoring Status history
            //panelMonitoringList.panelMonitorings.Add(panelMonitoring);

            if (panelMonitoring.ItemType == PanelMonitoring.C_MONITORING_AREA_ITEM_TYPE)
            {
                Areas[panelMonitoring.ItemNo].Status = panelMonitoring.Status;
            }
            else if (panelMonitoring.ItemType == PanelMonitoring.C_MONITORING_ZONE_ITEM_TYPE)
            {
                Zones[panelMonitoring.ItemNo].Status = panelMonitoring.Status;
            }
            else if (panelMonitoring.ItemType == PanelMonitoring.C_MONITORING_PGM_ITEM_TYPE)
            {
                PGMs[panelMonitoring.ItemNo].Status = panelMonitoring.Status;
            }
            else if (panelMonitoring.ItemType == PanelMonitoring.C_MONITORING_DOOR_ITEM_TYPE)
            {
                Doors[panelMonitoring.ItemNo].Status = panelMonitoring.Status;
            }
        }        
    }    
}
