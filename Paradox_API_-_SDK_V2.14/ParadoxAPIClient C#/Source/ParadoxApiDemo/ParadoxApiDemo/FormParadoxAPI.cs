using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;
using Harmony.SDK.Paradox;
using Harmony.SDK.Paradox.Model;
using ParadoxApiDemo.ViewModel;

namespace ParadoxApiDemo
{
    public partial class FormParadoxAPI : Form
    {
        //UInt32 mPanelID = 0x01; //For the menu items - PanelID test variable needed to keep a reference on a communication link  

        public List<ControlPanelViewModel> controlPanels = new List<ControlPanelViewModel>();
        IPDOXSettings ipDOXSettings = new IPDOXSettings();

        public FormParadoxAPI()
        {
            InitializeComponent();

            ParadoxAPI.formRef = this;

            bool exists = Directory.Exists("Data");

            if (!exists)
                Directory.CreateDirectory("Data");

            exists = Directory.Exists("Video");

            if (!exists)
                Directory.CreateDirectory("Video");

            DirectoryInfo directoryInfo = Directory.GetParent("Video");

            if (directoryInfo.Exists)
            {
                textBoxVideoFileDirectory.Text = directoryInfo.FullName + "\\Video";
            }

            dateTimePickerVideoAlarm.Value = System.DateTime.Now;

            LoadPanelFromFile();

            UpdateUI();

            ParadoxAPI.MonitoringStatusChanges = monitoringStatusChangesDelegate;
            ParadoxAPI.heartbeatDelegate = heartbeatDelegate;
            ParadoxAPI.receiveLiveEventDelegate = receiveLiveEventDelegate;
            ParadoxAPI.receiveBufferedEventDelegate = receiveBufferedEventDelegate;
            ParadoxAPI.receiveReportingEventDelegate = receiveReportingEventDelegate;
            ParadoxAPI.progressChangedDelegate = progressChangedDelegate;
            ParadoxAPI.progressErrorDelegate = progressErrorDelegate;
            ParadoxAPI.connectionStatusChangedDelegate = connectionStatusChangedDelegate;
            ParadoxAPI.rxStatusChangedDelegate = rxStatusChangedDelegate;
            ParadoxAPI.txStatusChangedDelegate = txStatusChangedDelegate;
            ParadoxAPI.iPModuleDetectedDelegate = iPModuleDetectedDelegate;
            ParadoxAPI.smsRequestDelegate = smsRequestDelegate;
            ParadoxAPI.accountRegistrationDelegate = accountRegistrationDelegate;
            ParadoxAPI.accountUpdateDelegate = accountUpdateDelegate;
            ParadoxAPI.accountLinkDelegate = accountLinkDelegate;
            ParadoxAPI.ipDOXSocketChangedDelegate = ipDOXSocketChangedDelegate;

            ParadoxAPI.RegisterAllCallback();

            ParadoxAPI.logDelegate = LogDelegate;
            ParadoxAPI.notifyTaskCompletedDelegate = NotifyTaskCompletedDelegate;
            ParadoxAPI.refreshNotificationDelegate = RefreshNotificationDelegate;
            ParadoxAPI.errorNotificationDelegate = ErrorNotificationDelegate; 
           
            String version = "";

            ParadoxAPI.GetAPIVersion(ref version);

            this.Text = this.Text + " - Version " + version;

            FillDoorAccessLevels();
            FillDoorHolidays();            
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void DisconnectFromControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.DisconnectFromPanel(controlPanel.panelID);
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ConnectToControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.ConnectToPanel(controlPanel.panelID, controlPanel.Settings);

            if (PanelResults.Succeeded((UInt32)returnValue))
            {
                PanelInfoEx panelInfoEx = new PanelInfoEx();

                returnValue = ParadoxAPI.RetrievePanelInfo(controlPanel.panelID, panelInfoEx);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    controlPanel.InfoEx = panelInfoEx.FullCopy();

                    controlPanel.Settings.PanelType = controlPanel.InfoEx.Description;

                    ReadDateTimeFromControlPanel(controlPanel);

                    UpdatePanel(controlPanel);
                }
            }
        }               

        private bool GetControlPanel(UInt32 panelID, ref ControlPanelViewModel controlPanel)
        {
            Int32 pnlID = (Int32)panelID - 1;

            if ((pnlID > -1) && (pnlID < controlPanels.Count()))
            {
                controlPanel = controlPanels.ElementAt(pnlID);

                return (controlPanel != null);
            }
            else
            {
                return false;
            }
        }

        private bool GetSelectedControlPanel(UInt32 panelID, ref ControlPanelViewModel controlPanel)
        {
            if ((tvContolPanels as TreeView).SelectedNode != null)
            {
                TreeNode treeNode = (tvContolPanels as TreeView).SelectedNode;

                switch (treeNode.Level)
                {
                    case 0:
                        if (treeNode.Index < controlPanels.Count())
                        {
                            controlPanel = controlPanels.ElementAt(treeNode.Index);

                            return (controlPanel.panelID == panelID);
                        }
                        else
                        {
                           return false; 
                        }

                    case 1:
                        TreeNode treeNodeParent = treeNode.Parent;

                        if (treeNodeParent.Index < controlPanels.Count())
                        {
                            controlPanel = controlPanels.ElementAt(treeNodeParent.Index);

                            return (controlPanel.panelID == panelID);
                        }
                        else
                        {
                           return false; 
                        }
                       
                    default: 
                        {
                            return false;
                        }
                }
            }
            else
            {
                return false;
            }                       
        }

        private bool GetSelectedControlPanel(ref ControlPanelViewModel controlPanel)
        {
            if ((tvContolPanels as TreeView).SelectedNode != null)
            {
                TreeNode treeNode = (tvContolPanels as TreeView).SelectedNode;

                switch (treeNode.Level)
                {
                    case 0:
                        if (treeNode.Index < controlPanels.Count())
                        {
                            controlPanel = controlPanels.ElementAt(treeNode.Index);

                            return (controlPanel != null);
                        }
                        else
                        {
                           return false; 
                        }

                    case 1:
                        TreeNode treeNodeParent = treeNode.Parent;

                        if (treeNodeParent.Index < controlPanels.Count())
                        {
                            controlPanel = controlPanels.ElementAt(treeNodeParent.Index);

                            return (controlPanel != null);
                        }
                        else
                        {
                           return false; 
                        }

                    default: 
                        {
                            return false;
                        }
                }
            }
            else
            {
                return false;
            }                    
        }

        private Boolean GetSelectedControlPanelConnected(ref ControlPanelViewModel controlPanel)
        {
            if ((tvContolPanels as TreeView).SelectedNode != null)
            {
                TreeNode treeNode = (tvContolPanels as TreeView).SelectedNode;

                switch (treeNode.Level)
                {
                    case 0:
                        if (treeNode.Index < controlPanels.Count())
                        {
                            controlPanel = controlPanels.ElementAt(treeNode.Index);

                            return (controlPanel.ConnectionStatus == "CONNECTED");
                        }
                        else
                        {
                           return false; 
                        }                        
                    case 1:
                        TreeNode treeNodeParent = treeNode.Parent;

                        if (treeNodeParent.Index < controlPanels.Count())
                        {
                            controlPanel = controlPanels.ElementAt(treeNodeParent.Index);

                            return (controlPanel.ConnectionStatus == "CONNECTED");
                        }
                        else
                        {
                           return false; 
                        }

                    default: 
                        {
                            return false;
                        }
                }
            }
            else
            {
                return false;
            }                              
        }

        private Int32 SaveIPDOXSettingsToFile(String fileName = "IPDOXSettings.xml", Boolean overWrite = true)
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
                XmlWriter writer = XmlWriter.Create(output, ws);

                try
                {
                    ws.Indent = true;

                    writer.WriteProcessingInstruction("xml", "version='1.0'");
                    writer.WriteStartElement("Data");

                    ipDOXSettings.serializeXML(ws, output, writer, ref objectCount);
                    
                    writer.WriteFullEndElement(); // "Data"
                    writer.Flush();

                    doc.InnerXml = output.ToString();
                    doc.Save(fileName);
                    return 0;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        private void LoadPanelFromFile()
        {
            List<String> files = new List<String>();

            String[] filePaths = Directory.GetFiles(@"Data\", "*.xml");

            String ctrlstr = "ControlPanelID_";
            Int32 lPanelID = 0;
            String lStr = "";


            foreach (String file in filePaths)
            {
                String substr = "";

                Int32 pos = file.IndexOf(".xml");
                if (pos > 0)
                {
                    substr = file.Remove(pos, 4);
                }

                pos = substr.IndexOf("ControlPanelID_");
                if (pos > 0)
                {
                    pos = pos + ctrlstr.Length;
                    lStr = substr.Remove(0, pos);
                    lPanelID = Convert.ToInt32(lStr);                    
                    files.Add("Data\\ControlPanelID_" + String.Format("{0:D4}", lPanelID));
                }
            }

            files.Sort();

            foreach (String str in files)
            {
                Int32 pos = str.IndexOf("ControlPanelID_");
                if (pos >= 0)
                {
                    pos = pos + ctrlstr.Length;
                    lStr = str.Remove(0, pos);
                    lPanelID = Convert.ToInt32(lStr);

                    var controlPanel = new ControlPanelViewModel();
                    controlPanel.panelID = (UInt32)lPanelID;
                    controlPanels.Add(controlPanel);
                    lStr = "Data\\ControlPanelID_" + String.Format("{0}", lPanelID) + ".xml";                                       
                    controlPanel.LoadFromFile(lStr);
                }
                else
                {
                    pos = str.IndexOf("IPDOXSettings");
                    if (pos >= 0)
                    {
                        if (File.Exists(str))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(str);
                            String xml = doc.InnerXml;
                            ipDOXSettings.parseXML(xml);
                            UpdateIPDOXSettingsUI();
                        }
                    }
                }
            }                        
        }

        private void UpdateUIOnNewPanel(ControlPanelViewModel controlPanel)
        {
            
            TreeNode treeNode = new TreeNode(String.Format("Control Panel (PanelID = {0:D4})", controlPanel.panelID));

            if (controlPanel.Settings.PanelType != null)
            {
                treeNode.Nodes.Add(new TreeNode("Panel type: " + controlPanel.Settings.PanelType));
            }
            else
            {
                treeNode.Nodes.Add(new TreeNode("Panel Type: Auto-Detect"));
            }

            if (controlPanel.Settings.SystemAlarmLanguage != null)
            {
                treeNode.Nodes.Add(new TreeNode("System Alarm Language: " + controlPanel.Settings.SystemAlarmLanguage));
            }
            else
            {
                treeNode.Nodes.Add(new TreeNode("System Alarm Language: English"));
            }

            if (controlPanel.Settings.ComType == "SERIAL")
            {
                treeNode.Nodes.Add(new TreeNode("Communication Type: SERIAL"));

                treeNode.Nodes.Add(new TreeNode("COM Port: " + Convert.ToString(controlPanel.Settings.ComPort)));
                treeNode.Nodes.Add(new TreeNode("Baud Rate: " + Convert.ToString(controlPanel.Settings.BaudRate)));
                treeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                tvContolPanels.Nodes.Add(treeNode);

            }
            else if ((controlPanel.Settings.ComType == "IP") && !controlPanel.Settings.SMSCallback)
            {
                treeNode.Nodes.Add(new TreeNode("Communication Type: IP/Static"));

                treeNode.Nodes.Add(new TreeNode("IP Address: " + controlPanel.Settings.IPAddress));
                treeNode.Nodes.Add(new TreeNode("IP Port: " + Convert.ToString(controlPanel.Settings.IPPort)));
                treeNode.Nodes.Add(new TreeNode("IP Password: " + controlPanel.Settings.IPPassword));
                treeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                tvContolPanels.Nodes.Add(treeNode);

            }
            else if ((controlPanel.Settings.ComType == "IP") && controlPanel.Settings.SMSCallback)
            {
                //GPRS callback
                treeNode.Nodes.Add(new TreeNode("Communication Type: GPRS/Callback"));

                treeNode.Nodes.Add(new TreeNode("IP Address: " + controlPanel.Settings.IPAddress));
                treeNode.Nodes.Add(new TreeNode("IP Port: " + Convert.ToString(controlPanel.Settings.IPPort)));
                treeNode.Nodes.Add(new TreeNode("IP Password: " + controlPanel.Settings.IPPassword));
                treeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                tvContolPanels.Nodes.Add(treeNode);

            }
            else if (controlPanel.Settings.ComType == "DNS")
            {
                treeNode.Nodes.Add(new TreeNode("Communication Type: DNS"));

                treeNode.Nodes.Add(new TreeNode("Site ID: " + controlPanel.Settings.SiteID));
                treeNode.Nodes.Add(new TreeNode("IP Password: " + controlPanel.Settings.IPPassword));
                treeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                tvContolPanels.Nodes.Add(treeNode);

            }
            else
            {
                treeNode.Nodes.Add(new TreeNode("Communication Type: SERIAL"));

                treeNode.Nodes.Add(new TreeNode("COM Port: " + Convert.ToString(controlPanel.Settings.ComPort)));
                treeNode.Nodes.Add(new TreeNode("Baud Rate: " + Convert.ToString(controlPanel.Settings.BaudRate)));
                treeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                tvContolPanels.Nodes.Add(treeNode);
            }

            treeNode.TreeView.Sort();

            FillControlPanel(tvControlPanelStatus, listViewUsers, controlPanel);
                      
        }

        private void UpdateUIOnPanelConnected(ControlPanelViewModel controlPanel)
        {
            ControlPanelViewModel ctrlPanel = null;

            TreeNode workingTreeNode = null;

            for (int i = 0; i < (tvContolPanels as TreeView).GetNodeCount(false); i++)
            {
                workingTreeNode = (tvContolPanels as TreeView).Nodes[i];

                ctrlPanel = controlPanels.ElementAt(workingTreeNode.Index);

                if (ctrlPanel.panelID == controlPanel.panelID)
                {
                    break;
                }

            }

            if (workingTreeNode != null)

            {
                if (controlPanel.ConnectionStatus == "CONNECTED")
                {
                    workingTreeNode.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    workingTreeNode.ForeColor = System.Drawing.Color.Black;
                }

                (tvContolPanels as TreeView).Invalidate();
            }
            
        }

        private void UpdateUIOnPanelChanged(ControlPanelViewModel controlPanel)
        {
            TreeNode workingTreeNode = null;
               
            tvContolPanels.BeginUpdate();
            try
            {
                if ((tvContolPanels as TreeView).SelectedNode != null)
                {
                    TreeNode treeNode = (tvContolPanels as TreeView).SelectedNode;

                    ControlPanelViewModel ctrlPanel = null;

                    switch (treeNode.Level)
                    {
                        case 0:
                            if (treeNode.Index < controlPanels.Count())
                            {
                                ctrlPanel = controlPanels.ElementAt(treeNode.Index);

                                if (ctrlPanel.panelID == controlPanel.panelID)
                                {
                                    workingTreeNode = treeNode;
                                }
                            }
                            break;
                        case 1:
                            TreeNode treeNodeParent = treeNode.Parent;

                            if (treeNodeParent.Index < controlPanels.Count())
                            {
                                ctrlPanel = controlPanels.ElementAt(treeNodeParent.Index);

                                if (ctrlPanel.panelID == controlPanel.panelID)
                                {
                                    workingTreeNode = treeNodeParent;
                                }
                            }
                            break;
                    }
                }


                if (workingTreeNode != null)
                {
                    workingTreeNode.Nodes.Clear();

                    if (controlPanel.Settings.PanelType != null)
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("Panel type: " + controlPanel.Settings.PanelType));
                    }
                    else
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("Panel Type: Auto-Detect"));
                    }

                    if (controlPanel.Settings.SystemAlarmLanguage != null)
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("System Alarm Language: " + controlPanel.Settings.SystemAlarmLanguage));
                    }
                    else
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("System Alarm Language: English"));
                    }


                    if (controlPanel.Settings.ComType == "SERIAL")
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("Communication Type: SERIAL"));

                        workingTreeNode.Nodes.Add(new TreeNode("COM Port: " + Convert.ToString(controlPanel.Settings.ComPort)));
                        workingTreeNode.Nodes.Add(new TreeNode("Baud Rate: " + Convert.ToString(controlPanel.Settings.BaudRate)));
                        workingTreeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));
                    }
                    else if ((controlPanel.Settings.ComType == "IP") && !controlPanel.Settings.SMSCallback)
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("Communication Type: IP/Static"));

                        workingTreeNode.Nodes.Add(new TreeNode("IP Address: " + controlPanel.Settings.IPAddress));
                        workingTreeNode.Nodes.Add(new TreeNode("IP Port: " + Convert.ToString(controlPanel.Settings.IPPort)));
                        workingTreeNode.Nodes.Add(new TreeNode("IP Password: " + controlPanel.Settings.IPPassword));
                        workingTreeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                    }
                    else if ((controlPanel.Settings.ComType == "IP") && controlPanel.Settings.SMSCallback)
                    {
                        //GPRS callback
                        workingTreeNode.Nodes.Add(new TreeNode("Communication Type: GPRS/Callback"));

                        workingTreeNode.Nodes.Add(new TreeNode("IP Address: " + controlPanel.Settings.IPAddress));
                        workingTreeNode.Nodes.Add(new TreeNode("IP Port: " + Convert.ToString(controlPanel.Settings.IPPort)));
                        workingTreeNode.Nodes.Add(new TreeNode("IP Password: " + controlPanel.Settings.IPPassword));
                        workingTreeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                    }
                    else if (controlPanel.Settings.ComType == "DNS")
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("Communication Type: DNS"));

                        workingTreeNode.Nodes.Add(new TreeNode("Site ID: " + controlPanel.Settings.SiteID));
                        workingTreeNode.Nodes.Add(new TreeNode("IP Password: " + controlPanel.Settings.IPPassword));
                        workingTreeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));

                    }
                    else
                    {
                        workingTreeNode.Nodes.Add(new TreeNode("Communication Type: SERIAL"));

                        workingTreeNode.Nodes.Add(new TreeNode("COM Port: " + Convert.ToString(controlPanel.Settings.ComPort)));
                        workingTreeNode.Nodes.Add(new TreeNode("Baud Rate: " + Convert.ToString(controlPanel.Settings.BaudRate)));
                        workingTreeNode.Nodes.Add(new TreeNode("User Code: " + controlPanel.Settings.UserCode));
                    }
                }
            }
            finally
            {
                tvContolPanels.EndUpdate();
            }            
        }

        private void DeletePanel(ControlPanelViewModel controlPanel)
        {
            TreeNode workingTreeNode = null;

            if ((tvContolPanels as TreeView).SelectedNode != null)
            {
                TreeNode treeNode = (tvContolPanels as TreeView).SelectedNode;

                ControlPanelViewModel ctrlPanel = null;

                switch (treeNode.Level)
                {
                    case 0:
                        if (treeNode.Index < controlPanels.Count())
                        {
                            ctrlPanel = controlPanels.ElementAt(treeNode.Index);

                            if (ctrlPanel.panelID == controlPanel.panelID)
                            {
                                workingTreeNode = treeNode;
                            }
                        }
                        break;
                    case 1:
                        TreeNode treeNodeParent = treeNode.Parent;

                        if (treeNodeParent.Index < controlPanels.Count())
                        {
                            ctrlPanel = controlPanels.ElementAt(treeNodeParent.Index);

                            if (ctrlPanel.panelID == controlPanel.panelID)
                            {
                                workingTreeNode = treeNodeParent;
                            }
                        }
                        break;
                }
            }

            if (workingTreeNode != null)
            {
                workingTreeNode.Remove();
                controlPanels.Remove(controlPanel);

            }            
        }

        private void UpdateControlPanelStatusMonitoring(ControlPanelViewModel controlPanel, PanelMonitoring panelMonitoring)
        {
            controlPanel.UpdateStatus(panelMonitoring);

            if (GetSelectedControlPanel(controlPanel.panelID, ref controlPanel))
            {
                UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel);
            }
        }

        private void UpdateControlPanelStatusView(TreeView treeView, ControlPanelViewModel controlPanel, UInt32 ItemNo, String ItemType)
        {
            TreeNode treeNode = null;
            String sName = "";

            if (ItemNo == 0)
                return;

            if (treeView.Nodes.Count == 0)
            {
                FillControlPanelStatusView(treeView, controlPanel);
            }

            if (treeView.Nodes.Count == 0)
                return;

            treeView.BeginUpdate();
            try
            {
                if (ItemType == PanelObjectTypes.OT_AREA)
                {
                    if (ItemNo <= controlPanel.Areas.panelAreas.Count)
                    {
                        treeNode = treeView.Nodes[0];

                        PanelArea panelArea = controlPanel.Areas[ItemNo];

                        if (panelArea != null)
                        {
                            sName = "Area " + Convert.ToString(panelArea.AreaNo);

                            TreeNode[] tns = treeView.Nodes.Find("treeNode" + "Area" + Convert.ToString(panelArea.AreaNo), true);

                            if (tns.Count() > 0)
                            {
                                tns[0].Text = sName + ": (" + panelArea.AreaLabel + ") " + "   [" + panelArea.Status + " ]   " +
                                                "Enabled: " + Convert.ToString(panelArea.AreaEnabled) + ", " +
                                                "Exit Delay: " + Convert.ToString(panelArea.AreaExitDelay) + ", " +
                                                "Bell Cut- Off Timer: " + Convert.ToString(panelArea.AreaBellCutOffTimer) + ", " +
                                                "Auto-Arming Time: " + Convert.ToString(panelArea.AreaAutoArmingTime) + ", " +
                                                "No Movement: " + Convert.ToString(panelArea.AreaNoMovementTimer);
                            }
                        }

                    }
                }
                else if (ItemType == PanelObjectTypes.OT_ZONE)
                {
                    if (ItemNo <= controlPanel.Zones.PanelZones.Count)
                    {
                        treeNode = treeView.Nodes[0];

                        PanelZone panelZone = controlPanel.Zones[ItemNo];

                        if (panelZone != null)
                        {
                            sName = "Zone " + Convert.ToString(panelZone.ZoneNo);

                            TreeNode[] tns = treeView.Nodes.Find("treeNode" + "Zone" + Convert.ToString(panelZone.ZoneNo), true);

                            if (tns.Count() > 0)
                            {
                                tns[0].Text = sName + ":  (" + panelZone.ZoneLabel + ") " + "   [" + panelZone.Status + " ]   " +
                                                "Enabled: " + Convert.ToString(panelZone.ZoneEnabled) + ", " +
                                                "Serial #: " + panelZone.ZoneSerialNo + ", " +
                                                "Input #: " + Convert.ToString(panelZone.ZoneInputNo) + ", " +
                                                "Area #: " + Convert.ToString(panelZone.ZonePartition) + ", " +
                                                "Definition: " + panelZone.ZoneDefinition + ", " +
                                                "Alarm Type: " + panelZone.ZoneAlarmType + ", " +
                                                "Stay: " + Convert.ToString(panelZone.ZoneStay) + ", " +
                                                "Force: " + Convert.ToString(panelZone.ZoneForce) + ", " +
                                                "Bypass: " + Convert.ToString(panelZone.ZoneBypass) + ", " +
                                                "Auto-Shutdown: " + Convert.ToString(panelZone.ZoneAutoShutdown) + ", " +
                                                "RF-Supervision: " + Convert.ToString(panelZone.ZoneRFSupervision) + ", " +
                                                "Intellizone: " + Convert.ToString(panelZone.ZoneIntellizone) + ", " +
                                                "Delay Before Transmission: " + Convert.ToString(panelZone.ZoneDelayBeforeTransmission);
                            }
                        }
                    }
                }
                else if (ItemType == PanelObjectTypes.OT_PGM)
                {
                    if (ItemNo <= controlPanel.PGMs.panelPGMs.Count)
                    {
                        treeNode = treeView.Nodes[0];

                        PanelPGM panelPGM = controlPanel.PGMs[ItemNo];

                        if (panelPGM != null)
                        {
                            sName = "PGM " + Convert.ToString(panelPGM.PGMNo);

                            TreeNode[] tns = treeView.Nodes.Find("treeNode" + "PGM" + Convert.ToString(panelPGM.PGMNo), true);

                            if (tns.Count() > 0)
                            {
                                tns[0].Text = sName + ": (" + panelPGM.PGMLabel + ") " + "   [" + panelPGM.Status + " ]   " +
                                                "Timer: " + Convert.ToString(panelPGM.PGMTimer) + ", " +
                                                "Serial #: " + panelPGM.PGMSerialNo + ", " +
                                                "Input #: " + Convert.ToString(panelPGM.PGMInputNo) + ", " +
                                                "Activation Event: " + panelPGM.PGMActivationEvent + ", " +
                                                "Deactivation Event: " + panelPGM.PGMDeactivationEvent + ", " +
                                                "Pulse Every 30 Secs: " + Convert.ToString(panelPGM.PGMPulseEvery30Secs) + ", " +
                                                "Pulse On Any Alarm: " + Convert.ToString(panelPGM.PGMPulseOnAnyAlarm);
                            }
                        }
                    }
                }
                else if (ItemType == PanelObjectTypes.OT_DOOR)
                {
                    if (ItemNo <= controlPanel.Doors.panelDoors.Count)
                    {
                        treeNode = treeView.Nodes[0];

                        PanelDoor panelDoor = controlPanel.Doors[ItemNo];

                        if (panelDoor != null)
                        {
                            sName = "Door " + Convert.ToString(panelDoor.DoorNo);

                            TreeNode[] tns = treeView.Nodes.Find("treeNode" + "Door" + Convert.ToString(panelDoor.DoorNo), true);

                            if (tns.Count() > 0)
                            {
                                tns[0].Text = sName + ": (" + panelDoor.DoorLabel + ") " + "   [" + panelDoor.Status + " ]   " +
                                                "Enabled: " + Convert.ToString(panelDoor.DoorEnabled) + ", " +
                                                "Serial #: " + Convert.ToString(panelDoor.DoorSerialNo);
                            }
                        }
                    }
                }

                else if (ItemType == PanelObjectTypes.OT_USER)
                {

                }
            }
            finally
            {
                treeView.EndUpdate();
            }                
        }

        private void FillControlPanelStatusView(TreeView treeView, ControlPanelViewModel controlPanel)
        {
            treeView.Nodes.Clear();
            treeView.BeginUpdate();
            try
            {
                if (controlPanel.Areas.panelAreas.Count > 0)
                {
                    TreeNode treeNodeAreas = new TreeNode("Areas");
                    treeView.Nodes.Add(treeNodeAreas);

                    foreach (PanelArea panelArea in controlPanel.Areas.panelAreas)
                    {
                        TreeNode treeNodeArea = new TreeNode("Area " + Convert.ToString(panelArea.AreaNo));
                        treeNodeArea.Name = "treeNode" + "Area" + Convert.ToString(panelArea.AreaNo);
                        treeNodeAreas.Nodes.Add(treeNodeArea);
                    }
                }

                if (controlPanel.Zones.PanelZones.Count > 0)
                {
                    TreeNode treeNodeZones = new TreeNode("Zones");
                    treeView.Nodes.Add(treeNodeZones);

                    foreach (PanelZone panelZone in controlPanel.Zones.PanelZones)
                    {
                        TreeNode treeNodeZone = new TreeNode("Zone " + Convert.ToString(panelZone.ZoneNo));
                        treeNodeZone.Name = "treeNode" + "Zone" + Convert.ToString(panelZone.ZoneNo);
                        treeNodeZones.Nodes.Add(treeNodeZone);
                    }
                }

                if (controlPanel.PGMs.panelPGMs.Count > 0)
                {
                    TreeNode treeNodePGMs = new TreeNode("PGMs");
                    treeView.Nodes.Add(treeNodePGMs);

                    foreach (PanelPGM panelPGM in controlPanel.PGMs.panelPGMs)
                    {
                        TreeNode treeNodePGM = new TreeNode("PGM " + Convert.ToString(panelPGM.PGMNo));
                        treeNodePGM.Name = "treeNode" + "PGM" + Convert.ToString(panelPGM.PGMNo);
                        treeNodePGMs.Nodes.Add(treeNodePGM);
                    }
                }


                if (controlPanel.Doors.panelDoors.Count > 0)
                {
                    TreeNode treeNodeDoors = new TreeNode("Doors");
                    treeView.Nodes.Add(treeNodeDoors);

                    foreach (PanelDoor panelDoor in controlPanel.Doors.panelDoors)
                    {
                        TreeNode treeNodeDoor = new TreeNode("Door " + Convert.ToString(panelDoor.DoorNo));
                        treeNodeDoor.Name = "treeNode" + "Door" + Convert.ToString(panelDoor.DoorNo);
                        treeNodeDoors.Nodes.Add(treeNodeDoor);
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
                treeView.ExpandAll();
                treeView.SelectedNode = treeView.TopNode;
            }            
        }

        private void UpdateControlPanelStatusView(TreeView treeView, ControlPanelViewModel controlPanel)        
        {
            TreeNode treeNode = null;
            String sName = "";

            if (treeView.Nodes.Count == 0)
            {
                FillControlPanelStatusView(treeView, controlPanel);
            }

            if (treeView.Nodes.Count == 0)
                return;

            treeView.BeginUpdate();
            try
            {
                if (controlPanel.Areas.panelAreas.Count > 0)
                {

                    treeNode = treeView.Nodes[0];

                    foreach (PanelArea panelArea in controlPanel.Areas.panelAreas)
                    {
                        sName = "Area " + Convert.ToString(panelArea.AreaNo);

                        TreeNode[] tns = treeView.Nodes.Find("treeNode" + "Area" + Convert.ToString(panelArea.AreaNo), true);

                        if (tns.Count() > 0)
                        {
                            tns[0].Text = sName + ": (" + panelArea.AreaLabel + ") " + "   [" + panelArea.Status + " ]   " +
                                            "Enabled: " + Convert.ToString(panelArea.AreaEnabled) + ", " +
                                            "Exit Delay: " + Convert.ToString(panelArea.AreaExitDelay) + ", " +
                                            "Bell Cut- Off Timer: " + Convert.ToString(panelArea.AreaBellCutOffTimer) + ", " +
                                            "Auto-Arming Time: " + Convert.ToString(panelArea.AreaAutoArmingTime) + ", " +
                                            "No Movement: " + Convert.ToString(panelArea.AreaNoMovementTimer);
                        }
                    }
                }

                if (controlPanel.Zones.PanelZones.Count > 0)
                {
                    treeNode = treeView.Nodes[0];

                    foreach (PanelZone panelZone in controlPanel.Zones.PanelZones)
                    {
                        sName = "Zone " + Convert.ToString(panelZone.ZoneNo);

                        TreeNode[] tns = treeView.Nodes.Find("treeNode" + "Zone" + Convert.ToString(panelZone.ZoneNo), true);

                        if (tns.Count() > 0)
                        {
                            tns[0].Text = sName + ":  (" + panelZone.ZoneLabel + ") " + "   [" + panelZone.Status + " ]   " +
                                            "Enabled: " + Convert.ToString(panelZone.ZoneEnabled) + ", " +
                                            "Serial #: " + panelZone.ZoneSerialNo + ", " +
                                            "Input #: " + Convert.ToString(panelZone.ZoneInputNo) + ", " +
                                            "Area #: " + Convert.ToString(panelZone.ZonePartition) + ", " +
                                            "Definition: " + panelZone.ZoneDefinition + ", " +
                                            "Alarm Type: " + panelZone.ZoneAlarmType + ", " +
                                            "Stay: " + Convert.ToString(panelZone.ZoneStay) + ", " +
                                            "Force: " + Convert.ToString(panelZone.ZoneForce) + ", " +
                                            "Bypass: " + Convert.ToString(panelZone.ZoneBypass) + ", " +
                                            "Auto-Shutdown: " + Convert.ToString(panelZone.ZoneAutoShutdown) + ", " +
                                            "RF-Supervision: " + Convert.ToString(panelZone.ZoneRFSupervision) + ", " +
                                            "Intellizone: " + Convert.ToString(panelZone.ZoneIntellizone) + ", " +
                                            "Delay Before Transmission: " + Convert.ToString(panelZone.ZoneDelayBeforeTransmission);
                        }
                    }
                }

                if (controlPanel.PGMs.panelPGMs.Count > 0)
                {
                    treeNode = treeView.Nodes[0];

                    foreach (PanelPGM panelPGM in controlPanel.PGMs.panelPGMs)
                    {
                        sName = "PGM " + Convert.ToString(panelPGM.PGMNo);

                        TreeNode[] tns = treeView.Nodes.Find("treeNode" + "PGM" + Convert.ToString(panelPGM.PGMNo), true);

                        if (tns.Count() > 0)
                        {
                            tns[0].Text = sName + ": (" + panelPGM.PGMLabel + ") " + "   [" + panelPGM.Status + " ]   " +
                                            "Timer: " + Convert.ToString(panelPGM.PGMTimer) + ", " +
                                            "Serial #: " + panelPGM.PGMSerialNo + ", " +
                                            "Input #: " + Convert.ToString(panelPGM.PGMInputNo) + ", " +
                                            "Activation Event: " + panelPGM.PGMActivationEvent + ", " +
                                            "Deactivation Event: " + panelPGM.PGMDeactivationEvent + ", " +
                                            "Pulse Every 30 Secs: " + Convert.ToString(panelPGM.PGMPulseEvery30Secs) + ", " +
                                            "Pulse On Any Alarm: " + Convert.ToString(panelPGM.PGMPulseOnAnyAlarm);
                        }
                    }
                }

                if (controlPanel.Doors.panelDoors.Count > 0)
                {
                    treeNode = treeView.Nodes[0];

                    foreach (PanelDoor panelDoor in controlPanel.Doors.panelDoors)
                    {
                        sName = "Door " + Convert.ToString(panelDoor.DoorNo);

                        TreeNode[] tns = treeView.Nodes.Find("treeNode" + "Door" + Convert.ToString(panelDoor.DoorNo), true);

                        if (tns.Count() > 0)
                        {
                            tns[0].Text = sName + ": (" + panelDoor.DoorLabel + ") " + "   [" + panelDoor.Status + " ]   " +
                                            "Enabled: " + Convert.ToString(panelDoor.DoorEnabled) + ", " +
                                            "Serial #: " + Convert.ToString(panelDoor.DoorSerialNo);
                        }
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
            }            
        }

        private void UpdatePanel(ControlPanelViewModel controlPanel)
        {            
            if (controlPanel.InfoEx.AreaCount != controlPanel.Areas.panelAreas.Count)
            {
                controlPanel.Areas.Clear();

                for (int i = 1; i <= controlPanel.InfoEx.AreaCount; i++)
                {
                    PanelArea panelArea = new PanelArea();
                    panelArea.AreaNo = (UInt32)i;
                    panelArea.AreaLabel = "Area " + Convert.ToString(panelArea.AreaNo);
                    controlPanel.Areas.panelAreas.Add(panelArea);
                }
            }

            if (controlPanel.InfoEx.ZoneCount != controlPanel.Zones.PanelZones.Count)
            {
                controlPanel.Zones.Clear();

                for (int i = 1; i <= controlPanel.InfoEx.ZoneCount; i++)
                {
                    PanelZone panelZone = new PanelZone();
                    panelZone.ZoneNo = (UInt32)i;
                    panelZone.ZonePartition = 1;
                    panelZone.ZoneLabel = "Zone " + Convert.ToString(panelZone.ZoneNo);
                    controlPanel.Zones.PanelZones.Add(panelZone);
                }
            }

            if (controlPanel.InfoEx.PGMCount != controlPanel.PGMs.panelPGMs.Count)
            {
                controlPanel.PGMs.Clear();

                for (int i = 1; i <= controlPanel.InfoEx.PGMCount; i++)
                {
                    PanelPGM panelPGM = new PanelPGM();
                    panelPGM.PGMNo = (UInt32)i;
                    panelPGM.PGMLabel = "PGM " + Convert.ToString(panelPGM.PGMNo);
                    controlPanel.PGMs.panelPGMs.Add(panelPGM);
                }
            }

            if (controlPanel.InfoEx.DoorCount != controlPanel.Doors.panelDoors.Count)
            {
                controlPanel.Doors.Clear();

                for (int i = 1; i <= controlPanel.InfoEx.DoorCount; i++)
                {
                    PanelDoor panelDoor = new PanelDoor();
                    panelDoor.DoorNo = (UInt32)i;
                    panelDoor.DoorLabel = "Door " + Convert.ToString(panelDoor.DoorNo);
                    controlPanel.Doors.panelDoors.Add(panelDoor);
                }
            }

            if (controlPanel.InfoEx.UserCount != controlPanel.Users.panelUsers.Count)
            {
                controlPanel.Users.Clear();

                for (int i = 1; i <= controlPanel.InfoEx.UserCount; i++)
                {
                    PanelUser panelUser = new PanelUser();
                    panelUser.UserNo = (UInt32)i;
                    panelUser.UserName = "User " + Convert.ToString(panelUser.UserNo);
                    //panelUser.UserCode = Convert.ToString(panelUser.UserNo).PadLeft(4, '0');                
                    controlPanel.Users.panelUsers.Add(panelUser);
                }
            }

            if (controlPanel.InfoEx.AccessLevelCount != controlPanel.PanelAccessLevels.panelAccessLevels.Count)
            {
                controlPanel.PanelAccessLevels.Clear();

                for (int i = 0; i < controlPanel.InfoEx.AccessLevelCount; i++)
                {
                    PanelAccessLevel panelAccessLevel = new PanelAccessLevel();
                    panelAccessLevel.AccessLevelNo = (UInt32)i;

                    if (i == 0)
                    {
                        panelAccessLevel.AccessLevelLabel = "All Doors";
                    }
                    else
                    {
                        panelAccessLevel.AccessLevelLabel = "Access Rigths " + Convert.ToString(panelAccessLevel.AccessLevelNo);
                    }
                    controlPanel.PanelAccessLevels.panelAccessLevels.Add(panelAccessLevel);
                }
            }

            if (controlPanel.InfoEx.ScheduleCount != controlPanel.PanelSchedules.panelSchedules.Count)
            {
                controlPanel.PanelSchedules.Clear();

                for (int i = 1; i <= controlPanel.InfoEx.ScheduleCount; i++)
                {
                    PanelSchedule panelSchedule = new PanelSchedule();
                    panelSchedule.ScheduleNo = (UInt32)i;
                    panelSchedule.ScheduleLabel = "Access Schedule" + Convert.ToString(panelSchedule.ScheduleNo);
                    controlPanel.PanelSchedules.panelSchedules.Add(panelSchedule);
                }
            }
        }
        
        private void UpdateControlPanelStatus(ControlPanelViewModel controlPanel)
        {
            String productDescription = controlPanel.InfoEx.Description;

            labelSMSMessageToSend.Text = "SMS Message to Send: " + controlPanel.smsMessageToSend;

            if ((productDescription == null) && controlPanel.InfoEx.SerialNo == null)
            {
                productDescription = "Panel: ?";
            }
            else if ((productDescription == null) || (productDescription == ""))
            {
                productDescription = "Panel Serial #: " + controlPanel.InfoEx.SerialNo + " - " + String.Format("Version {0:X}.{1:X}.{2:d3}", controlPanel.InfoEx.Version, controlPanel.InfoEx.Revision, controlPanel.InfoEx.Build);
            }
            else
            {
                productDescription = "Panel: " + productDescription + " - Serial #: " + controlPanel.InfoEx.SerialNo + " - " + String.Format("Version {0:X}.{1:X}.{2:d3}", controlPanel.InfoEx.Version, controlPanel.InfoEx.Revision, controlPanel.InfoEx.Build);
            }

            PanelProductLabel.Text = productDescription;

            if (controlPanel.ConnectionStatus == "CONNECTED")
            {
                tsBtnConnect.Text = "Disconnect";
                PanelStatusLabel.ForeColor = System.Drawing.Color.Green;
                PanelStatusLabel.Font = new Font(PanelStatusLabel.Font, FontStyle.Bold);
                if (System.DateTime.FromOADate(0.0) != controlPanel.dateTime)
                {
                    PanelDateTimeLabel.Text = "Panel Time: " + Convert.ToString(controlPanel.dateTime);
                }
                else
                {
                    PanelDateTimeLabel.Text = "Panel Time: ";
                }
            }
            else
            {
                tsBtnConnect.Text = "Connect";
                PanelStatusLabel.ForeColor = SystemColors.ControlText;
                PanelStatusLabel.Font = new Font(PanelStatusLabel.Font, FontStyle.Regular);
                PanelDateTimeLabel.Text = "Panel Time: ";
                controlPanel.dateTime = System.DateTime.FromOADate(0.0);
            }

            PanelStatusLabel.Text = controlPanel.ConnectionStatus;
            PanelStatusProgressBar.Value = controlPanel.ConnectionProgress;
            PanelStatusInfoLabel.Text = "Last Msg: " + controlPanel.ConnectionStatusInfo;


            if ((controlPanel.ConnectionProgress == 0) || (controlPanel.ConnectionProgress == 100))
            {
                PanelStatusProgressBar.Visible = false;
            }
            else
            {
                PanelStatusProgressBar.Visible = true;
            }

            UpdateControlPanelIPReceiver(controlPanel, 1);
            UpdateControlPanelIPReceiver(controlPanel, 2);
                   
        }

        private void UpdateControlPanelIPReceiver(ControlPanelViewModel controlPanel, UInt32 ItemNo)
        {
            PanelIPReporting panelIPReporting = null;

            if (ItemNo == 1)
            {
                panelIPReporting = controlPanel.PanelIPReportingReceiver[ItemNo];

                if (panelIPReporting != null)
                {
                    textBoxIpReceiver1Wan1IpAddress.Text = panelIPReporting.WAN1IPAddress;
                    textBoxIpReceiver1Wan1IpPort.Text = Convert.ToString(panelIPReporting.WAN1IPPort);
                    textBoxIpReceiver1Wan2IpAddress.Text = panelIPReporting.WAN2IPAddress;
                    textBoxIpReceiver1Wan2IpPort.Text = Convert.ToString(panelIPReporting.WAN2IPPort);
                    textBoxIpReceiver1Password.Text = panelIPReporting.ReceiverIPPassword;
                    textBoxIpReceiver1Profile.Text = Convert.ToString(panelIPReporting.ReceiverIPProfile);
                    textBoxRcv1Area1AccountNo.Text = panelIPReporting.Area1AccountNo;
                    textBoxRcv1Area2AccountNo.Text = panelIPReporting.Area2AccountNo;
                    labelIpReceiver1Status.Text = panelIPReporting.Status;
                }
            }
            else if (ItemNo == 2)
            {
                panelIPReporting = controlPanel.PanelIPReportingReceiver[ItemNo];

                if (panelIPReporting != null)
                {
                    textBoxIpReceiver2Wan1IpAddress.Text = panelIPReporting.WAN1IPAddress;
                    textBoxIpReceiver2Wan1IpPort.Text = Convert.ToString(panelIPReporting.WAN1IPPort);
                    textBoxIpReceiver2Wan2IpAddress.Text = panelIPReporting.WAN2IPAddress;
                    textBoxIpReceiver2Wan2IpPort.Text = Convert.ToString(panelIPReporting.WAN2IPPort);
                    textBoxIpReceiver2Password.Text = panelIPReporting.ReceiverIPPassword;
                    textBoxIpReceiver2Profile.Text = Convert.ToString(panelIPReporting.ReceiverIPProfile);
                    textBoxRcv2Area1AccountNo.Text = panelIPReporting.Area1AccountNo;
                    textBoxRcv2Area2AccountNo.Text = panelIPReporting.Area2AccountNo;
                    labelIpReceiver2Status.Text = panelIPReporting.Status;
                }
            }
        }

        private void RefreshControlPanelUI()
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {                
                UpdateControlPanelStatus(controlPanel);
                UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel);
                UpdateControlPanelTrouble(listViewPanelTrouble, controlPanel);
                FillControlPanelUserView(listViewUsers, controlPanel);
            }
        }
        
        private void UpdateUI()
        {
            btnRX.Image = imageListStatus.Images[0];
            btnTX.Image = imageListStatus.Images[0];

            foreach (var controlPanel in controlPanels)
            {
                UpdateUIOnNewPanel(controlPanel);
            }

            if (tvContolPanels.Nodes.Count > 0)
            {
                tvContolPanels.SelectedNode = tvContolPanels.Nodes[0];
            }
        }

        private void LogDelegate(UInt32 panelID, Int32 returnValue, String value)
        {
            textBoxLogs.AppendText("Panel ID: " + Convert.ToString(panelID) + "\u2028");
            textBoxLogs.AppendText("Result: " + PanelResults.GetResultCode((UInt32)returnValue) + "\u2028");
            textBoxLogs.AppendText("\u2028");
            textBoxLogs.AppendText(value + "\u2028");
            textBoxLogs.AppendText("\u2028");
        }

        private void RefreshNotificationDelegate(UInt32 panelID, Int32 returnValue)
        {
            ControlPanelViewModel controlPanel = null;           

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                UpdateControlPanelTrouble(listViewPanelTrouble, controlPanel);

                PanelDateTimeLabel.Text = "Panel Time: " + Convert.ToString(controlPanel.dateTime);                
            }            
        }

        private void ErrorNotificationDelegate(UInt32 panelID, Int32 returnValue, String ErrorMsg)
        {
            String errorString = "Panel ID: " + Convert.ToString(panelID) + " - " + ErrorMsg + " - " + String.Format("0x{0:X8}", (UInt32)returnValue);
                        
            textBoxErrors.AppendText(errorString);                                    
            textBoxErrors.AppendText("\u2028");

            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanel(ref controlPanel))
            {
                PanelStatusInfoLabel.Text = "Last Msg: " + errorString;
                controlPanel.lastErrorMsg = errorString;
            }
        }

        private void NotifyTaskCompletedDelegate(UInt32 panelID, Int32 returnValue, UInt32 ItemNo, String ItemType, String ActionType, Object obj)
        {
            ControlPanelViewModel controlPanel = null;                      

            if (GetSelectedControlPanel(panelID, ref controlPanel))
            {
                if (ActionType == OperationTypes.AT_READ)
                {
                    if (ItemType == PanelObjectTypes.OT_IP_RECEIVER)
                    {
                        PanelIPReporting pnlIPReporting = (PanelIPReporting)obj;

                        PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[ItemNo];

                        panelIPReporting.ReportingIPEnabled = pnlIPReporting.ReportingIPEnabled;
                        panelIPReporting.WAN1IPAddress = pnlIPReporting.WAN1IPAddress;
                        panelIPReporting.WAN1IPPort = pnlIPReporting.WAN1IPPort;
                        panelIPReporting.WAN2IPAddress = pnlIPReporting.WAN2IPAddress;
                        panelIPReporting.WAN2IPPort = pnlIPReporting.WAN2IPPort;
                        panelIPReporting.ReceiverIPPassword = pnlIPReporting.ReceiverIPPassword;
                        panelIPReporting.ReceiverIPProfile = pnlIPReporting.ReceiverIPProfile;
                        panelIPReporting.Area1AccountNo = pnlIPReporting.Area1AccountNo;
                        panelIPReporting.Area2AccountNo = pnlIPReporting.Area2AccountNo;  

                        UpdateControlPanelIPReceiver(controlPanel, ItemNo);
                    }
                    else if (ItemType == PanelObjectTypes.OT_USER)
                    {
                        UpdateControlPanelUserView(listViewUsers, controlPanel, ItemNo, ItemType);
                    }
                    else if (ItemType == PanelObjectTypes.OT_USERS)
                    {
                        foreach (PanelUser panelUser in controlPanel.Users.panelUsers)
                        {
                            UpdateControlPanelUserView(listViewUsers, controlPanel, panelUser.UserNo, PanelObjectTypes.OT_USER);
                        }
                    }
                    else if (ItemType == PanelObjectTypes.OT_ZONES)
                    {
                        foreach (PanelZone panelZone in controlPanel.Zones.PanelZones)
                        {
                            UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel, panelZone.ZoneNo, PanelObjectTypes.OT_ZONE);
                        }
                    }
                    else if (ItemType == PanelObjectTypes.OT_DOORS)
                    {
                        foreach (PanelDoor panelDoor in controlPanel.Doors.panelDoors)
                        {
                            UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel, panelDoor.DoorNo, PanelObjectTypes.OT_DOOR);
                        }

                        FillControlPanelDoorView(listViewDoors, controlPanel);
                    }
                    else if (ItemType == PanelObjectTypes.OT_AREAS)
                    {
                        foreach (PanelArea panelArea in controlPanel.Areas.panelAreas)
                        {
                            UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel, panelArea.AreaNo, PanelObjectTypes.OT_AREA);
                        }
                    }
                    else if (ItemType == PanelObjectTypes.OT_PGMS)
                    {
                        foreach (PanelPGM panelPGM in controlPanel.PGMs.panelPGMs)
                        {
                            UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel, panelPGM.PGMNo, PanelObjectTypes.OT_PGM);
                        }
                    }
                    else if (ItemType == PanelObjectTypes.OT_SCHEDULE)
                    {
                        FillControlPanelScheduleView(listViewSchedules, controlPanel);                                                
                    }
                    else if (ItemType == PanelObjectTypes.OT_SCHEDULES)
                    {
                        FillControlPanelScheduleView(listViewSchedules, controlPanel);
                    }
                    else if (ItemType == PanelObjectTypes.OT_HOLIDAYS)
                    {
                        UpdateDoorHolidays(controlPanel);                                                
                    }
                    else if (ItemType == PanelObjectTypes.OT_ACCESS_LEVEL)
                    {
                        UpdateDoorAccessLevel(controlPanel, ItemNo, ItemType);                                                 
                    }
                    else if (ItemType == PanelObjectTypes.OT_ACCESS_LEVELS)
                    {
                        foreach (PanelAccessLevel panelAccessLevel in controlPanel.PanelAccessLevels.panelAccessLevels)
                        {
                            UpdateDoorAccessLevel(controlPanel, panelAccessLevel.AccessLevelNo, PanelObjectTypes.OT_ACCESS_LEVEL);
                        }
                    }                        
                   else if (ItemType == PanelObjectTypes.OT_PANEL_INFO_EX) 
                    {
                        if (controlPanel.Settings.PanelType == "")
                        {
                            var panelInfoEx = (PanelInfoEx)obj;

                            controlPanel.InfoEx = panelInfoEx.FullCopy();

                            controlPanel.Settings.PanelType = controlPanel.InfoEx.Description;

                            if (GetControlPanel(panelID, ref controlPanel))
                            {
                                UpdateUIOnPanelChanged(controlPanel);

                                controlPanel.SaveToFile(String.Format("Data\\ControlPanelID_{0}.xml", controlPanel.panelID), true);

                                RefreshControlPanelUI();
                            }
                        }            
                    }
                    else
                    {
                        UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel, ItemNo, ItemType);
                    }                                        
                }
                else if (ActionType == OperationTypes.AT_WRITE)
                {
                    
                }

                PanelStatusInfoLabel.Text = "Last Msg: " + ActionType + " " + ItemType + " " + Convert.ToString(ItemNo) + " Result: " + PanelResults.GetResultCode((UInt32)returnValue);
            }
            else if (GetControlPanel(panelID, ref controlPanel))
            {
                StatusLabelPanelInfo.Text = "Last Msg: " + ActionType + " " + ItemType + " " + Convert.ToString(ItemNo) + " Result: " + PanelResults.GetResultCode((UInt32)returnValue);

                if (ItemType == PanelObjectTypes.OT_PANEL_INFO_EX)
                {
                    if (controlPanel.Settings.PanelType == "")
                    {
                        var panelInfoEx = (PanelInfoEx)obj;

                        controlPanel.InfoEx = panelInfoEx.FullCopy();

                        controlPanel.Settings.PanelType = controlPanel.InfoEx.Description;

                        if (GetControlPanel(panelID, ref controlPanel))
                        {
                            UpdateUIOnPanelChanged(controlPanel);

                            controlPanel.SaveToFile(String.Format("Data\\ControlPanelID_{0}.xml", controlPanel.panelID), true);

                            RefreshControlPanelUI();
                        }
                    }
                }
            }
        }

        private void monitoringStatusChangesDelegate(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {
                UpdateControlPanelStatusMonitoring(controlPanel, panelMonitoring);                
            }            
        }

        private void receiveLiveEventDelegate(UInt32 panelID, PanelEvent panelEvent)
        {
            UpdateToListViewPanelEvent(listViewPanelEvent, panelID, panelEvent, false);
        }

        private void receiveBufferedEventDelegate(UInt32 panelID, PanelEvent panelEvent)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {
                controlPanel.BufferedEvents.AddEvent(panelEvent);

                if (GetSelectedControlPanel(panelID, ref controlPanel))
                {
                    UpdateToListViewPanelEvent(listViewPanelBufferedEvent, panelID, panelEvent, true);
                }
            }
        }

        private void FillControlPanelUserView(ListView listView, ControlPanelViewModel controlPanel)
        {
            listView.BeginUpdate();
            try
            {
                if (listView.Columns.Count != 3)
                {
                    listView.View = View.Details;
                    listView.GridLines = true;
                    listView.FullRowSelect = true;
                    listView.Columns.Clear();

                    listView.Columns.Add("#", 50, HorizontalAlignment.Center);
                    listView.Columns.Add("Label", 150, HorizontalAlignment.Center);
                    listView.Columns.Add("Code", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Card", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Remote Serial #", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Assigned Area", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Can Bypass Zone", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Can Stay/Sleep Arm", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Can Force Arm", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Can Arm Only", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Can Activation PGM Only", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Duress Code", 100, HorizontalAlignment.Center);

                    //Evo
                    listView.Columns.Add("User Type", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Access Control Enabled", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Access Level No", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Access Schedule No", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Extended Unlock Time", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Add Schedule Tolerance", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Code Follows Schedule", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Arm with Card", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Card Disarm on Access", 100, HorizontalAlignment.Center); 
                    listView.Columns.Add("Card and PIN to Disarm", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Code Length", 100, HorizontalAlignment.Center); 
                }

                string[] arr = new string[23];
                ListViewItem itm = null;

                listView.Items.Clear();

                foreach (PanelUser panelUser in controlPanel.Users.panelUsers)
                {
                    arr[0] = Convert.ToString(panelUser.UserNo);
                    arr[1] = panelUser.UserName;
                    arr[2] = panelUser.UserCode;
                    arr[3] = panelUser.UserCard;
                    arr[4] = panelUser.UserRemoteSerialNo;
                    arr[5] = panelUser.UserPartitionAccess;
                    arr[6] = Convert.ToString(panelUser.UserCanBypass);
                    arr[7] = Convert.ToString(panelUser.UserCanStaySleepArm);
                    arr[8] = Convert.ToString(panelUser.UserCanForceArm);                    
                    arr[9] = Convert.ToString(panelUser.UserCanArmOnly);
                    arr[10] = Convert.ToString(panelUser.UserCanActivationPGMOnly);
                    arr[11] = Convert.ToString(panelUser.UserCanDuress);
                    //EVO  
                    arr[12] = Convert.ToString(panelUser.UserType);
                    arr[13] = Convert.ToString(panelUser.UserAccessControlEnabled);
                    arr[14] = Convert.ToString(panelUser.UserAccessLevelNo);
                    arr[15] = Convert.ToString(panelUser.UserAccessScheduleNo);
                    arr[16] = Convert.ToString(panelUser.UserExtendedUnlockTime);
                    arr[17] = Convert.ToString(panelUser.UserAddScheduleTolerance);
                    arr[18] = Convert.ToString(panelUser.UserCodeFollowsSchedule);
                    arr[19] = Convert.ToString(panelUser.UserArmWithCard);
                    arr[20] = Convert.ToString(panelUser.UserCardDisarmOnAccess);
                    arr[21] = Convert.ToString(panelUser.UserCardAndPINDisarm);
                    arr[22] = Convert.ToString(panelUser.UserCodeLength);
                                        
                    itm = new ListViewItem(arr);
                    listView.Items.Add(itm);
                }
            }

            finally
            {
                listView.EndUpdate();
            }
        }

        private void UpdateControlPanelUserView(ListView listView, ControlPanelViewModel controlPanel, UInt32 ItemNo, String ItemType)
        {
            if ((ItemNo == 0) || (ItemType != PanelObjectTypes.OT_USER))
                return;

            if (listView.Items.Count != controlPanel.Users.panelUsers.Count)
            {
                FillControlPanelUserView(listView, controlPanel);
            }

            listView.BeginUpdate();
            try
            {                
                ListViewItem itm = null;

                if (listView.Items.Count == controlPanel.Users.panelUsers.Count)
                {
                    PanelUser panelUser = controlPanel.Users[ItemNo];

                    if (panelUser != null)
                    {
                        itm = listView.FindItemWithText(Convert.ToString(panelUser.UserNo));

                        if (itm != null)
                        {
                            itm.SubItems[0].Text = Convert.ToString(panelUser.UserNo);
                            itm.SubItems[1].Text = panelUser.UserName;
                            itm.SubItems[2].Text = panelUser.UserCode;
                            itm.SubItems[3].Text = panelUser.UserCard;
                            itm.SubItems[4].Text = panelUser.UserRemoteSerialNo;
                            itm.SubItems[5].Text = panelUser.UserPartitionAccess;
                            itm.SubItems[6].Text = Convert.ToString(panelUser.UserCanBypass);
                            itm.SubItems[7].Text = Convert.ToString(panelUser.UserCanStaySleepArm);
                            itm.SubItems[8].Text = Convert.ToString(panelUser.UserCanForceArm);
                            itm.SubItems[9].Text = Convert.ToString(panelUser.UserCanArmOnly);
                            itm.SubItems[10].Text = Convert.ToString(panelUser.UserCanActivationPGMOnly);
                            itm.SubItems[11].Text = Convert.ToString(panelUser.UserCanDuress);
                            //EVO
                            itm.SubItems[12].Text = Convert.ToString(panelUser.UserType);
                            itm.SubItems[13].Text = Convert.ToString(panelUser.UserAccessControlEnabled);
                            itm.SubItems[14].Text = Convert.ToString(panelUser.UserAccessLevelNo);
                            itm.SubItems[15].Text = Convert.ToString(panelUser.UserAccessScheduleNo);
                            itm.SubItems[16].Text = Convert.ToString(panelUser.UserExtendedUnlockTime);
                            itm.SubItems[17].Text = Convert.ToString(panelUser.UserAddScheduleTolerance);
                            itm.SubItems[18].Text = Convert.ToString(panelUser.UserCodeFollowsSchedule);
                            itm.SubItems[19].Text = Convert.ToString(panelUser.UserArmWithCard);
                            itm.SubItems[20].Text = Convert.ToString(panelUser.UserCardDisarmOnAccess);
                            itm.SubItems[21].Text = Convert.ToString(panelUser.UserCardAndPINDisarm);
                            itm.SubItems[22].Text = Convert.ToString(panelUser.UserCodeLength);
                        }
                    }
                }
            }

            finally
            {
                listView.EndUpdate();
            }
        } 
        
        private void UpdateControlPanelUserView(ListView listView, ControlPanelViewModel controlPanel)
        {
            if (listView.Items.Count != controlPanel.Users.panelUsers.Count)
            {
                FillControlPanelUserView(listView, controlPanel);
            }

            listView.BeginUpdate();
            try
            {                              
                ListViewItem itm = null;
                                
                foreach (PanelUser panelUser in controlPanel.Users.panelUsers)
                {
                    itm = listView.FindItemWithText(Convert.ToString(panelUser.UserNo));

                    if (itm != null)
                    {
                        itm.SubItems[0].Text = Convert.ToString(panelUser.UserNo);
                        itm.SubItems[1].Text = panelUser.UserName;
                        itm.SubItems[2].Text = panelUser.UserCode;
                        itm.SubItems[3].Text = panelUser.UserCard;
                        itm.SubItems[4].Text = panelUser.UserRemoteSerialNo;
                        itm.SubItems[5].Text = panelUser.UserPartitionAccess;
                        itm.SubItems[6].Text = Convert.ToString(panelUser.UserCanBypass);
                        itm.SubItems[7].Text = Convert.ToString(panelUser.UserCanStaySleepArm);
                        itm.SubItems[8].Text = Convert.ToString(panelUser.UserCanForceArm);
                        itm.SubItems[9].Text = Convert.ToString(panelUser.UserCanArmOnly);
                        itm.SubItems[10].Text = Convert.ToString(panelUser.UserCanActivationPGMOnly);
                        itm.SubItems[11].Text = Convert.ToString(panelUser.UserCanDuress);

                        //EVO
                        itm.SubItems[12].Text = Convert.ToString(panelUser.UserType);
                        itm.SubItems[13].Text = Convert.ToString(panelUser.UserAccessControlEnabled);
                        itm.SubItems[14].Text = Convert.ToString(panelUser.UserAccessLevelNo);
                        itm.SubItems[15].Text = Convert.ToString(panelUser.UserAccessScheduleNo);
                        itm.SubItems[16].Text = Convert.ToString(panelUser.UserExtendedUnlockTime);
                        itm.SubItems[17].Text = Convert.ToString(panelUser.UserAddScheduleTolerance);
                        itm.SubItems[18].Text = Convert.ToString(panelUser.UserCodeFollowsSchedule);
                        itm.SubItems[19].Text = Convert.ToString(panelUser.UserArmWithCard);
                        itm.SubItems[20].Text = Convert.ToString(panelUser.UserCardDisarmOnAccess);
                        itm.SubItems[21].Text = Convert.ToString(panelUser.UserCardAndPINDisarm);
                        itm.SubItems[22].Text = Convert.ToString(panelUser.UserCodeLength);
                    }
                }                
            }

            finally
            {
                listView.EndUpdate();
            }            
        }        

        private void UpdateControlPanelTrouble(ListView listview, ControlPanelViewModel controlPanel)
        {
            listview.Items.Clear();
            listview.BeginUpdate();
            try
            {                                   
                if (listview.Columns.Count < 2)
                {
                    listview.View = View.Details;
                    listview.GridLines = true;
                    listview.FullRowSelect = true;
                    listview.Columns.Clear();
                    //Add column header
                    listview.Columns.Add("ID", 100);                    
                    listview.Columns.Add("Trouble", 250);                                                            
                 }
                                                               
                string[] arr = new string[2];

                foreach(PanelTrouble panelTrouble in controlPanel.Troubles.panelTroubles)
                {
                    arr[0] = Convert.ToString(panelTrouble.ItemNo);                                    
                    arr[1] = panelTrouble.Status;                    

                    ListViewItem item = new ListViewItem(arr);                              
                    listview.Items.Add(item);
                }            
            }
                
            finally
            {
                listview.EndUpdate();
            }
        }        

        private void UpdateListViewIPReportingEvents(ListView listview, PanelReportingEvent panelReportingEvent)
        {
            listview.BeginUpdate();
            try
            {
                if (listview.Columns.Count < 10)
                {
                    listview.View = View.Details;
                    listview.GridLines = true;
                    listview.FullRowSelect = true;
                    listview.Columns.Clear();
                    //Add column header
                    listview.Columns.Add("ID", 50, HorizontalAlignment.Center);
                    listview.Columns.Add("Date + Time", 150, HorizontalAlignment.Center);
                    listview.Columns.Add("Account #", 80, HorizontalAlignment.Center);
                    listview.Columns.Add("Event CID #", 90, HorizontalAlignment.Center);
                    listview.Columns.Add("Description", 200, HorizontalAlignment.Center);
                    listview.Columns.Add("Partition/Door", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Zone/User", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Received from", 120, HorizontalAlignment.Center);
                    listview.Columns.Add("Status", 300, HorizontalAlignment.Center);
                    listview.Columns.Add("Protocol ID", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("VOD IP Port", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("VOD Session Key", 100, HorizontalAlignment.Center); 
                 }
                                                               
                string[] arr = new string[12];

                arr[0] = Convert.ToString(panelReportingEvent.EventID);                                
                arr[1] = Convert.ToString(panelReportingEvent.EventDateTime);
                arr[2] = panelReportingEvent.EventAccountNo;
                arr[3] = panelReportingEvent.EventCode;
                arr[4] = panelReportingEvent.EventDescription;
                arr[5] = panelReportingEvent.EventAreaDoorNo;
                arr[6] = panelReportingEvent.EventZoneUserNo;
                arr[7] = panelReportingEvent.EventMACAddress;
                arr[8] = panelReportingEvent.EventStatus;
                arr[9] = panelReportingEvent.EventProtocolID;
                arr[10] = Convert.ToString(panelReportingEvent.VODIPPort);
                arr[11] = panelReportingEvent.VODSessionKey;

                ListViewItem item = new ListViewItem(arr);                              
                listview.Items.Insert(0, item);
            }
            finally
            {
                listview.EndUpdate();
            }
        }

        private void receiveReportingEventDelegate(PanelReportingEvent panelReportingEvent)
        {
            UpdateListViewIPReportingEvents(listViewIPReportingEvents, panelReportingEvent);
        }

        private void progressChangedDelegate(UInt32 panelID, UInt32 task, String description, UInt32 percent)
        {
            //General control panel status
            
            if ((percent == 0) || (percent == 100))
            {
                StatusLabelPanelInfo.Text = "Last Msg: ";
            }
            else
            {
                StatusLabelPanelInfo.Text = "Last Msg: " + " - Panel ID: " + Convert.ToString(panelID) + " - " + description;
            }

            //Update Control Panel Status and Selected Control Panel 

            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {

                controlPanel.ConnectionProgress = (Int32)percent;

                if ((percent == 0) || (percent == 100))
                {
                    controlPanel.ConnectionStatusInfo = "";
                }
                else
                {
                    controlPanel.ConnectionStatusInfo = description;
                }

                if (GetSelectedControlPanel(panelID, ref controlPanel))
                {

                    if ((percent == 0) || (percent == 100))
                    {
                        PanelStatusProgressBar.Visible = false;
                    }
                    else
                    {
                        PanelStatusProgressBar.Visible = true;
                    }
                    

                    PanelStatusProgressBar.Value = controlPanel.ConnectionProgress;
                    PanelStatusInfoLabel.Text = "Last Msg: " + controlPanel.ConnectionStatusInfo;
                }
            }
        }

        private void progressErrorDelegate(UInt32 panelID, UInt32 task, UInt32 errorCode, String errorMsg)
        {
            //General control panel status            
            StatusLabelPanelInfo.Text = "Last Msg: " + " - Panel ID: " + Convert.ToString(panelID) + " - Error Code: " + Convert.ToString(errorCode) + " - " + errorMsg;

            //Update Control Panel Status and Selected Control Panel 

            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {
                controlPanel.ConnectionStatusInfo = "Error Code: 0x" + errorCode.ToString("x") + " - " + errorMsg; ;
                controlPanel.ConnectionProgress = 0;

                if (GetSelectedControlPanel(panelID, ref controlPanel))
                {
                    PanelStatusProgressBar.Value = controlPanel.ConnectionProgress;
                    PanelStatusInfoLabel.Text = "Last Msg: " + controlPanel.ConnectionStatusInfo;
                }
            }
        }

        private void connectionStatusChangedDelegate(UInt32 panelID, String status)
        {
            //Update Control Panel Status and Selected Control Panel 

            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {
                controlPanel.ConnectionStatus = status;                                       

                UpdateUIOnPanelChanged(controlPanel);

                UpdateUIOnPanelConnected(controlPanel);

                controlPanel.SaveToFile(String.Format("Data\\ControlPanelID_{0}.xml", controlPanel.panelID), true);


                if (GetSelectedControlPanel(panelID, ref controlPanel))
                {
                    PanelStatusLabel.Text = controlPanel.ConnectionStatus;

                    if (controlPanel.ConnectionStatus == "DISCONNECTED")
                    {
                        tsBtnConnect.Text = "Connect";
                        PanelStatusLabel.ForeColor = SystemColors.ControlText;
                        PanelStatusLabel.Font = new Font(PanelStatusLabel.Font, FontStyle.Regular);
                    }
                    else
                    {
                        tsBtnConnect.Text = "Disconnect";
                        PanelStatusLabel.ForeColor = System.Drawing.Color.Green;
                        PanelStatusLabel.Font = new Font(PanelStatusLabel.Font, FontStyle.Bold);
                        PanelStatusInfoLabel.Text = "Last Msg: ";
                        controlPanel.ConnectionProgress = 0;
                        UpdateControlPanelStatus(controlPanel);
                        Int32 returnValue = ParadoxAPI.StartControlPanelMonitoring(controlPanel.panelID);
                    }
                }
            }                 
        }
        
        //Callback call from ThreadPool.QueueUserWorkItem
        private void GetSystemTroublesFromControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            PanelTroubleList panelTroubleList = new PanelTroubleList();

            Int32 returnValue = ParadoxAPI.GetSystemTroubles(controlPanel.panelID, panelTroubleList);

            controlPanel.Troubles = panelTroubleList;
        }
               
        private void heartbeatDelegate(UInt32 panelID)
        {
            //To keep the connection active, every x seconds (normally 10 seconds) the ParadoxAPI send a command to the control panel when the communication is idlle

            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadDateTimeFromControlPanel), controlPanel);
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetSystemTroublesFromControlPanel), controlPanel);
                ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateIPReportingRcvStatusToControlPanel), controlPanel); 
            }
        }

        private void FormParadoxAPI_FormClosing(object sender, FormClosingEventArgs e)
        {
            ParadoxAPI.UnregisterAllCallback();
        }   

        private void timerStatusRx_Tick(object sender, EventArgs e)
        {
            btnRX.Image = imageListStatus.Images[0];
            timerStatusRx.Enabled = false;
        }
        
        private void rxStatusChangedDelegate(UInt32 panelID, Int32 byteCount)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanel(panelID, ref controlPanel))
            {
                btnRX.Image = imageListStatus.Images[3];
                timerStatusRx.Enabled = true;
            }
            else
            {
                btnRX.Image = imageListStatus.Images[0];
                btnTX.Image = imageListStatus.Images[0];
            }
        }

        private void timerStatusTx_Tick(object sender, EventArgs e)
        {
            btnTX.Image = imageListStatus.Images[0];
            timerStatusTx.Enabled = false;
        }

        private void txStatusChangedDelegate(UInt32 panelID, Int32 byteCount)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanel(panelID, ref controlPanel))
            {
                btnTX.Image = imageListStatus.Images[4];
                timerStatusTx.Enabled = true;
            }
            else
            {
                btnRX.Image = imageListStatus.Images[0];
                btnTX.Image = imageListStatus.Images[0];
            }
        }

        private void iPModuleDetectedDelegate(ModuleInfo moduleInfo)
        {            
            UpdateListViewIPDeviceOnNetwork(moduleInfo);            
        }

        private void smsRequestDelegate(UInt32 panelID, String sms)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetControlPanel(panelID, ref controlPanel))
            {
                controlPanel.smsMessageToSend = sms;

                if (GetSelectedControlPanelConnected(ref controlPanel))
                {
                    labelSMSMessageToSend.Text = "SMS Message to Send: " + controlPanel.smsMessageToSend;                    
                }
            }
        }

        private void accountRegistrationDelegate(PanelReportingAccount panelReportingAccount)
        {
            UpdateListViewIPReportingAccounts(listViewIPReportingAccounts, panelReportingAccount);
        }

        private void accountUpdateDelegate(PanelReportingAccount panelReportingAccount)
        {
            UpdateListViewIPReportingAccounts(listViewIPReportingAccounts, panelReportingAccount);
        }

        private void accountLinkDelegate(PanelReportingAccount panelReportingAccount)
        {
            UpdateListViewIPReportingAccounts(listViewIPReportingAccounts, panelReportingAccount);
        }

        private void ipDOXSocketChangedDelegate(UInt32 port, UInt32 status, String description)
        {
            labelIPDOXStatus.Text = "Status: " + "Port: " + Convert.ToString(port) + " - " + description;
        }
        
        private void UpdateListViewIPReportingAccounts(ListView listview, PanelReportingAccount panelReportingAccount)
        {
            if (panelReportingAccount.MACAddress.Trim() == "")
            {
                return;
            }

            listview.BeginUpdate();
            try
            {
                if (listview.Columns.Count < 14)
                {
                    listview.View = View.Details;
                    listview.GridLines = true;
                    listview.FullRowSelect = true;
                    listview.Columns.Clear();
                    //Add column header
                    listview.Columns.Add("MAC Address", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Status", 120, HorizontalAlignment.Center);
                    listview.Columns.Add("Account #", 70, HorizontalAlignment.Center);
                    listview.Columns.Add("Profile ID", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Protocol ID", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Panel Type", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Panel Serial #", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Panel Version", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Module Type", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Module Serial #", 140, HorizontalAlignment.Center);
                    listview.Columns.Add("Module Version", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Registration Date", 150, HorizontalAlignment.Center);
                    listview.Columns.Add("Last IP Address", 100, HorizontalAlignment.Center);
                    listview.Columns.Add("Last Polling Time", 150, HorizontalAlignment.Center);                    
                 }

                
                ListViewItem itm = listview.FindItemWithText(panelReportingAccount.MACAddress);

                if (itm == null)
                {
                    string[] arr = new string[14];

                    arr[0] = panelReportingAccount.MACAddress;
                    arr[1] = panelReportingAccount.AccountStatus;
                    arr[2] = panelReportingAccount.AccountNo;
                    arr[3] = Convert.ToString(panelReportingAccount.ProfileID);
                    arr[4] = panelReportingAccount.ProtocolID;
                    arr[5] = panelReportingAccount.PanelType;
                    arr[6] = panelReportingAccount.PanelSerialNo;
                    arr[7] = panelReportingAccount.PanelVersion;
                    arr[8] = panelReportingAccount.ModuleType;
                    arr[9] = panelReportingAccount.ModuleSerialNo;
                    arr[10] = panelReportingAccount.ModuleVersion;                    
                    arr[11] = Convert.ToString(panelReportingAccount.RegistrationDate);
                    arr[12] = panelReportingAccount.LastIPAddress;                    
                    arr[13] = Convert.ToString(panelReportingAccount.LastPollingTime);

                    ListViewItem item = new ListViewItem(arr);

                    listview.Items.Insert(0, item);
                }               
                else
                {
                    itm.SubItems[0].Text = panelReportingAccount.MACAddress;
                    itm.SubItems[1].Text = panelReportingAccount.AccountStatus;
                    itm.SubItems[2].Text = panelReportingAccount.AccountNo;
                    itm.SubItems[3].Text = Convert.ToString(panelReportingAccount.ProfileID);
                    itm.SubItems[4].Text = panelReportingAccount.ProtocolID;
                    itm.SubItems[5].Text = panelReportingAccount.PanelType;
                    itm.SubItems[6].Text = panelReportingAccount.PanelSerialNo;
                    itm.SubItems[7].Text = panelReportingAccount.PanelVersion;
                    itm.SubItems[8].Text = panelReportingAccount.ModuleType;
                    itm.SubItems[9].Text = panelReportingAccount.ModuleSerialNo;
                    itm.SubItems[10].Text = panelReportingAccount.ModuleVersion;
                    itm.SubItems[11].Text = Convert.ToString(panelReportingAccount.RegistrationDate);
                    itm.SubItems[12].Text = panelReportingAccount.LastIPAddress;
                    itm.SubItems[13].Text = Convert.ToString(panelReportingAccount.LastPollingTime);                                  
                }
            }

            finally
            {
                listview.EndUpdate();
            }
        }

        private void UpdateToListViewPanelEvent(ListView listview, UInt32 panelID, PanelEvent panelEvent, Boolean buffered)
        {
            listview.BeginUpdate();
            try
            {
                //Add items in the listview
                string[] arr = new string[10];
                ListViewItem itm;

                //Add first item
                arr[0] = Convert.ToString(panelID);
                arr[1] = panelEvent.EventDateTime;
                arr[2] = panelEvent.EventType;
                arr[3] = panelEvent.EventLabel;
                arr[4] = panelEvent.EventSerialNo;
                arr[5] = panelEvent.EventDescription;
                arr[6] = panelEvent.EventAdditionalInfo;
                arr[7] = panelEvent.EventUserLabel;
                arr[8] = panelEvent.EventSequenceNo;
                if (buffered)
                {
                    arr[9] = "true";
                }
                else
                {
                    arr[9] = "false";
                }

                itm = new ListViewItem(arr);
                listview.Items.Insert(0, itm);
            }

            finally
            {
                listview.EndUpdate();
                PanelBufferedEventCountLabel.Text = Convert.ToString(listview.Items.Count) + " Events";
            }

        }

        private void FillBufferdEventListView(ListView listview, ControlPanelViewModel controlPanel)
        {
            listview.Items.Clear();

            listview.BeginUpdate();
            try
            {
                foreach (PanelEvent panelEvent in controlPanel.BufferedEvents.panelEvents)
                {
                    //Add items in the listview
                    string[] arr = new string[10];
                    ListViewItem itm;

                    //Add first item
                    arr[0] = Convert.ToString(controlPanel.panelID);
                    arr[1] = panelEvent.EventDateTime;
                    arr[2] = panelEvent.EventType;
                    arr[3] = panelEvent.EventLabel;
                    arr[4] = panelEvent.EventSerialNo;
                    arr[5] = panelEvent.EventDescription;
                    arr[6] = panelEvent.EventAdditionalInfo;
                    arr[7] = panelEvent.EventUserLabel;
                    arr[8] = panelEvent.EventSequenceNo;
                    arr[9] = "true";

                    itm = new ListViewItem(arr);
                    listview.Items.Insert(0, itm);
                }
            }

            finally
            {
                listview.EndUpdate();
                PanelBufferedEventCountLabel.Text = Convert.ToString(listview.Items.Count) + " Events";
            }

        }

        private void ClearListViewIPDeviceOnNetwork()
        {
            listViewIPDeviceOnNetwork.Clear();
            listViewIPDeviceOnNetwork.BeginUpdate();
            try
            {
                listViewIPDeviceOnNetwork.View = View.Details;
                listViewIPDeviceOnNetwork.GridLines = true;
                listViewIPDeviceOnNetwork.FullRowSelect = true;

                listViewIPDeviceOnNetwork.Columns.Clear();
                //Add column header
                ColumnHeader colHeader = listViewIPDeviceOnNetwork.Columns.Add("Please Wait...", 500);

                //Add items in the listview
                string[] arr = new string[1];
                ListViewItem itm;

                //Add first item
                arr[0] = "Broadcasting on port 10000";
                itm = new ListViewItem(arr);
                listViewIPDeviceOnNetwork.Items.Add(itm);

            }
            finally
            {
                listViewIPDeviceOnNetwork.EndUpdate();
            }
        }

        private void UpdateListViewIPDeviceOnNetwork(ModuleInfoList moduleInfoList)
        {
            listViewIPDeviceOnNetwork.Clear();
            listViewIPDeviceOnNetwork.BeginUpdate();
            try
            {
                if (listViewIPDeviceOnNetwork.Columns.Count < 7)
                {
                    listViewIPDeviceOnNetwork.View = View.Details;
                    listViewIPDeviceOnNetwork.GridLines = true;
                    listViewIPDeviceOnNetwork.FullRowSelect = true;
                    listViewIPDeviceOnNetwork.Columns.Clear();
                    //Add column header
                    listViewIPDeviceOnNetwork.Columns.Add("Site Name", 150);
                    listViewIPDeviceOnNetwork.Columns.Add("Device", 70);
                    listViewIPDeviceOnNetwork.Columns.Add("IP Address", 100);
                    listViewIPDeviceOnNetwork.Columns.Add("IP Port", 50);
                    listViewIPDeviceOnNetwork.Columns.Add("MAC Address", 140);
                    listViewIPDeviceOnNetwork.Columns.Add("Serial #", 70);
                    listViewIPDeviceOnNetwork.Columns.Add("DHCP", 50);
                }

                foreach (ModuleInfo moduleInfo in moduleInfoList.moduleInfos)
                {
                    //Add items in the listview
                    string[] arr = new string[7];
                    ListViewItem itm;

                    //Add first item
                    arr[0] = moduleInfo.SiteNameString;
                    arr[1] = moduleInfo.TypeString;
                    arr[2] = moduleInfo.IPString;
                    arr[3] = moduleInfo.IPPortString;
                    arr[4] = moduleInfo.MacString;
                    arr[5] = moduleInfo.SerialNoString;
                    arr[6] = moduleInfo.DHCPString;
                    itm = new ListViewItem(arr);
                    listViewIPDeviceOnNetwork.Items.Add(itm);
                }
            }

            finally
            {
                listViewIPDeviceOnNetwork.EndUpdate();
            }

        }

        private void UpdateListViewIPDeviceOnNetwork(ModuleInfo moduleInfo)
        {
            listViewIPDeviceOnNetwork.BeginUpdate();
            try
            {
                if (listViewIPDeviceOnNetwork.Columns.Count < 7)
                {
                    listViewIPDeviceOnNetwork.View = View.Details;
                    listViewIPDeviceOnNetwork.GridLines = true;
                    listViewIPDeviceOnNetwork.FullRowSelect = true;
                    listViewIPDeviceOnNetwork.Columns.Clear();
                    //Add column header
                    listViewIPDeviceOnNetwork.Columns.Add("Site Name", 150);
                    listViewIPDeviceOnNetwork.Columns.Add("Device", 70);
                    listViewIPDeviceOnNetwork.Columns.Add("IP Address", 100);
                    listViewIPDeviceOnNetwork.Columns.Add("IP Port", 50);
                    listViewIPDeviceOnNetwork.Columns.Add("MAC Address", 140);
                    listViewIPDeviceOnNetwork.Columns.Add("Serial #", 70);
                    listViewIPDeviceOnNetwork.Columns.Add("DHCP", 50);
                }            
                
                //Add items in the listview
                string[] arr = new string[7];
                ListViewItem itm;

                //Add first item
                arr[0] = moduleInfo.SiteNameString;
                arr[1] = moduleInfo.TypeString;
                arr[2] = moduleInfo.IPString;
                arr[3] = moduleInfo.IPPortString;
                arr[4] = moduleInfo.MacString;
                arr[5] = moduleInfo.SerialNoString;
                arr[6] = moduleInfo.DHCPString;
                itm = new ListViewItem(arr);
                listViewIPDeviceOnNetwork.Items.Add(itm);                
            }

            finally
            {
                listViewIPDeviceOnNetwork.EndUpdate();
            }

        }

        private void FillControlPanel(TreeView treeView, ListView listView, ControlPanelViewModel controlPanel)
        {
            FillControlPanelStatusView(treeView, controlPanel);
            FillControlPanelUserView(listView, controlPanel);
            
            if (controlPanel.SupportAccessControl())
            {
                tabPageAccessControl.Visible = true;                
                doorToolStripMenuItem.Visible = true;
            }
            else
            {
                tabPageAccessControl.Visible = false;
                doorToolStripMenuItem.Visible = false;
            }
        }

        private void LoadPanelBufferedEventsButton_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                Int32 returnValue = ParadoxAPI.ReadBufferedEvents(controlPanel.panelID, controlPanel.GetTotalBufferedEvent());
            }
        }
                
        private void retrievePanelInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {

                PanelInfoEx panelInfoEx = new PanelInfoEx();

                Int32 returnValue = ParadoxAPI.RetrievePanelInfo(controlPanel.panelID, panelInfoEx);
            }
        }
               
        private void MenuItemArmArea1_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = AreaActions.C_CONTROL_AREA_ARM;
                panelControl.Items = "1";

                Int32 returnValue = ParadoxAPI.ControlArea(controlPanel.panelID, panelControl);
            }
        }

        private void MenuItemDisarmArea1_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = AreaActions.C_CONTROL_AREA_DISARM;
                panelControl.Items = "1";

                Int32 returnValue = ParadoxAPI.ControlArea(controlPanel.panelID, panelControl);
            }
        }

        private void MenuItemArmArea2_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = AreaActions.C_CONTROL_AREA_ARM;
                panelControl.Items = "2";

                Int32 returnValue = ParadoxAPI.ControlArea(controlPanel.panelID, panelControl);
            }
        }

        private void MenuItemDisarmArea2_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = AreaActions.C_CONTROL_AREA_DISARM;
                panelControl.Items = "2";

                Int32 returnValue = ParadoxAPI.ControlArea(controlPanel.panelID, panelControl);
            }
        }

        private void areaStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelMonitoring panelMonitoring = new PanelMonitoring();

                Int32 returnValue = ParadoxAPI.AreaStatus(controlPanel.panelID, panelMonitoring);
            }
        }

        private void MenuItemBypassZones_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = ZoneActions.C_CONTROL_ZONE_BYPASS;
                panelControl.Items = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32";

                Int32 returnValue = ParadoxAPI.ControlZone(controlPanel.panelID, panelControl);
            }
        }

        private void MenuItemUnbypassZones_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = ZoneActions.C_CONTROL_ZONE_UNBYPASS;
                panelControl.Items = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32";

                Int32 returnValue = ParadoxAPI.ControlZone(controlPanel.panelID, panelControl);
            }
        }

        private void zoneStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelMonitoring panelMonitoring = new PanelMonitoring();

                Int32 returnValue = ParadoxAPI.ZoneStatus(controlPanel.panelID, panelMonitoring);
            }
        }

        private void MenuItemPGMsOn_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

             if (GetSelectedControlPanelConnected(ref controlPanel))
             {
                 PanelControl panelControl = new PanelControl();

                 //To Do initialize values

                 panelControl.Command = PGMActions.C_CONTROL_PGM_ON;
                 panelControl.Items = "1,2";

                 Int32 returnValue = ParadoxAPI.ControlPGM(controlPanel.panelID, panelControl);
             }
        }

        private void MenuItemPGMsOff_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                //To Do initialize values

                panelControl.Command = PGMActions.C_CONTROL_PGM_OFF;
                panelControl.Items = "1,2";

                Int32 returnValue = ParadoxAPI.ControlPGM(controlPanel.panelID, panelControl);
            }
        }

        private void pGMStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelMonitoring panelMonitoring = new PanelMonitoring();

                Int32 returnValue = ParadoxAPI.PGMStatus(controlPanel.panelID, panelMonitoring);
            }
        }

        private void MenuItemUnlockDoors_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = DoorActions.C_CONTROL_DOOR_UNLOCK;
                panelControl.Items = "1,2";

                Int32 returnValue = ParadoxAPI.ControlDoor(controlPanel.panelID, panelControl);
            }
        }

        private void MenuItemLockDoors_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();

                panelControl.Command = DoorActions.C_CONTROL_DOOR_LOCK;
                panelControl.Items = "1,2";

                Int32 returnValue = ParadoxAPI.ControlDoor(controlPanel.panelID, panelControl);
            }
        }

        private void doorStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelMonitoring panelMonitoring = new PanelMonitoring();

                Int32 returnValue = ParadoxAPI.DoorStatus(controlPanel.panelID, panelMonitoring);
            }
        }

        private void readTimeStampToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelTimeStamp panelTimeStamp = new PanelTimeStamp();

                Int32 returnValue = ParadoxAPI.ReadTimeStamp(controlPanel.panelID, 0, panelTimeStamp);
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadDateTimeFromControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            System.DateTime dateTime = new System.DateTime();

            dateTime = System.DateTime.Now;

            Int32 returnValue = ParadoxAPI.ReadDateTime(controlPanel.panelID, dateTime);

            if (PanelResults.Succeeded((UInt32)returnValue))
            {
                controlPanel.dateTime = dateTime;                
            }
        }
        
        private void readDateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                System.DateTime dateTime = new System.DateTime();

                dateTime = System.DateTime.Now;

                Int32 returnValue = ParadoxAPI.ReadDateTime(controlPanel.panelID, dateTime);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    controlPanel.dateTime = dateTime;

                    PanelDateTimeLabel.Text = "Panel Time: " + Convert.ToString(controlPanel.dateTime);
                }
            }
        }
               
        private void writeDateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                System.DateTime dateTime = new System.DateTime();

                dateTime = System.DateTime.Now;

                Int32 returnValue = ParadoxAPI.WriteDateTime(controlPanel.panelID, dateTime);
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllAreasFromControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.ReadAllAreas(controlPanel.panelID, controlPanel.Areas);

            //foreach (PanelArea panelArea in controlPanel.Areas.panelAreas)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadArea(controlPanel.panelID, panelArea.AreaNo, panelArea);
            //}       
        }

        private void readAllAreasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllAreasFromControlPanel), controlPanel);                            
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllZonesFromControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.ReadAllZones(controlPanel.panelID, controlPanel.Zones);

            //foreach (PanelZone panelZone in controlPanel.Zones.panelZones)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadZone(controlPanel.panelID, panelZone.ZoneNo, panelZone);
            //}    
        }

        private void readAllZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllZonesFromControlPanel), controlPanel);            
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllPGMsFromControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.ReadAllPGMs(controlPanel.panelID, controlPanel.PGMs);

            //foreach (PanelPGM panelPGM in controlPanel.PGMs.panelPGMs)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadPGM(controlPanel.panelID, panelPGM.PGMNo, panelPGM);
            //}   
        }

        private void readAllPGMsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllPGMsFromControlPanel), controlPanel);                  
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllDoorsFromControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.ReadAllDoors(controlPanel.panelID, controlPanel.Doors);

            //foreach (PanelDoor panelDoor in controlPanel.Doors.panelDoors)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadDoor(controlPanel.panelID, panelDoor.DoorNo, panelDoor);
            //} 
        }

        private void readAllDoorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllDoorsFromControlPanel), controlPanel);                               
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllUsersFromControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            ParadoxAPI.ReadAllUsers(controlPanel.panelID, controlPanel.Users);
            
            //foreach (PanelUser panelUser in controlPanel.Users.panelUsers)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadUser(controlPanel.panelID, panelUser.UserNo, panelUser);
            //}   
        }

        private void readAllUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllUsersFromControlPanel), controlPanel);                      
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void WriteMultipleUsersToControlPanel(Object stateInfo)
        {
            ControlPanelViewModel controlPanel = (ControlPanelViewModel)stateInfo;

            PanelUserList tmpPanelUsers = new PanelUserList();

            //Write User Code for user 2 to 9 
            foreach (PanelUser panelUser in controlPanel.Users.panelUsers)
            {
                if ((panelUser.UserNo != 0) && (panelUser.UserNo != 1))
                {
                    panelUser.UserCode = Convert.ToString(panelUser.UserNo) + Convert.ToString(panelUser.UserNo) + Convert.ToString(panelUser.UserNo) + Convert.ToString(panelUser.UserNo);
                }

                PanelUser tmpPanelUser = new PanelUser();                                
                tmpPanelUser = panelUser;
                tmpPanelUsers.panelUsers.Add(tmpPanelUser);
                tmpPanelUser.Name = "User" + Convert.ToString(panelUser.UserNo);        

                if (tmpPanelUser.UserNo == 9)
                {
                    break;
                }
            }

            ParadoxAPI.WriteMultipleUsers(controlPanel.panelID, tmpPanelUsers);            
        }

        private void menuItemWriteMutipleUsers_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(WriteMultipleUsersToControlPanel), controlPanel);
            }
        }

        private void MenuItemReadUser5_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelUser panelUser = new PanelUser();
                               
                Int32 returnValue = ParadoxAPI.ReadUser(controlPanel.panelID, 5, panelUser);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    PanelUser returnPanelUser = controlPanel.Users[5];
                    returnPanelUser.UserNo = panelUser.UserNo;
                    returnPanelUser.UserName = panelUser.UserName;
                    returnPanelUser.UserCode = panelUser.UserCode;
                    returnPanelUser.UserPartitionAccess = panelUser.UserPartitionAccess;
                    returnPanelUser.UserCanBypass = panelUser.UserCanBypass;
                    returnPanelUser.UserRemoteSerialNo = panelUser.UserRemoteSerialNo;
                }
            }
        }

        private void MenuItemWriteUser5_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelUser panelUser = controlPanel.Users[5];

                panelUser.Name = "User5";
                panelUser.UserNo = 5;
                panelUser.UserName = "Test User 5";
                panelUser.UserCode = "555555";
                panelUser.UserPartitionAccess = "1,2";
                panelUser.UserCanBypass = true;
                panelUser.UserRemoteSerialNo = "123456";

                Int32 returnValue = ParadoxAPI.WriteUser(controlPanel.panelID, 5, panelUser);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                }
            }
        }
        
        private void readMonitoringStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelMonitoring panelMonitoring = new PanelMonitoring();

                Int32 returnValue = ParadoxAPI.ReadMonitoringStatus(controlPanel.panelID, panelMonitoring);
            }
        }

        private void getSystemTroublesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelTroubleList panelTroubleList = new PanelTroubleList();

                Int32 returnValue = ParadoxAPI.GetSystemTroubles(controlPanel.panelID, panelTroubleList);

                controlPanel.Troubles = panelTroubleList;

                UpdateControlPanelTrouble(listViewPanelTrouble, controlPanel);
            }
        }               

        private void writeIPReportingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelIPReporting panelIPReporting = new PanelIPReporting();

                //To Do initialize values

                Int32 returnValue = ParadoxAPI.WriteIPReporting(controlPanel.panelID, 1, panelIPReporting);
            }
        }

        private void readIPReportingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelIPReporting panelIPReporting = new PanelIPReporting();

                Int32 returnValue = ParadoxAPI.ReadIPReporting(controlPanel.panelID, 1, panelIPReporting);
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void DiscoverModulesOnLanNetwork(Object stateInfo)
        {
            ModuleInfoList moduleInfoList = (ModuleInfoList)stateInfo;

            Int32 returnValue = ParadoxAPI.DiscoverModules(moduleInfoList);
        }

        private void tsBtnDiscoverIPModuleOnLAN_Click(object sender, EventArgs e)
        {
            ClearListViewIPDeviceOnNetwork();

            ModuleInfoList moduleInfoList = new ModuleInfoList();

            ThreadPool.QueueUserWorkItem(new WaitCallback(DiscoverModulesOnLanNetwork), moduleInfoList);             
        }
        
        private void tsBtnAddControlPanel_Click_1(object sender, EventArgs e)
        {
            var controlPanel = new ControlPanelViewModel();
            var formPanelSettings = new FormPanelSettings(controlPanel.Settings);

            if (formPanelSettings.ShowDialog(this) == DialogResult.OK)
            {
                UInt32 no = 1;

                ControlPanelViewModel ctrlPanel = null;
                TreeNode treeNode = null;

                for (int i = 0; i < (tvContolPanels as TreeView).GetNodeCount(false); i++)                  
                {
                    treeNode = (tvContolPanels as TreeView).Nodes[i];

                    ctrlPanel = controlPanels.ElementAt(treeNode.Index);                                       

                    if (ctrlPanel.panelID != no)
                    {
                        controlPanel.panelID = no;
                        break;
                    }

                    no += 1;

                }

                if (controlPanel.panelID == 0)
                {
                    controlPanel.panelID = (UInt32)controlPanels.Count() + 1;
                }

                controlPanel.Settings = formPanelSettings.mSettings.FullCopy();

                bool inserted = false;

                for (int i = 0; i < controlPanels.Count; i++)
                {
                    ctrlPanel = controlPanels[i];

                    if (ctrlPanel.panelID > controlPanel.panelID)
                    {
                        controlPanels.Insert(i, controlPanel);
                        inserted = true;
                        break;
                    }
                }

                if (! inserted)
                {
                    controlPanels.Add(controlPanel);
                }    

                UpdateUIOnNewPanel(controlPanel);
                
                controlPanel.SaveToFile(String.Format("Data\\ControlPanelID_{0}.xml", controlPanel.panelID), true);
            }
        }

        private void tsBtnModifySettings_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanel(ref controlPanel))
            {
                FormPanelSettings formPanelSettings = new FormPanelSettings(controlPanel.Settings);

                formPanelSettings.mSettings = controlPanel.Settings.FullCopy();

                if (formPanelSettings.ShowDialog(this) == DialogResult.OK)
                {                    
                    controlPanel.Settings = formPanelSettings.mSettings.FullCopy();

                    UpdateUIOnPanelChanged(controlPanel);

                    controlPanel.SaveToFile(String.Format("Data\\ControlPanelID_{0}.xml", controlPanel.panelID), true);
                }
            }
        }

        private void tsBtnDeletePanel_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                MessageBox.Show("Control Panel is Connected!");
            }
            else if (GetSelectedControlPanel(ref controlPanel))
            {
                string message = "Delete Control Panel " + Convert.ToString(controlPanel.panelID) + " ?";
                string caption = "Deleting Control Panel";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // If the no button was pressed ... 
                if (result == DialogResult.Yes)
                {

                    controlPanel.DeleteFile(String.Format("Data\\ControlPanelID_{0}.xml", controlPanel.panelID));
                    DeletePanel(controlPanel);
                }
            }
        }

        private void tsBtnConnect_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DisconnectFromControlPanel), controlPanel);                 
            }
            else if (GetSelectedControlPanel(ref controlPanel))
            {               
                ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectToControlPanel), controlPanel);                
            }
            else
            {
                MessageBox.Show("No Control Panel item selected");
            }
        }
                    
        private void tvContolPanels_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((sender as TreeView).SelectedNode != null)
            {
                TreeNode treeNode = (sender as TreeView).SelectedNode;

                switch (treeNode.Level)
                {
                    case 0:
                        if (treeNode.Index < controlPanels.Count())
                        {
                            var controlPanel = controlPanels.ElementAt(treeNode.Index);

                            FillBufferdEventListView(listViewPanelBufferedEvent, controlPanel);
                            FillControlPanel(tvControlPanelStatus, listViewUsers, controlPanel);
                            UpdateControlPanelStatus(controlPanel);
                            UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel);
                            UpdateControlPanelTrouble(listViewPanelTrouble, controlPanel);
                            FillControlPanelDoorView(listViewDoors, controlPanel);
                            FillControlPanelScheduleView(listViewSchedules, controlPanel);
                            UpdateDoorHolidays(controlPanel);
                            UpdateDoorAccessLevels(controlPanel);
                             
                            if (controlPanel.SupportAccessControl())
                            {
                                tabPageAccessControl.Visible = true;
                                doorToolStripMenuItem.Visible = true;
                            }
                            else
                            {
                                tabPageAccessControl.Visible = false;
                                doorToolStripMenuItem.Visible = false;
                            }
                        }
                        break;
                    case 1:
                        TreeNode treeNodeParent = treeNode.Parent;

                        if (treeNodeParent.Index < controlPanels.Count())
                        {
                            var controlPanel = controlPanels.ElementAt(treeNodeParent.Index);

                            FillBufferdEventListView(listViewPanelBufferedEvent, controlPanel);
                            FillControlPanel(tvControlPanelStatus, listViewUsers, controlPanel);
                            UpdateControlPanelStatus(controlPanel);
                            UpdateControlPanelStatusView(tvControlPanelStatus, controlPanel);
                            UpdateControlPanelTrouble(listViewPanelTrouble, controlPanel);
                            FillControlPanelDoorView(listViewDoors, controlPanel);
                            FillControlPanelScheduleView(listViewSchedules, controlPanel);
                            UpdateDoorHolidays(controlPanel);
                            UpdateDoorAccessLevels(controlPanel);
                            if (controlPanel.SupportAccessControl())
                            {
                                tabPageAccessControl.Visible = true;
                                doorToolStripMenuItem.Visible = true;
                            }
                            else
                            {
                                tabPageAccessControl.Visible = false;
                                doorToolStripMenuItem.Visible = false;
                            }
                        }
                        break;
                }
            }           
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            textBoxLogs.Clear();
        }
                         
        private void UpdateIPDOXSettingsUI()
        {
            textBoxIPDOXWanAddress.Text = ipDOXSettings.WANAddress;
            textBoxIPDOXWanPort.Text = Convert.ToString(ipDOXSettings.WANPort);
            textBoxIPDOXIPPassword.Text = ipDOXSettings.IPPassword;
        }

        private void btnStartIPDOX_Click(object sender, EventArgs e)
        {            
            ipDOXSettings.WANAddress = textBoxIPDOXWanAddress.Text;
            ipDOXSettings.WANPort = Convert.ToInt32(textBoxIPDOXWanPort.Text);
            ipDOXSettings.IPPassword = textBoxIPDOXIPPassword.Text;
            ipDOXSettings.WANEnabled = true;

            SaveIPDOXSettingsToFile("Data\\IPDOXSettings.xml", true);

            Int32 returnValue = ParadoxAPI.StartIPDOX(ipDOXSettings);
        }
                
        private void btnrefreshUI_Click(object sender, EventArgs e)
        {
            RefreshControlPanelUI();
        }

        private void toolStripButtonClearErrors_Click(object sender, EventArgs e)
        {
            textBoxErrors.Clear();
        }

        private void ConnectToAllControlPanels()
        {
            foreach (var controlPanel in controlPanels)
            {
                if (controlPanel.ConnectionStatus == "DISCONNECTED")
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectToControlPanel), controlPanel); 
                }
            }
        }

        private void DisconnectFromAllControlPanels()
        {
            foreach (var controlPanel in controlPanels)
            {
                if (controlPanel.ConnectionStatus != "DISCONNECTED")
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DisconnectFromControlPanel), controlPanel);
                }
            }
        }

        private void menuConnectToAllControlPanels_Click(object sender, EventArgs e)
        {
            ConnectToAllControlPanels();
        }

        private void menuDisconnectFromAllControlPanels_Click(object sender, EventArgs e)
        {
            DisconnectFromAllControlPanels();
        }

        private void btnStopIPDOX_Click(object sender, EventArgs e)
        {            
            if (PanelResults.Succeeded((UInt32)ParadoxAPI.StopIPDOX()))
            {
                labelIPDOXStatus.Text = "Status:";
            }
        }

        private void btnDeleteSelectedIPDOXAccount_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewIPReportingAccounts.SelectedItems)
            {
                var macAddress = listViewItem.SubItems[0].Text;

                if (PanelResults.Succeeded((UInt32)ParadoxAPI.DeleteIPDOXAccount(macAddress)))
                {
                    listViewItem.Remove();
                }
            }
        }

        private void btnGetLocalIPAddress_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface network in networkInterfaces)
            {
                // Read the IP configuration for each network
                IPInterfaceProperties properties = network.GetIPProperties();

                // Each network interface may have multiple IP addresses
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    // We're only interested in IPv4 addresses for now
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    // Ignore loopback addresses (e.g., 127.0.0.1)
                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    textBoxIPDOXWanAddress.Text = address.Address.ToString();                    
                }
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadIPReportingProgrammingRcv1ToControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            PanelIPReporting pnlIPReporting = new PanelIPReporting();

            if (PanelResults.Succeeded((UInt32)ParadoxAPI.ReadIPReporting(controlPanel.panelID, 1, pnlIPReporting)))
            {
                //PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[1];

                //panelIPReporting.ReportingIPEnabled = pnlIPReporting.ReportingIPEnabled;
                //panelIPReporting.WAN1IPAddress = pnlIPReporting.WAN1IPAddress;
                //panelIPReporting.WAN1IPPort = pnlIPReporting.WAN1IPPort;
                //panelIPReporting.WAN2IPAddress = pnlIPReporting.WAN2IPAddress;
                //panelIPReporting.WAN2IPPort = pnlIPReporting.WAN2IPPort;
                //panelIPReporting.ReceiverIPPassword = pnlIPReporting.ReceiverIPPassword;
                //panelIPReporting.ReceiverIPProfile = pnlIPReporting.ReceiverIPProfile;
                //panelIPReporting.Area1AccountNo = pnlIPReporting.Area1AccountNo;
                //panelIPReporting.Area2AccountNo = pnlIPReporting.Area2AccountNo;                                   
            }
        }

        private void buttonReadReceiverProg1_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadIPReportingProgrammingRcv1ToControlPanel), controlPanel);                
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadIPReportingProgrammingRcv2ToControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            PanelIPReporting pnlIPReporting = new PanelIPReporting();

            if (PanelResults.Succeeded((UInt32)ParadoxAPI.ReadIPReporting(controlPanel.panelID, 2, pnlIPReporting)))
            {
                //PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[2];

                //panelIPReporting.ReportingIPEnabled = pnlIPReporting.ReportingIPEnabled;
                //panelIPReporting.WAN1IPAddress = pnlIPReporting.WAN1IPAddress;
                //panelIPReporting.WAN1IPPort = pnlIPReporting.WAN1IPPort;
                //panelIPReporting.WAN2IPAddress = pnlIPReporting.WAN2IPAddress;
                //panelIPReporting.WAN2IPPort = pnlIPReporting.WAN2IPPort;
                //panelIPReporting.ReceiverIPPassword = pnlIPReporting.ReceiverIPPassword;
                //panelIPReporting.ReceiverIPProfile = pnlIPReporting.ReceiverIPProfile;
                //panelIPReporting.Area1AccountNo = pnlIPReporting.Area1AccountNo;
                //panelIPReporting.Area2AccountNo = pnlIPReporting.Area2AccountNo;    
            }
        }   

        private void buttonReadReceiverProg2_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadIPReportingProgrammingRcv2ToControlPanel), controlPanel);                
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void UpdateIPReportingRcvStatusToControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            PanelIPReportingStatusList panelIPReportingStatusList = new PanelIPReportingStatusList();
            
            if (PanelResults.Succeeded((UInt32)ParadoxAPI.IPReportingStatus(controlPanel.panelID, panelIPReportingStatusList)))
            {               
                controlPanel.PanelIPReportingReceiver[1].Status = panelIPReportingStatusList[0].RegistrationStatus + " " + panelIPReportingStatusList[0].RegistrationError;                      
                controlPanel.PanelIPReportingReceiver[2].Status = panelIPReportingStatusList[1].RegistrationStatus + " " + panelIPReportingStatusList[1].RegistrationError;                
            }            
        } 

        private void buttonUpdateReceiverStatus_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateIPReportingRcvStatusToControlPanel), controlPanel);                                                                
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void WriteIPReportingProgrammingRcv1ToControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[1]; 

            if (panelIPReporting != null)
            {
                Int32 returnValue = ParadoxAPI.WriteIPReporting(controlPanel.panelID, 1, panelIPReporting);            
            }            
        }

        private void buttonWriteReceiverProg1_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[1];

                if (panelIPReporting != null)
                {
                    panelIPReporting.ReceiverNo = 1;
                    panelIPReporting.ReportingIPEnabled = true;
                    panelIPReporting.WAN1IPAddress = textBoxIpReceiver1Wan1IpAddress.Text;
                    panelIPReporting.WAN1IPPort = Convert.ToInt32(textBoxIpReceiver1Wan1IpPort.Text);
                    panelIPReporting.WAN2IPAddress = textBoxIpReceiver1Wan2IpAddress.Text;
                    panelIPReporting.WAN2IPPort = Convert.ToInt32(textBoxIpReceiver1Wan2IpPort.Text);
                    panelIPReporting.ReceiverIPPassword = textBoxIpReceiver1Password.Text;
                    panelIPReporting.ReceiverIPProfile = Convert.ToInt32(textBoxIpReceiver1Profile.Text);
                    panelIPReporting.Area1AccountNo = textBoxRcv1Area1AccountNo.Text;
                    panelIPReporting.Area2AccountNo = textBoxRcv1Area2AccountNo.Text;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(WriteIPReportingProgrammingRcv1ToControlPanel), controlPanel); 
                }               
            }
        }

        //Callback call from ThreadPool.QueueUserWorkItem
        private void WriteIPReportingProgrammingRcv2ToControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[2];

            if (panelIPReporting != null)
            {
                Int32 returnValue = ParadoxAPI.WriteIPReporting(controlPanel.panelID, 2, panelIPReporting);
            }
        }     

        private void buttonWriteReceiverProg2_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelIPReporting panelIPReporting = controlPanel.PanelIPReportingReceiver[2];

                if (panelIPReporting != null)
                {
                    panelIPReporting.ReceiverNo = 2;
                    panelIPReporting.ReportingIPEnabled = true;
                    panelIPReporting.WAN1IPAddress = textBoxIpReceiver2Wan1IpAddress.Text;
                    panelIPReporting.WAN1IPPort = Convert.ToInt32(textBoxIpReceiver2Wan1IpPort.Text);
                    panelIPReporting.WAN2IPAddress = textBoxIpReceiver2Wan2IpAddress.Text;
                    panelIPReporting.WAN2IPPort = Convert.ToInt32(textBoxIpReceiver2Wan2IpPort.Text);
                    panelIPReporting.ReceiverIPPassword = textBoxIpReceiver2Password.Text;
                    panelIPReporting.ReceiverIPProfile = Convert.ToInt32(textBoxIpReceiver2Profile.Text);
                    panelIPReporting.Area1AccountNo = textBoxRcv2Area1AccountNo.Text;
                    panelIPReporting.Area2AccountNo = textBoxRcv2Area2AccountNo.Text;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(WriteIPReportingProgrammingRcv2ToControlPanel), controlPanel); 
                }                
            }
        }

        private void buttonRegisterReceiverProg1_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();
                panelControl.Command = "Register";
                panelControl.Items = "1";
                
                if (PanelResults.Succeeded((UInt32)ParadoxAPI.RegisterPanel(controlPanel.panelID, panelControl)))
                {
                }
            }
        }

        private void buttonRegisterReceiverProg2_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelControl panelControl = new PanelControl();
                panelControl.Command = "Register";
                panelControl.Items = "2";
                
                if (PanelResults.Succeeded((UInt32)ParadoxAPI.RegisterPanel(controlPanel.panelID, panelControl)))
                {
                }
            }
        }

        private void textBoxAccountNo_TextChanged(object sender, EventArgs e)
        {
            if (sender == textBoxRcv1Area1AccountNo)
            {
                textBoxRcv2Area1AccountNo.Text = textBoxRcv1Area1AccountNo.Text;
            }
            else if (sender == textBoxRcv2Area1AccountNo)
            {
                textBoxRcv1Area1AccountNo.Text = textBoxRcv2Area1AccountNo.Text;
            }
            else if (sender == textBoxRcv1Area2AccountNo)
            {
                textBoxRcv2Area2AccountNo.Text = textBoxRcv1Area2AccountNo.Text;
            }
            else if (sender == textBoxRcv2Area2AccountNo)
            {
                textBoxRcv1Area2AccountNo.Text = textBoxRcv2Area2AccountNo.Text;
            }            
        }

        private void readAllDoorsMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllDoorsFromControlPanel), controlPanel);
            }
        }

        private void readDoor1MenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelDoor panelDoor = new PanelDoor();

                Int32 returnValue = ParadoxAPI.ReadDoor(controlPanel.panelID, 1, panelDoor);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    PanelDoor returnPanelDoor = controlPanel.Doors[1];
                    returnPanelDoor.DoorLabel =                                  panelDoor.DoorLabel;                        
                    returnPanelDoor.DoorEnabled =                                panelDoor.DoorEnabled;
                    returnPanelDoor.DoorSerialNo =                               panelDoor.DoorSerialNo;
                    returnPanelDoor.DoorAreaAssignment =                         panelDoor.DoorAreaAssignment;
                    returnPanelDoor.DoorAccessEnabled =                          panelDoor.DoorAccessEnabled;
                    returnPanelDoor.DoorAccessOption =                           panelDoor.DoorAccessOption;
                    returnPanelDoor.DoorAccessCodeOnKeypad =                     panelDoor.DoorAccessCodeOnKeypad;
                    returnPanelDoor.DoorAccessCardAndCode =                      panelDoor.DoorAccessCardAndCode;
                    returnPanelDoor.DoorAccessArmRestricted =                    panelDoor.DoorAccessArmRestricted;
                    returnPanelDoor.DoorAccessDisarmRestricted =                 panelDoor.DoorAccessDisarmRestricted;
                    returnPanelDoor.DoorBurglaryAlarmOnForced =                  panelDoor.DoorBurglaryAlarmOnForced;
                    returnPanelDoor.DoorSkipDelayOnArmWithCard =                 panelDoor.DoorSkipDelayOnArmWithCard;
                    returnPanelDoor.DoorBurglaryAlarmOnLeftOpen =                panelDoor.DoorBurglaryAlarmOnLeftOpen;
                    returnPanelDoor.DoorMasterOnlyOnClockLost =                  panelDoor.DoorMasterOnlyOnClockLost;
                    returnPanelDoor.DoorEntryToleranceWindow =                   panelDoor.DoorEntryToleranceWindow;
                    returnPanelDoor.DoorReportOnRequestToExit =                  panelDoor.DoorReportOnRequestToExit;
                    returnPanelDoor.DoorReportOnDoorCommandFromPC =              panelDoor.DoorReportOnDoorCommandFromPC;
                    returnPanelDoor.DoorReportOnUserAccessDenied =               panelDoor.DoorReportOnUserAccessDenied;
                    returnPanelDoor.DoorReportOnUserAccessGranted =              panelDoor.DoorReportOnUserAccessGranted;
                    returnPanelDoor.DoorReportOnLeftOpen =                       panelDoor.DoorReportOnLeftOpen;
                    returnPanelDoor.DoorReportOnFocedOpen =                      panelDoor.DoorReportOnFocedOpen;
                    returnPanelDoor.DoorUnlockScheduleStartTimeA =               panelDoor.DoorUnlockScheduleStartTimeA;
                    returnPanelDoor.DoorUnlockScheduleEndTimeA =                 panelDoor.DoorUnlockScheduleEndTimeA;
                    returnPanelDoor.DoorUnlockScheduleDaysA =                    panelDoor.DoorUnlockScheduleDaysA;
                    returnPanelDoor.DoorUnlockScheduleStartTimeB =               panelDoor.DoorUnlockScheduleStartTimeB;
                    returnPanelDoor.DoorUnlockScheduleEndTimeB =                 panelDoor.DoorUnlockScheduleEndTimeB;
                    returnPanelDoor.DoorUnlockScheduleDaysB =                    panelDoor.DoorUnlockScheduleDaysB;
                    returnPanelDoor.DoorSafeModeEnabled =                        panelDoor.DoorSafeModeEnabled;
                    returnPanelDoor.DoorSafeModeCard1 =                          panelDoor.DoorSafeModeCard1;
                    returnPanelDoor.DoorSafeModeCard2 =                          panelDoor.DoorSafeModeCard2;
                    returnPanelDoor.DoorSafeModeCard3 =                          panelDoor.DoorSafeModeCard3;
                    returnPanelDoor.DoorSafeModeCard4 =                          panelDoor.DoorSafeModeCard4;
                    returnPanelDoor.DoorCardActivatesUnlockedSchedule =          panelDoor.DoorCardActivatesUnlockedSchedule;
                    returnPanelDoor.DoorUnlockDoorOnFireAlarm =                  panelDoor.DoorUnlockDoorOnFireAlarm;
                    returnPanelDoor.DoorUnlockOnRequestForExit =                 panelDoor.DoorUnlockOnRequestForExit;                    
                }
            }
        }

        private void WriteDoor1MenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelDoor panelDoor = controlPanel.Doors[1];

                panelDoor.DoorNo = 1;
                panelDoor.DoorLabel = "My Door 1";
                panelDoor.DoorEnabled = true;
                panelDoor.DoorSerialNo = "12123456";
                panelDoor.DoorAreaAssignment = "1,2,3";
                panelDoor.DoorAccessEnabled = true;
                panelDoor.DoorAccessOption = "Or";
                panelDoor.DoorAccessCodeOnKeypad = true;
                panelDoor.DoorAccessCardAndCode = true;
                panelDoor.DoorAccessArmRestricted = true;
                panelDoor.DoorAccessDisarmRestricted = true;
                panelDoor.DoorBurglaryAlarmOnForced = true;
                panelDoor.DoorSkipDelayOnArmWithCard = true;
                panelDoor.DoorBurglaryAlarmOnLeftOpen = true;
                panelDoor.DoorMasterOnlyOnClockLost = true;
                panelDoor.DoorEntryToleranceWindow = 10;
                panelDoor.DoorReportOnRequestToExit = true;
                panelDoor.DoorReportOnDoorCommandFromPC = true;
                panelDoor.DoorReportOnUserAccessDenied = true;
                panelDoor.DoorReportOnUserAccessGranted = true;
                panelDoor.DoorReportOnLeftOpen = true;
                panelDoor.DoorReportOnFocedOpen = true;

                DateTime dt = new DateTime(2015, 01, 01, 2, 30, 0);

                panelDoor.DoorUnlockScheduleStartTimeA = dt;
                panelDoor.DoorUnlockScheduleEndTimeA = dt.AddHours(5);
                panelDoor.DoorUnlockScheduleDaysA = "Mon, Tue, Wed, Thu, Fri, Hol";
                panelDoor.DoorUnlockScheduleStartTimeB = dt.AddHours(1);
                panelDoor.DoorUnlockScheduleEndTimeB = dt.AddHours(3);
                panelDoor.DoorUnlockScheduleDaysB = "Mon, Tue, Wed, Thu, Fri, Hol";
                panelDoor.DoorSafeModeEnabled = true;
                panelDoor.DoorSafeModeCard1 = "001:12345";
                panelDoor.DoorSafeModeCard2 = "002:12345";
                panelDoor.DoorSafeModeCard3 = "003:12345";
                panelDoor.DoorSafeModeCard4 = "004:12345";
                panelDoor.DoorCardActivatesUnlockedSchedule = true;
                panelDoor.DoorUnlockDoorOnFireAlarm = true;
                panelDoor.DoorUnlockOnRequestForExit = true;

                Int32 returnValue = ParadoxAPI.WriteDoor(controlPanel.panelID, 1, panelDoor);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                }
            }
        }

        private void FillDoorAccessLevels()
        {
            gridAccessLevels.AllowUserToAddRows = false;
            gridAccessLevels.AllowUserToDeleteRows = false;
            gridAccessLevels.AllowUserToOrderColumns = false;
            gridAccessLevels.AllowUserToResizeRows = false;
            gridAccessLevels.ColumnCount = 2;
            gridAccessLevels.Columns[0].Name = "No";
            gridAccessLevels.Columns[1].Name = "Label";

            gridAccessLevels.Columns[0].Width = 30;
            gridAccessLevels.Columns[1].Width = 100;

            DataGridViewCheckBoxColumn chk;
                
            for (int i = 1; i <= 32; i++)
            {
                chk = new DataGridViewCheckBoxColumn();
                gridAccessLevels.Columns.Add(chk);
                chk.HeaderText = Convert.ToString(i);
                chk.Name = "AccLvl" + Convert.ToString(i);
                chk.Width = 30;
                chk.ReadOnly = true;
            }

            for (int i = 0; i < 16; i++)
            {
                if (i == 0)
                {
                    string[] row = new string[] { Convert.ToString(i), "All Doors"};
                    
                    Int32 aRow = gridAccessLevels.Rows.Add(row);
                    
                    DataGridViewRow gridRow = gridAccessLevels.Rows[aRow];
                    
                    DataGridViewCheckBoxCell chkCell;

                    for (int c = 2; c < gridAccessLevels.ColumnCount; c++)
                    {
                        chkCell = (DataGridViewCheckBoxCell)gridRow.Cells[c];
                        chkCell.Value = true;
                    }
                }
                else
                {
                    string[] row = new string[] { Convert.ToString(i), "Access Rights " + Convert.ToString(i) };
                    gridAccessLevels.Rows.Add(row);                    
                }                                
            }                
        }

        private void UpdateDoorAccessLevels(ControlPanelViewModel controlPanel)
        {
            DataGridViewRow row;
            DataGridViewCheckBoxCell chk;

            for (int r = 0; r < gridAccessLevels.RowCount; r++)
            {
                if (r != 0)
                {
                    row = gridAccessLevels.Rows[r];

                    for (int c = 2; c < gridAccessLevels.ColumnCount; c++)
                    {
                        chk = (DataGridViewCheckBoxCell)row.Cells[c];
                        chk.Value = false;
                    }
                }
            }
            
            foreach (PanelAccessLevel panelAccessLevel in controlPanel.PanelAccessLevels.panelAccessLevels)
            {
                row = gridAccessLevels.Rows[(Int32)(panelAccessLevel.AccessLevelNo)];

                foreach (var doorNo in panelAccessLevel.Doors)
                {
                    chk = (DataGridViewCheckBoxCell)row.Cells[doorNo];
                    chk.Value = true;
                }
            }
        }

        private void UpdateDoorAccessLevel(ControlPanelViewModel controlPanel, UInt32 ItemNo, String ItemType)
        {
            if (ItemNo != 0)
            {
                PanelAccessLevel panelAccessLevel = controlPanel.PanelAccessLevels[ItemNo];

                DataGridViewCheckBoxCell chk;
                DataGridViewRow row = gridAccessLevels.Rows[(Int32)(panelAccessLevel.AccessLevelNo)];

                for (int c = 2; c < gridAccessLevels.ColumnCount; c++)
                {
                    chk = (DataGridViewCheckBoxCell)row.Cells[c];
                    chk.Value = false;
                }

                foreach (var doorNo in panelAccessLevel.Doors)
                {
                    chk = (DataGridViewCheckBoxCell)row.Cells[doorNo + 1];
                    chk.Value = true;
                }
            }
        }

        private void FillDoorHolidays()
        {
            gridHolidays.AllowUserToAddRows = false;
            gridHolidays.AllowUserToDeleteRows = false;
            gridHolidays.AllowUserToOrderColumns = false;
            gridHolidays.AllowUserToResizeRows = false;
            gridHolidays.ColumnCount = 1;
            gridHolidays.Columns[0].Name = "Month";

            gridHolidays.Columns[0].Width = 60;
            
            for (int i = 1; i <= 31; i++)
            {
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                gridHolidays.Columns.Add(chk);
                chk.HeaderText = Convert.ToString(i);
                chk.Name = "Hol" + Convert.ToString(i);
                chk.Width = 30;
            }

            string[] row;
            
            row = new string[] {"January"};
            gridHolidays.Rows.Add(row);
            row = new string[] { "February" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "March" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "April" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "May" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "June" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "July" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "August" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "September" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "October" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "November" };
            gridHolidays.Rows.Add(row);
            row = new string[] { "December" };
            gridHolidays.Rows.Add(row);
            
        }

        private void UpdateDoorHolidays(ControlPanelViewModel controlPanel)
        {
            DataGridViewRow row;
            DataGridViewCheckBoxCell chk;

            for (int r = 1; r < gridHolidays.RowCount; r++)
            {
                row = gridHolidays.Rows[r];
                               
                for (int c = 1; c < gridHolidays.ColumnCount; c++)
                {
                    chk = (DataGridViewCheckBoxCell)row.Cells[c];
                    chk.Value = false;
                }
            }            

            foreach (PanelHoliday panelHoliday in controlPanel.Holidays.panelHolidays)
            {
                if ((panelHoliday.Month > 0) && (panelHoliday.Month <= 12) && (panelHoliday.Day > 0) && (panelHoliday.Day <= 31))
                {
                    row = gridHolidays.Rows[(Int32)(panelHoliday.Month)];
                    chk = (DataGridViewCheckBoxCell)row.Cells[(Int32)(panelHoliday.Day)];
                    chk.Value = true;
                }                
            }
        }
       
        private void FillControlPanelDoorView(ListView listView, ControlPanelViewModel controlPanel)
        {            
            listView.BeginUpdate();
            try
            {
                if (listView.Columns.Count != 3)
                {
                    listView.View = View.Details;
                    listView.GridLines = true;
                    listView.FullRowSelect = true;
                    listView.Columns.Clear();

                    listView.Columns.Add("#", 50, HorizontalAlignment.Center);
                    listView.Columns.Add("Label", 150, HorizontalAlignment.Center);
                    listView.Columns.Add("Serial #", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Option", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Code On Keypad", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Card And Code", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Arm Restricted", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Disarm Restricted", 120, HorizontalAlignment.Center);
                                        
                    //general feature
                    listView.Columns.Add("Access Enabled", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Burglary Alarm on Forced", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Skip Exit Delay on Arm With Card", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Burglary Alarm on Left Open", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Only Master have Access on Clock Lost", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Entry Tolerance Window", 100, HorizontalAlignment.Center);
                    
                    listView.Columns.Add("Report on Request To Exit", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Report on Door Command from PC", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Report on User Access Denied", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Report on User Access Granted", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Report on Left Open", 100, HorizontalAlignment.Center);
                    listView.Columns.Add("Report on Foced Open", 100, HorizontalAlignment.Center);
                                        
                    listView.Columns.Add("Door Enabled", 120, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Schedule Start Time A", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Schedule End Time A", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Schedule Days A", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Schedule Start Time B", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Schedule End Time B", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Schedule Days B", 140, HorizontalAlignment.Center);

                    listView.Columns.Add("Safe Mode Enabled", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Safe Mode Card 1", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Safe Mode Card 2", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Safe Mode Card 3", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Safe Mode Card 4", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Card Activates Unlocked Schedule", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock Door on Fire Alarm", 140, HorizontalAlignment.Center);
                    listView.Columns.Add("Unlock on Request for Exit", 140, HorizontalAlignment.Center);

                }
                                                                                                                                        
                string[] arr = new string[35];
                ListViewItem itm = null;

                listView.Items.Clear();

                foreach (PanelDoor panelDoor in controlPanel.Doors.panelDoors)
                {
                    arr[0] = Convert.ToString(panelDoor.DoorNo);
                    arr[1] = panelDoor.DoorLabel;
                    arr[2] = panelDoor.DoorSerialNo;
                    arr[3] = panelDoor.DoorAccessOption;
                    arr[4] = Convert.ToString(panelDoor.DoorAccessCodeOnKeypad);
                    arr[5] = Convert.ToString(panelDoor.DoorAccessCardAndCode);
                    arr[6] = Convert.ToString(panelDoor.DoorAccessArmRestricted);
                    arr[7] = Convert.ToString(panelDoor.DoorAccessDisarmRestricted);
                    arr[8] = Convert.ToString(panelDoor.DoorAccessEnabled);
                    arr[9] = Convert.ToString(panelDoor.DoorBurglaryAlarmOnForced);
                    arr[10] = Convert.ToString(panelDoor.DoorSkipDelayOnArmWithCard);
                    arr[11] = Convert.ToString(panelDoor.DoorBurglaryAlarmOnLeftOpen);
                    arr[12] = Convert.ToString(panelDoor.DoorMasterOnlyOnClockLost);
                    arr[13] = Convert.ToString(panelDoor.DoorEntryToleranceWindow);
                    arr[14] = Convert.ToString(panelDoor.DoorReportOnRequestToExit);
                    arr[15] = Convert.ToString(panelDoor.DoorReportOnDoorCommandFromPC);
                    arr[16] = Convert.ToString(panelDoor.DoorReportOnUserAccessDenied);
                    arr[17] = Convert.ToString(panelDoor.DoorReportOnUserAccessGranted);
                    arr[18] = Convert.ToString(panelDoor.DoorReportOnLeftOpen);
                    arr[19] = Convert.ToString(panelDoor.DoorReportOnFocedOpen);                    
                    arr[20] = Convert.ToString(panelDoor.DoorEnabled);                                       
                    arr[21] = Convert.ToString(panelDoor.DoorUnlockScheduleStartTimeA);
                    arr[22] = Convert.ToString(panelDoor.DoorUnlockScheduleEndTimeA);
                    arr[23] = Convert.ToString(panelDoor.DoorUnlockScheduleDaysA);
                    arr[24] = Convert.ToString(panelDoor.DoorUnlockScheduleStartTimeB);
                    arr[25] = Convert.ToString(panelDoor.DoorUnlockScheduleEndTimeB);
                    arr[26] = Convert.ToString(panelDoor.DoorUnlockScheduleDaysB);
                    arr[27] = Convert.ToString(panelDoor.DoorSafeModeEnabled);
                    arr[28] = Convert.ToString(panelDoor.DoorSafeModeCard1);
                    arr[29] = Convert.ToString(panelDoor.DoorSafeModeCard2);
                    arr[30] = Convert.ToString(panelDoor.DoorSafeModeCard3);
                    arr[31] = Convert.ToString(panelDoor.DoorSafeModeCard4);
                    arr[32] = Convert.ToString(panelDoor.DoorCardActivatesUnlockedSchedule);
                    arr[33] = Convert.ToString(panelDoor.DoorUnlockDoorOnFireAlarm);
                    arr[34] = Convert.ToString(panelDoor.DoorUnlockOnRequestForExit);

                    itm = new ListViewItem(arr);
                    listView.Items.Add(itm);
                }
            }

            finally
            {
                listView.EndUpdate();
            }
        }

        private void FillControlPanelScheduleView(ListView listView, ControlPanelViewModel controlPanel)
        {
            listView.BeginUpdate();
            try
            {
                if (listView.Columns.Count != 3)
                {
                    listView.View = View.Details;
                    listView.GridLines = true;
                    listView.FullRowSelect = true;
                    listView.Columns.Clear();

                    listView.Columns.Add("#", 50, HorizontalAlignment.Center);
                    listView.Columns.Add("Label", 150, HorizontalAlignment.Center);
                    listView.Columns.Add("Backup Schedule #", 150, HorizontalAlignment.Center);
                    listView.Columns.Add("Interval A", 400, HorizontalAlignment.Center);
                    listView.Columns.Add("Interval B", 400, HorizontalAlignment.Center);                    
                }
                                                                                                                                        
                string[] arr = new string[5];
                ListViewItem itm = null;

                listView.Items.Clear();

                foreach (PanelSchedule panelSchedule in controlPanel.PanelSchedules.panelSchedules)
                {
                    arr[0] = Convert.ToString(panelSchedule.ScheduleNo);
                    arr[1] = panelSchedule.ScheduleLabel;
                    arr[2] = Convert.ToString(panelSchedule.ScheduleBackupNo);
                    arr[3] = Convert.ToString(panelSchedule.ScheduleStartTimeIntervalA) + " - " + Convert.ToString(panelSchedule.ScheduleEndTimeIntervalA) + " " + panelSchedule.ScheduleDaysIntervalA;
                    arr[4] = Convert.ToString(panelSchedule.ScheduleStartTimeIntervalB) + " - " + Convert.ToString(panelSchedule.ScheduleEndTimeIntervalB) + " " + panelSchedule.ScheduleDaysIntervalB;
                    

                    itm = new ListViewItem(arr);
                    listView.Items.Add(itm);
                }
            }
            finally
            {
                listView.EndUpdate();
            }
        }
        
        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllAccessLevelsFromControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            Int32 returnValue = ParadoxAPI.ReadAllAccessLevels(controlPanel.panelID, controlPanel.PanelAccessLevels);

            //foreach (PanelAccessLevel panelAccessLevel in controlPanel.PanelAccessLevels.panelAccessLevels)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadAccessLevel(controlPanel.panelID, panelAccessLevel.AccessLevelNo, panelAccessLevel);
            //}   
        }

        private void ReadAllAccessLevelsMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllAccessLevelsFromControlPanel), controlPanel);
            }
        }

        private void readAccessLevel1MenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelAccessLevel panelAccessLevel = new PanelAccessLevel();

                Int32 returnValue = ParadoxAPI.ReadAccessLevel(controlPanel.panelID, 1, panelAccessLevel);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    PanelAccessLevel returnAccessLevel = controlPanel.PanelAccessLevels[1];
                    returnAccessLevel.AccessLevelDoors = panelAccessLevel.AccessLevelDoors;
                }

                returnValue = ParadoxAPI.ReadAccessLevel(controlPanel.panelID, 15, panelAccessLevel);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    PanelAccessLevel returnAccessLevel = controlPanel.PanelAccessLevels[15];
                    returnAccessLevel.AccessLevelDoors = panelAccessLevel.AccessLevelDoors;
                }  
            }
        }

        private void writeAccessLevel1MenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;
            PanelAccessLevel panelAccessLevel;
            Int32 returnValue;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                panelAccessLevel = controlPanel.PanelAccessLevels[1];

                panelAccessLevel.AccessLevelNo = 1;                
                panelAccessLevel.AccessLevelDoors = "1, 3, 5, 7, 8, 9, 10";
                
                returnValue = ParadoxAPI.WriteAccessLevel(controlPanel.panelID, 1, panelAccessLevel);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                }

                panelAccessLevel = controlPanel.PanelAccessLevels[15];

                panelAccessLevel.AccessLevelNo = 15;
                panelAccessLevel.AccessLevelDoors = "11, 12, 14, 17, 18, 19, 32";

                returnValue = ParadoxAPI.WriteAccessLevel(controlPanel.panelID, 15, panelAccessLevel);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                }
            }
        }

        private void readHolidaysMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ParadoxAPI.ReadHolidays(controlPanel.panelID, controlPanel.Holidays);
            }
        }

        private void writeHolidaysMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                //for (int i = 1; i <= 12; i++)
                //{
                //    PanelHoliday panelHoliday = controlPanel.Holidays[(UInt32)i-1];
                //    panelHoliday.Day = 1;
                //    panelHoliday.Month = (UInt32)i;
                //}

                //for (int i = 1; i <= 12; i++)
                //{
                //    PanelHoliday panelHoliday = controlPanel.Holidays[(UInt32)i - 1];
                //    panelHoliday.Day = (UInt32)(1 + i);
                //    panelHoliday.Month = (UInt32)i;
                //}

                controlPanel.Holidays.Clear();
                
                DataGridViewRow row;
                DataGridViewCheckBoxCell chk;
                
                for (int r = 1; r < gridHolidays.RowCount; r++)
                {
                    row = gridHolidays.Rows[r];

                    for (int c = 1; c < gridHolidays.ColumnCount; c++)
                    {
                        chk = (DataGridViewCheckBoxCell)row.Cells[c];
                        
                        if ((Boolean)chk.Value == true)
                        {
                            PanelHoliday panelHoliday = controlPanel.Holidays[(UInt32)r-1];
                            panelHoliday.Day = (UInt32)(c);
                            panelHoliday.Month = (UInt32)(r);
                        }
                    }
                }      
                                
                ParadoxAPI.WriteHolidays(controlPanel.panelID, controlPanel.Holidays);
            }
        }               

        //Callback call from ThreadPool.QueueUserWorkItem
        private void ReadAllSchedulesFromControlPanel(Object stateInfo)
        {
            var controlPanel = (ControlPanelViewModel)stateInfo;

            //foreach (PanelSchedule panelSchedule in controlPanel.PanelSchedules.panelSchedules)
            //{
            //    Int32 returnValue = ParadoxAPI.ReadSchedule(.ReadSchedule(controlPanel.panelID, panelSchedule.ScheduleNo, panelSchedule);
            //}

            Int32 returnValue = ParadoxAPI.ReadAllSchedules(controlPanel.panelID, controlPanel.PanelSchedules);
        }               

        private void readAllSchedulesMenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ReadAllSchedulesFromControlPanel), controlPanel);
            }
        }

        private void readSchedule1MenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelSchedule panelSchedule = new PanelSchedule();

                Int32 returnValue = ParadoxAPI.ReadSchedule(controlPanel.panelID, 1, panelSchedule);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {
                    PanelSchedule returnSchedule = controlPanel.PanelSchedules[1];

                    returnSchedule.ScheduleLabel = panelSchedule.ScheduleLabel;
                    returnSchedule.ScheduleBackupNo = panelSchedule.ScheduleBackupNo;                    
                    returnSchedule.ScheduleStartTimeIntervalA = panelSchedule.ScheduleStartTimeIntervalA;
                    returnSchedule.ScheduleEndTimeIntervalA = panelSchedule.ScheduleEndTimeIntervalA;
                    returnSchedule.ScheduleDaysIntervalA = panelSchedule.ScheduleDaysIntervalA;
                    returnSchedule.ScheduleStartTimeIntervalB = panelSchedule.ScheduleStartTimeIntervalB;
                    returnSchedule.ScheduleEndTimeIntervalB = panelSchedule.ScheduleEndTimeIntervalB;
                    returnSchedule.ScheduleDaysIntervalB = panelSchedule.ScheduleDaysIntervalB;                                       
                }
            }
        }

        private void writeSchedule1MenuItem_Click(object sender, EventArgs e)
        {
            ControlPanelViewModel controlPanel = null;

            if (GetSelectedControlPanelConnected(ref controlPanel))
            {
                PanelSchedule panelSchedule = controlPanel.PanelSchedules[1];

                DateTime dt = new DateTime(2015, 01, 01, 15, 20, 0);

                panelSchedule.ScheduleNo = 1;
                panelSchedule.ScheduleBackupNo = 0;                    
                panelSchedule.ScheduleStartTimeIntervalA = dt;
                panelSchedule.ScheduleEndTimeIntervalA = dt.AddHours(3);
                panelSchedule.ScheduleDaysIntervalA = "Mon, Tue, Wed, Thu, Fri, Hol";
                panelSchedule.ScheduleStartTimeIntervalB = dt.AddHours(1);
                panelSchedule.ScheduleEndTimeIntervalB = dt.AddHours(2);
                panelSchedule.ScheduleDaysIntervalB = "Mon, Tue, Wed, Thu, Fri";              
                               
                Int32 returnValue = ParadoxAPI.WriteSchedule(controlPanel.panelID, 1, panelSchedule);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {

                }

                panelSchedule = controlPanel.PanelSchedules[15];

                dt = new DateTime(2015, 01, 01, 10, 10, 0);

                panelSchedule.ScheduleNo = 15;
                panelSchedule.ScheduleBackupNo = 0;
                panelSchedule.ScheduleStartTimeIntervalA = dt;
                panelSchedule.ScheduleEndTimeIntervalA = dt.AddHours(1);
                panelSchedule.ScheduleDaysIntervalA = "Mon, Tue, Wed, Thu, Fri, Hol";
                panelSchedule.ScheduleStartTimeIntervalB = dt.AddHours(1);
                panelSchedule.ScheduleEndTimeIntervalB = dt.AddHours(2);
                panelSchedule.ScheduleDaysIntervalB = "Mon, Tue, Wed, Thu, Fri";

                returnValue = ParadoxAPI.WriteSchedule(controlPanel.panelID, 15, panelSchedule);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {

                }

                panelSchedule = controlPanel.PanelSchedules[32];

                dt = new DateTime(2015, 01, 01, 18, 20, 0);

                panelSchedule.ScheduleNo = 32;
                panelSchedule.ScheduleBackupNo = 0;
                panelSchedule.ScheduleStartTimeIntervalA = dt;
                panelSchedule.ScheduleEndTimeIntervalA = dt.AddHours(3);
                panelSchedule.ScheduleDaysIntervalA = "Tue, Thu, Fri, Hol";
                panelSchedule.ScheduleStartTimeIntervalB = dt.AddHours(1);
                panelSchedule.ScheduleEndTimeIntervalB = dt.AddHours(1);
                panelSchedule.ScheduleDaysIntervalB = "Mon, Wed, Fri";

                returnValue = ParadoxAPI.WriteSchedule(controlPanel.panelID, 32, panelSchedule);

                if (PanelResults.Succeeded((UInt32)returnValue))
                {

                }
            }
        }

        private void buttonConfigureVideoServer_Click(object sender, EventArgs e)
        {
            VideoSettings videoSettings = new VideoSettings();

            videoSettings.VideoFileDir = textBoxVideoFileDirectory.Text;
            videoSettings.VideoFileLifeTime = Convert.ToUInt32(textBoxVideoLifetime.Text);

            Int32 returnValue = ParadoxAPI.ConfigureVideoServer(videoSettings);

            if (PanelResults.Succeeded((UInt32)returnValue))
            {
                labelConfigureVideoServerResult.Text = "Successfull";
            }
            else 
            {
                labelConfigureVideoServerResult.Text = PanelResults.GetResultCode((UInt32)returnValue) + " - " + String.Format("0x{0:X8}", (UInt32)returnValue);
            }
        }

        private void buttonGetVideoAlarmFiles_Click(object sender, EventArgs e)
        {
            VideoFileList videoFileList = new VideoFileList();

            String accountNo = textBoxAccountNo.Text; 
            UInt32 zoneNo = Convert.ToUInt32(textBoxZoneNo.Text);
            DateTime dateTime = dateTimePickerVideoAlarm.Value;

            Int32 returnValue = ParadoxAPI.GetVideoAlarmFiles(accountNo, zoneNo, dateTime, videoFileList);

            textBoxVideoFiles.Clear();

            if (PanelResults.Succeeded((UInt32)returnValue))
            {
                foreach (VideoFile videoFile in videoFileList.videoFiles)
                {
                    textBoxVideoFiles.AppendText("File ID: " + Convert.ToString(videoFile.FileID) + "\u2028");
                    textBoxVideoFiles.AppendText("File type: " + videoFile.FileType + "\u2028");
                    textBoxVideoFiles.AppendText("File Name: " + videoFile.FileName + "\u2028");
                    textBoxVideoFiles.AppendText("File Path: " + videoFile.FilePath + "\u2028");                    
                }
            }
            else 
            {
                textBoxVideoFiles.AppendText(PanelResults.GetResultCode((UInt32)returnValue) + " - " + String.Format("0x{0:X8}", (UInt32)returnValue));
            }
        }

        private void buttonStartVideoOnDemand_Click(object sender, EventArgs e)
        {
            VideoFileList videoFileList = new VideoFileList();

            String ipAddress = editVODIpAddress.Text; 
            UInt32 ipPort = Convert.ToUInt32(edtVODIpPort.Text);
            String SessionKey = edtSessionKey.Text; 

            Int32 returnValue = ParadoxAPI.StartVideoOnDemand(ipAddress, ipPort, SessionKey, videoFileList);

            textBoxVOD.Clear();

            if (PanelResults.Succeeded((UInt32)returnValue))
            {
                 foreach (VideoFile videoFile in videoFileList.videoFiles)
                {
                    textBoxVOD.AppendText("File ID: " + Convert.ToString(videoFile.FileID) + "\u2028");
                    textBoxVOD.AppendText("File type: " + videoFile.FileType + "\u2028");
                    textBoxVOD.AppendText("File Name: " + videoFile.FileName + "\u2028");
                    textBoxVOD.AppendText("File Path: " + videoFile.FilePath + "\u2028");                                           
                }
            }
            else 
            {
                textBoxVOD.AppendText(PanelResults.GetResultCode((UInt32)returnValue) + " - " + String.Format("0x{0:X8}", (UInt32)returnValue));
            }
        }

        private void buttonStartVideoOnDemandEx_Click(object sender, EventArgs e)
        {
            VideoFileList videoFileList = new VideoFileList();
            VODSettings vodSettings = new VODSettings();
            vodSettings.IPAddress = editVODIpAddressEx.Text;
            vodSettings.IPPort = Convert.ToUInt32(edtVODIpPortEx.Text);
            vodSettings.ServerPassword = edtServerPassword.Text;
            vodSettings.UserName = edtUserName.Text;
            vodSettings.VideoFormat = "360P256K";

            UInt32 panelId = Convert.ToUInt32(edtPanelID.Text);

            Int32 returnValue = ParadoxAPI.StartVideoOnDemandEx(panelId, vodSettings, videoFileList);

            textBoxVOD.Clear();

            if (PanelResults.Succeeded((UInt32)returnValue))
            {
                 foreach (VideoFile videoFile in videoFileList.videoFiles)
                {
                    textBoxVOD.AppendText("File ID: " + Convert.ToString(videoFile.FileID) + "\u2028");
                    textBoxVOD.AppendText("File type: " + videoFile.FileType + "\u2028");
                    textBoxVOD.AppendText("File Name: " + videoFile.FileName + "\u2028");
                    textBoxVOD.AppendText("File Path: " + videoFile.FilePath + "\u2028");

                    PlayVideo(videoFile.FilePath + videoFile.FileName);

                    break;                      
                }
            }
            else 
            {
                textBoxVOD.AppendText(PanelResults.GetResultCode((UInt32)returnValue) + " - " + String.Format("0x{0:X8}", (UInt32)returnValue));
            }
        }

        private void PlayVideo(String file)
        {
            Boolean exists = Directory.Exists("Paradox Video Player");

            if (exists)
            {
                DirectoryInfo directoryInfo = Directory.GetParent("Paradox Video Player");

                if (directoryInfo.Exists)
                {
                    String paradoxVideoDirectory = directoryInfo.FullName + "\\Paradox Video Player";

                    exists = File.Exists(paradoxVideoDirectory + "\\ParadoxVideoPlayer.exe");

                     if (exists)
                     {
                         Process.Start(paradoxVideoDirectory + "\\ParadoxVideoPlayer.exe", "\"" + file + "\"");
                     }
                }
            }
        }
    }
}