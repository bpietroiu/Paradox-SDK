using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ParadoxAPILibrary.External;
using ParadoxAPILibrary.Model;

namespace ParadoxAPILibrary
{
    /// <summary>
    /// Proxy on ParadoxAPIImport
    /// </summary>
    public class ParadoxAPI
    {
        #region Constants

        public static Form formRef = null;

        // Action Type
        public const string AT_READ = "Read";
        public const string AT_WRITE = "Write";

        // Control area
        public const string C_CONTROL_AREA_ARM = "Arm";
        public const string C_CONTROL_AREA_FORCE = "Force";
        public const string C_CONTROL_AREA_STAY = "Stay";
        public const string C_CONTROL_AREA_SLEEP = "Sleep";
        public const string C_CONTROL_AREA_INSTANT = "Instant";
        public const string C_CONTROL_AREA_DISARM = "Disarm";

        // Control Zone
        public const string C_CONTROL_ZONE_BYPASS = "Bypass";
        public const string C_CONTROL_ZONE_UNBYPASS = "Unbypass";

        // Control PGM
        public const string C_CONTROL_PGM_ON = "On";
        public const string C_CONTROL_PGM_OFF = "Off";
        public const string C_CONTROL_PGM_TEST = "Test";

        // Control Door
        public const string C_CONTROL_DOOR_LOCK = "Lock";
        public const string C_CONTROL_DOOR_UNLOCK = "Unlock";

        #endregion

        #region Callbacks

        private static ParadoxAPIImport.ProcConnectionStatusChangedDelegate _procConnectionStatusChangedDelegate;
        private static ParadoxAPIImport.ProcProgressChangedDelegate _procProgressChangedDelegate;
        private static ParadoxAPIImport.ProcProgressErrorDelegate _procProgressErrorDelegate;
        private static ParadoxAPIImport.ProcReceiveReportingEventDelegate _procReceiveReportingEventDelegate;
        private static ParadoxAPIImport.ProcReceiveBufferEventDelegate _procReceiveBufferEventDelegate;
        private static ParadoxAPIImport.ProcReceiveLiveEventDelegate _procReceiveLiveEventDelegate;
        private static ParadoxAPIImport.ProcMonitoringStatusChangedDelegate _procMonitoringStatusChangedDelegate;
        private static ParadoxAPIImport.ProcHeartbeatDelegate _procHeartbeatDelegate;
        private static ParadoxAPIImport.ProcRxStatusChangedDelegate _procRxStatusChangedDelegate;
        private static ParadoxAPIImport.ProcTxStatusChangedDelegate _procTxStatusChangedDelegate;
        private static ParadoxAPIImport.ProcIPModuleDetectedDelegate _procIPModuleDetectedDelegate;
        private static ParadoxAPIImport.ProcSMSRequestDelegate _procSMSRequestDelegate;
        private static ParadoxAPIImport.ProcAccountRegistrationDelegate _procAccountRegistrationDelegate;
        private static ParadoxAPIImport.ProcAccountUpdateDelegate _procAccountUpdateDelegate;
        private static ParadoxAPIImport.ProcAccountLinkDelegate _procAccountLinkDelegate;
        private static ParadoxAPIImport.ProcIPDOXSocketChangedDelegate _procIPDOXSocketChangedDelegate;

        public static void RegisterAllCallback()
        {
            _procConnectionStatusChangedDelegate = new ParadoxAPIImport.ProcConnectionStatusChangedDelegate(ConnectionStatusChangedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterConnectionStatusChangedCallback(_procConnectionStatusChangedDelegate);

            _procProgressChangedDelegate = new ParadoxAPIImport.ProcProgressChangedDelegate(ProgressChangedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterProgressChangedCallback(_procProgressChangedDelegate);

            _procProgressErrorDelegate = new ParadoxAPIImport.ProcProgressErrorDelegate(ProgressErrorCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterProgressErrorCallback(_procProgressErrorDelegate);

            _procMonitoringStatusChangedDelegate = new ParadoxAPIImport.ProcMonitoringStatusChangedDelegate(MonitoringStatusChangedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterMonitoringStatusChangedCallback(_procMonitoringStatusChangedDelegate);

            _procReceiveLiveEventDelegate = new ParadoxAPIImport.ProcReceiveLiveEventDelegate(ReceiveLiveEventCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterReceiveLiveEventCallback(_procReceiveLiveEventDelegate);

            _procReceiveBufferEventDelegate = new ParadoxAPIImport.ProcReceiveBufferEventDelegate(ReceiveBufferEventCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterReceiveBufferEventCallback(_procReceiveBufferEventDelegate);

            _procReceiveReportingEventDelegate = new ParadoxAPIImport.ProcReceiveReportingEventDelegate(ReceiveReportingEventCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterReceiveReportingEventCallback(_procReceiveReportingEventDelegate);

            _procHeartbeatDelegate = new ParadoxAPIImport.ProcHeartbeatDelegate(HeartbeatCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterHeartbeatCallback(_procHeartbeatDelegate);

            _procRxStatusChangedDelegate = new ParadoxAPIImport.ProcRxStatusChangedDelegate(RxStatusChangedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterRxStatusChangedCallback(_procRxStatusChangedDelegate);

            _procTxStatusChangedDelegate = new ParadoxAPIImport.ProcTxStatusChangedDelegate(TxStatusChangedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterTxStatusChangedCallback(_procTxStatusChangedDelegate);

            _procIPModuleDetectedDelegate = new ParadoxAPIImport.ProcIPModuleDetectedDelegate(IPModuleDetectedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterIPModuleDetectedCallback(_procIPModuleDetectedDelegate);

            _procSMSRequestDelegate = new ParadoxAPIImport.ProcSMSRequestDelegate(SMSRequestCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterSMSRequestCallback(_procSMSRequestDelegate);

            _procAccountRegistrationDelegate = new ParadoxAPIImport.ProcAccountRegistrationDelegate(AccountRegistrationCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterAccountRegistrationCallback(_procAccountRegistrationDelegate);

            _procAccountUpdateDelegate = new ParadoxAPIImport.ProcAccountUpdateDelegate(AccountUpdateCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterAccountUpdateCallback(_procAccountUpdateDelegate);

            _procAccountLinkDelegate = new ParadoxAPIImport.ProcAccountLinkDelegate(AccountLinkCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterAccountLinkCallback(_procAccountLinkDelegate);

            _procIPDOXSocketChangedDelegate = new ParadoxAPIImport.ProcIPDOXSocketChangedDelegate(IPDOXSocketChangedCalledFromParadoxAPI);
            ParadoxAPIImport.RegisterIPDOXSocketChangedCallback(_procIPDOXSocketChangedDelegate);
        }

        public static void UnregisterAllCallback()
        {
            ParadoxAPIImport.UnregisterAllCallback();
        }

        #region MonitoringStatusChangesDelegate

        public delegate void MonitoringStatusChangesDelegate(UInt32 panelID, PanelMonitoring panelMonitoring);

        public static MonitoringStatusChangesDelegate MonitoringStatusChanges = null;

        // Callback
        public static void MonitoringStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlStatus)
        {
            if (MonitoringStatusChanges != null)
            {
                using (var panelMonitoring = new PanelMonitoring())
                {
                    panelMonitoring.parseXML(xmlStatus);

                    MonitoringStatusChanges(panelID, panelMonitoring);
                }
            }
        }

        #endregion

        #region ReceiveLiveEventDelegate

        public delegate void ReceiveLiveEventDelegate(UInt32 panelID, PanelEvent panelEvent);

        public static ReceiveLiveEventDelegate receiveLiveEventDelegate = null;

        // Callback
        public static void ReceiveLiveEventCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlEvents)
        {

            if (receiveLiveEventDelegate != null)
            {
                PanelEvent panelEvent = new PanelEvent();
                try
                {
                    panelEvent.parseXML(xmlEvents);

                    receiveLiveEventDelegate(panelID, panelEvent);
                }
                finally
                {
                    panelEvent.Dispose();
                }
            }
        }

        #endregion

        public delegate void ReceiveBufferedEventDelegate(UInt32 panelID, PanelEvent panelEvent);

        public static ReceiveBufferedEventDelegate receiveBufferedEventDelegate = null;

        // Callback
        public static void ReceiveBufferEventCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlEvents)
        {
            if (receiveBufferedEventDelegate != null)
            {
                PanelEvent panelEvent = new PanelEvent();
                try
                {
                    panelEvent.parseXML(xmlEvents);

                    receiveBufferedEventDelegate(panelID, panelEvent);
                }
                finally
                {
                    panelEvent.Dispose();
                }
            }
        }

        public delegate void ReceiveReportingEventDelegate(PanelReportingEvent panelReportingEvent);

        public static ReceiveReportingEventDelegate receiveReportingEventDelegate = null;

        // Callback
        public static void ReceiveReportingEventCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] string xmlEvents)
        {
            if (receiveReportingEventDelegate != null)
            {
                PanelReportingEvent panelReportingEvent = new PanelReportingEvent();
                try
                {
                    Log(0, 0, xmlEvents);

                    if (panelReportingEvent.parseXML(xmlEvents))
                    {
                        receiveReportingEventDelegate(panelReportingEvent);
                    }
                }
                finally
                {
                    panelReportingEvent.Dispose();
                }
            }
        }

        public delegate void ProgressChangedDelegate(UInt32 panelID, UInt32 task, string description, UInt32 percent);

        public static ProgressChangedDelegate progressChangedDelegate = null;

        // Callback
        public static void ProgressChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 task, [MarshalAs(UnmanagedType.BStr)] string description, [MarshalAs(UnmanagedType.U4)] UInt32 percent)
        {
            if (progressChangedDelegate != null)
            {
                progressChangedDelegate(panelID, task, description, percent);
            }
        }

        public delegate void ProgressErrorDelegate(UInt32 panelID, UInt32 task, UInt32 errorCode, string errorMsg);

        public static ProgressErrorDelegate progressErrorDelegate = null;

        // Callback
        public static void ProgressErrorCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 task, [MarshalAs(UnmanagedType.U4)] UInt32 errorCode, [MarshalAs(UnmanagedType.BStr)] string errorMsg)
        {
            if (progressErrorDelegate != null)
            {
                progressErrorDelegate(panelID, task, errorCode, errorMsg);
            }
        }

        public delegate void ConnectionStatusChangedDelegate(UInt32 panelID, string status);

        public static ConnectionStatusChangedDelegate connectionStatusChangedDelegate = null;

        // Callback
        public static void ConnectionStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)]UInt32 ptr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string status)
        {
            if (connectionStatusChangedDelegate != null)
            {
                connectionStatusChangedDelegate(panelID, status);
            }
        }

        public delegate void HeartbeatDelegate(UInt32 panelID);

        public static HeartbeatDelegate heartbeatDelegate = null;

        // Callback
        public static void HeartbeatCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID)
        {
            if (heartbeatDelegate != null)
            {
                heartbeatDelegate(panelID);
            }
        }

        public delegate void RxStatusChangedDelegate(UInt32 panelID, int byteCount);

        public static RxStatusChangedDelegate rxStatusChangedDelegate = null;

        // Callback
        public static void RxStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID, int byteCount)
        {
            if (rxStatusChangedDelegate != null)
            {
                rxStatusChangedDelegate(panelID, byteCount);
            }
        }

        public delegate void TxStatusChangedDelegate(UInt32 panelID, int byteCount);

        public static TxStatusChangedDelegate txStatusChangedDelegate = null;

        // Callback
        public static void TxStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID, int byteCount)
        {
            if (txStatusChangedDelegate != null)
            {
                txStatusChangedDelegate(panelID, byteCount);
            }
        }

        public delegate void IPModuleDetectedDelegate(ModuleInfo moduleInfo);

        public static IPModuleDetectedDelegate iPModuleDetectedDelegate = null;

        // Callback
        public static void IPModuleDetectedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, string xmlModule)
        {
            if (iPModuleDetectedDelegate != null)
            {
                ModuleInfo moduleInfo = new ModuleInfo();

                if (moduleInfo.parseXML(xmlModule))
                {
                    iPModuleDetectedDelegate(moduleInfo);
                }
            }
        }

        public delegate void SMSRequestDelegate(UInt32 panelID, string sms);

        public static SMSRequestDelegate smsRequestDelegate = null;

        // Callback
        public static void SMSRequestCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID, string sms)
        {
            if (smsRequestDelegate != null)
            {
                smsRequestDelegate(panelID, sms);
            }
        }

        public delegate void AccountRegistrationDelegate(PanelReportingAccount panelReportingAccount);

        public static AccountRegistrationDelegate accountRegistrationDelegate = null;

        // Callback
        public static void AccountRegistrationCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, string xmlAccounts)
        {
            if (accountRegistrationDelegate != null)
            {
                PanelReportingAccount panelReportingAccount = new PanelReportingAccount();

                if (panelReportingAccount.parseXML(xmlAccounts))
                {
                    accountRegistrationDelegate(panelReportingAccount);
                }
            }
        }

        public delegate void AccountUpdateDelegate(PanelReportingAccount panelReportingAccount);

        public static AccountUpdateDelegate accountUpdateDelegate = null;

        // Callback
        public static void AccountUpdateCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, string xmlAccounts)
        {
            if (accountUpdateDelegate != null)
            {
                PanelReportingAccount panelReportingAccount = new PanelReportingAccount();

                if (panelReportingAccount.parseXML(xmlAccounts))
                {
                    accountUpdateDelegate(panelReportingAccount);
                }
            }
        }

        public delegate void AccountLinkDelegate(PanelReportingAccount panelReportingAccount);

        public static AccountLinkDelegate accountLinkDelegate = null;

        // Callback
        public static void AccountLinkCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, string xmlAccounts)
        {
            if (accountLinkDelegate != null)
            {
                PanelReportingAccount panelReportingAccount = new PanelReportingAccount();

                if (panelReportingAccount.parseXML(xmlAccounts))
                {
                    accountLinkDelegate(panelReportingAccount);
                }
            }
        }

        public delegate void IPDOXSocketChangedDelegate(UInt32 port, UInt32 status, string description);

        public static IPDOXSocketChangedDelegate ipDOXSocketChangedDelegate = null;

        // Callback
        public static void IPDOXSocketChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 port, UInt32 status, string description)
        {
            if (ipDOXSocketChangedDelegate != null)
            {
                ipDOXSocketChangedDelegate(port, status, description);
            }
        }

        public delegate void LogDelegate(UInt32 panelID, int returnValue, string logs);

        public static LogDelegate logDelegate = null;

        private static void Log(UInt32 panelID, int returnValue, string value)
        {
            if ((formRef != null) && (logDelegate != null))
            {
                formRef.Invoke(logDelegate, new object[] { panelID, returnValue, value });
            }
        }

        public delegate void NotifyTaskCompletedDelegate(UInt32 panelID, int returnValue, UInt32 ItemNo, string ItemType, string ActionType, object obj = null);

        public static NotifyTaskCompletedDelegate notifyTaskCompletedDelegate = null;

        private static void NotifyTaskCompleted(UInt32 panelID, int returnValue, UInt32 ItemNo, string ItemType, string ActionType, object obj = null)
        {
            if ((formRef != null) && (notifyTaskCompletedDelegate != null))
            {
                formRef.Invoke(notifyTaskCompletedDelegate, new object[] { panelID, returnValue, ItemNo, ItemType, ActionType, obj });
            }
        }

        public delegate void RefreshNotificationDelegate(UInt32 panelID, int returnValue);

        public static RefreshNotificationDelegate refreshNotificationDelegate = null;

        private static void RefreshNotification(UInt32 panelID, int returnValue)
        {
            if ((formRef != null) && (refreshNotificationDelegate != null))
            {
                formRef.Invoke(refreshNotificationDelegate, new object[] { panelID, returnValue });
            }
        }

        public delegate void ErrorNotificationDelegate(UInt32 panelID, int returnValue, string ErrorMsg);

        public static ErrorNotificationDelegate errorNotificationDelegate = null;

        private static void errorNotification(UInt32 panelID, int returnValue, string ErrorMsg)
        {
            if ((formRef != null) && (errorNotificationDelegate != null))
            {
                formRef.Invoke(errorNotificationDelegate, new object[] { panelID, returnValue, ErrorMsg });
            }
        }

        #endregion

        #region API Wrapers

        public static int GetAPIVersion(ref string version)
        {
            int returnValue = ParadoxAPIImport.GetDriverVersion(out version);

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "GetDriverVersion: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static int DiscoverModules(ModuleInfoList moduleInfoList)
        {
            if (moduleInfoList != null)
            {
                string xmlInfo = "";

                int returnValue = ParadoxAPIImport.DiscoverModule(out xmlInfo);

                moduleInfoList.parseXML(xmlInfo);

                Log(0, returnValue, xmlInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "DiscoverModules: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ConnectToPanel(UInt32 panelID, PanelSettings panelSettings, UInt32 WaitTimeOut = 30)
        {
            if (panelSettings != null)
            {
                string xmlSettings = "";

                panelSettings.serializeXML(ref xmlSettings);

                int returnValue = ParadoxAPIImport.ConnectPanel(panelID, xmlSettings, WaitTimeOut);

                Log(panelID, returnValue, xmlSettings);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ConnectToPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int DisconnectFromPanel(UInt32 panelID)
        {
            int returnValue = ParadoxAPIImport.DisconnectPanel(panelID);

            Log(panelID, returnValue, "Disconnet");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "DisconnectFromPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static int DetectPanel(UInt32 panelID, PanelSettings panelSettings, PanelInfo panelInfo)
        {
            if ((panelSettings != null) && (panelInfo != null))
            {
                string xmlSettings = "";
                string xmlInfo = "";

                panelSettings.serializeXML(ref xmlSettings);

                int returnValue = ParadoxAPIImport.DetectPanel(xmlSettings, out xmlInfo);

                panelInfo.parseXML(xmlInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "DetectPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int RetrievePanelInfo(UInt32 panelID, PanelInfoEx panelInfoEx)
        {
            if (panelInfoEx != null)
            {
                string xmlInfo = "";

                int returnValue = ParadoxAPIImport.RetrievePanelInfo(panelID, out xmlInfo);

                panelInfoEx.parseXML(xmlInfo);

                Log(panelID, returnValue, xmlInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "RetrievePanelInfo: " + PanelResults.GetResultCode((UInt32)returnValue));
                }
                else
                {
                    NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_PANEL_INFO_EX, AT_READ, panelInfoEx);
                }

                return returnValue;
            }

            return -1;
        }

        public static int RegisterPanel(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlAction = "";

                panelControl.serializeXML(ref xmlAction);

                int returnValue = ParadoxAPIImport.RegisterPanel(panelID, xmlAction);

                Log(panelID, returnValue, xmlAction);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "RegisterPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ControlArea(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlArea = "";

                panelControl.serializeXML(ref xmlArea);

                int returnValue = ParadoxAPIImport.ControlArea(panelID, xmlArea);

                Log(panelID, returnValue, xmlArea);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlArea: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int AreaStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                int returnValue = ParadoxAPIImport.AreaStatus(panelID, out xmlStatus);

                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "AreaStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ControlZone(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlZone = "";

                panelControl.serializeXML(ref xmlZone);

                int returnValue = ParadoxAPIImport.ControlZone(panelID, xmlZone);

                Log(panelID, returnValue, xmlZone);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlZone: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ZoneStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                int returnValue = ParadoxAPIImport.ZoneStatus(panelID, out xmlStatus);

                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ZoneStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ControlPGM(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlPGM = "";

                panelControl.serializeXML(ref xmlPGM);

                int returnValue = ParadoxAPIImport.ControlPGM(panelID, xmlPGM);

                Log(panelID, returnValue, xmlPGM);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlPGM: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int PGMStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                int returnValue = ParadoxAPIImport.PGMStatus(panelID, out xmlStatus);

                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "PGMStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ControlDoor(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlDoor = "";

                panelControl.serializeXML(ref xmlDoor);

                int returnValue = ParadoxAPIImport.ControlDoor(panelID, xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlDoor: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int DoorStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                int returnValue = ParadoxAPIImport.DoorStatus(panelID, out xmlStatus);

                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "DoorStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ReadTimeStamp(UInt32 panelID, UInt32 blockID, PanelTimeStamp panelTimeStamp)
        {
            if (panelTimeStamp != null)
            {
                string xmlTimeStamp = "";

                int returnValue = ParadoxAPIImport.ReadTimeStamp(panelID, blockID, out xmlTimeStamp);

                panelTimeStamp.parseXML(xmlTimeStamp);

                Log(panelID, returnValue, xmlTimeStamp);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadTimeStamp: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }

            return -1;
        }

        public static int ReadDateTime(UInt32 panelID, DateTime dateTime)
        {
            if (dateTime != null)
            {
                Double dt = 0.0;

                int returnValue = ParadoxAPIImport.ReadDateTime(panelID, ref dt);

                dateTime = DateTime.FromOADate(dt);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadDateTime: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }

            return -1;
        }

        public static int WriteDateTime(UInt32 panelID, DateTime dateTime)
        {
            if (dateTime != null)
            {
                Double dt = dateTime.ToOADate();

                int returnValue = ParadoxAPIImport.WriteDateTime(panelID, dt);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteDateTime: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ReadArea(UInt32 panelID, UInt32 areaNo, PanelArea panelArea)
        {
            if (panelArea != null)
            {
                string xmlArea = "";

                int returnValue = ParadoxAPIImport.ReadArea(panelID, areaNo, out xmlArea);

                panelArea.parseXML(xmlArea);

                Log(panelID, returnValue, xmlArea);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadArea: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, areaNo, PanelObjectTypes.OT_AREA, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllAreas(UInt32 panelID, PanelAreaList panelAreas)
        {
            if (panelAreas != null)
            {
                string xmlAreas = "";

                var returnValue = ParadoxAPIImport.ReadAllAreas(panelID, out xmlAreas);

                panelAreas.parseXML(xmlAreas);

                Log(panelID, returnValue, xmlAreas);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllAreas: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_AREAS, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadZone(UInt32 panelID, UInt32 zoneNo, PanelZone panelZone)
        {
            if (panelZone != null)
            {
                string xmlZone = "";

                int returnValue = ParadoxAPIImport.ReadZone(panelID, zoneNo, out xmlZone);

                panelZone.parseXML(xmlZone);

                Log(panelID, returnValue, xmlZone);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadZone: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, zoneNo, PanelObjectTypes.OT_ZONE, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllZones(UInt32 panelID, PanelZoneList panelZones)
        {
            if (panelZones != null)
            {
                string xmlZones = "";

                int returnValue = ParadoxAPIImport.ReadAllZones(panelID, out xmlZones);

                panelZones.parseXML(xmlZones);

                Log(panelID, returnValue, xmlZones);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllZones: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_ZONES, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadPGM(UInt32 panelID, UInt32 pgmNo, PanelPGM panelPGM)
        {
            if (panelPGM != null)
            {
                string xmlPGM = "";

                int returnValue = ParadoxAPIImport.ReadPGM(panelID, pgmNo, out xmlPGM);

                panelPGM.parseXML(xmlPGM);

                Log(panelID, returnValue, xmlPGM);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadPGM: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, pgmNo, PanelObjectTypes.OT_PGM, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllPGMs(UInt32 panelID, PanelPGMList panelPGMs)
        {
            if (panelPGMs != null)
            {
                string xmlPGMs = "";

                int returnValue = ParadoxAPIImport.ReadAllPGMs(panelID, out xmlPGMs);

                panelPGMs.parseXML(xmlPGMs);

                Log(panelID, returnValue, xmlPGMs);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllPGMs: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_PGMS, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadDoor(UInt32 panelID, UInt32 doorNo, PanelDoor panelDoor)
        {
            if (panelDoor != null)
            {
                string xmlDoor = "";

                int returnValue = ParadoxAPIImport.ReadDoor(panelID, doorNo, out xmlDoor);

                panelDoor.parseXML(xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadDoor: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, doorNo, PanelObjectTypes.OT_DOOR, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int WriteDoor(UInt32 panelID, UInt32 doorNo, PanelDoor panelDoor)
        {
            if (panelDoor != null)
            {
                string xmlDoor = "";

                panelDoor.serializeXML(ref xmlDoor);

                int returnValue = ParadoxAPIImport.WriteDoor(panelID, doorNo, xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteDoor: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, doorNo, PanelObjectTypes.OT_DOOR, AT_WRITE);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllDoors(UInt32 panelID, PanelDoorList panelDoors)
        {
            if (panelDoors != null)
            {
                string xmlDoors = "";

                int returnValue = ParadoxAPIImport.ReadAllDoors(panelID, out xmlDoors);

                panelDoors.parseXML(xmlDoors);

                Log(panelID, returnValue, xmlDoors);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllDoors: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_DOORS, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadUser(UInt32 panelID, UInt32 userNo, PanelUser panelUser)
        {
            if (panelUser != null)
            {
                string xmlUser = "";

                int returnValue = ParadoxAPIImport.ReadUser(panelID, userNo, out xmlUser);

                panelUser.parseXML(xmlUser);

                Log(panelID, returnValue, xmlUser);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadUser: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, userNo, PanelObjectTypes.OT_USER, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int WriteUser(UInt32 panelID, UInt32 userNo, PanelUser panelUser)
        {
            if (panelUser != null)
            {
                string xmlUser = "";

                panelUser.serializeXML(ref xmlUser);

                int returnValue = ParadoxAPIImport.WriteUser(panelID, userNo, xmlUser);

                Log(panelID, returnValue, xmlUser);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteUser: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, userNo, PanelObjectTypes.OT_USER, AT_WRITE);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllUsers(UInt32 panelID, PanelUserList panelUsers)
        {
            if (panelUsers != null)
            {
                string xmlUsers = "";

                int returnValue = ParadoxAPIImport.ReadAllUsers(panelID, out xmlUsers);

                panelUsers.parseXML(xmlUsers);

                Log(panelID, returnValue, xmlUsers);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllUsers: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_USERS, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int WriteMultipleUsers(UInt32 panelID, PanelUserList panelUsers)
        {
            if (panelUsers != null)
            {
                string xmlUsers = "";

                panelUsers.serializeXML(ref xmlUsers);

                int returnValue = ParadoxAPIImport.WriteMultipleUsers(panelID, xmlUsers);

                Log(panelID, returnValue, xmlUsers);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteMultipleUsers: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_USERS, AT_WRITE);

                return returnValue;
            }

            return -1;
        }

        public static int ReadSchedule(UInt32 panelID, UInt32 scheduleNo, PanelSchedule panelSchedule)
        {
            if (panelSchedule != null)
            {
                string xmlSchedule = "";

                int returnValue = ParadoxAPIImport.ReadSchedule(panelID, scheduleNo, out xmlSchedule);

                panelSchedule.parseXML(xmlSchedule);

                Log(panelID, returnValue, xmlSchedule);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadSchedule: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, scheduleNo, PanelObjectTypes.OT_SCHEDULE, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int WriteSchedule(UInt32 panelID, UInt32 scheduleNo, PanelSchedule panelSchedule)
        {
            if (panelSchedule != null)
            {
                string xmlSchedule = "";

                panelSchedule.serializeXML(ref xmlSchedule);

                int returnValue = ParadoxAPIImport.WriteSchedule(panelID, scheduleNo, xmlSchedule);

                Log(panelID, returnValue, xmlSchedule);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteSchedule: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, scheduleNo, PanelObjectTypes.OT_SCHEDULE, AT_WRITE);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllSchedules(UInt32 panelID, PanelScheduleList panelSchedules)
        {
            if (panelSchedules != null)
            {
                string xmlSchedules = "";

                int returnValue = ParadoxAPIImport.ReadAllSchedules(panelID, out xmlSchedules);

                panelSchedules.parseXML(xmlSchedules);

                Log(panelID, returnValue, xmlSchedules);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllSchedules: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_SCHEDULES, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAccessLevel(UInt32 panelID, UInt32 accessLevelNo, PanelAccessLevel panelAccessLevel)
        {
            if (panelAccessLevel != null)
            {
                string xmlAccessLevel = "";

                int returnValue = ParadoxAPIImport.ReadAccessLevel(panelID, accessLevelNo, out xmlAccessLevel);

                panelAccessLevel.parseXML(xmlAccessLevel);

                Log(panelID, returnValue, xmlAccessLevel);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAccessLevel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, accessLevelNo, PanelObjectTypes.OT_ACCESS_LEVEL, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int WriteAccessLevel(UInt32 panelID, UInt32 accessLevelNo, PanelAccessLevel panelAccessLevel)
        {
            if (panelAccessLevel != null)
            {
                string xmlAccessLevel = "";

                panelAccessLevel.serializeXML(ref xmlAccessLevel);

                int returnValue = ParadoxAPIImport.WriteAccessLevel(panelID, accessLevelNo, xmlAccessLevel);

                Log(panelID, returnValue, xmlAccessLevel);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteAccessLevel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, accessLevelNo, PanelObjectTypes.OT_ACCESS_LEVEL, AT_WRITE);

                return returnValue;
            }

            return -1;
        }

        public static int ReadAllAccessLevels(UInt32 panelID, PanelAccessLevelList panelAccessLevels)
        {
            if (panelAccessLevels != null)
            {
                string xmlAccessLevels = "";

                int returnValue = ParadoxAPIImport.ReadAllAccessLevels(panelID, out xmlAccessLevels);

                panelAccessLevels.parseXML(xmlAccessLevels);

                Log(panelID, returnValue, xmlAccessLevels);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllAccessLevels: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_ACCESS_LEVELS, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int ReadHolidays(UInt32 panelID, PanelHolidayList panelHolidayList)
        {
            if (panelHolidayList != null)
            {
                string xmlHolidays = "";

                int returnValue = ParadoxAPIImport.ReadHolidays(panelID, out xmlHolidays);

                panelHolidayList.parseXML(xmlHolidays);

                Log(panelID, returnValue, xmlHolidays);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadHolidays: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_HOLIDAYS, AT_READ);

                return returnValue;
            }

            return -1;
        }

        public static int WriteHolidays(UInt32 panelID, PanelHolidayList panelHolidayList)
        {
            if (panelHolidayList != null)
            {
                string xmlHolidays = "";

                panelHolidayList.serializeXML(ref xmlHolidays);

                int returnValue = ParadoxAPIImport.WriteHolidays(panelID, xmlHolidays);

                Log(panelID, returnValue, xmlHolidays);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteHolidays: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_HOLIDAYS, AT_WRITE);

                return returnValue;
            }

            return -1;
        }

        public static int ReadBufferedEvents(UInt32 panelID, UInt32 eventCount)
        {
            int returnValue = ParadoxAPIImport.ReadBufferEvents(panelID, eventCount);

            Log(panelID, returnValue, string.Format("Read Buffered Event Count: {0}", eventCount));

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "ReadBufferedEvents: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static int ReadMonitoringStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlMonitoring = "";

                int returnValue = ParadoxAPIImport.ReadMonitoring(panelID, out xmlMonitoring);

                panelMonitoring.parseXML(xmlMonitoring);

                Log(panelID, returnValue, xmlMonitoring);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadMonitoringStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }

            return -1;
        }

        public static int GetSystemTroubles(UInt32 panelID, PanelTroubleList panelTroubleList)
        {
            if (panelTroubleList != null)
            {
                string xmlTroubles = "";

                int returnValue = ParadoxAPIImport.SystemTroubles(panelID, out xmlTroubles);

                panelTroubleList.parseXML(xmlTroubles);

                Log(panelID, returnValue, xmlTroubles);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "GetSystemTroubles: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }

            return -1;
        }

        public static int StartControlPanelMonitoring(UInt32 panelID)
        {
            int returnValue = ParadoxAPIImport.StartMonitoring(panelID);

            Log(panelID, returnValue, "Start Monitoring");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "StartControlPanelMonitoring: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static int WriteIPReporting(UInt32 panelID, UInt32 receiverID, PanelIPReporting panelIPReporting)
        {
            if (panelIPReporting != null)
            {
                string xmlReporting = "";

                panelIPReporting.serializeXML(ref xmlReporting);

                int returnValue = ParadoxAPIImport.WriteIPReporting(panelID, receiverID, xmlReporting);

                Log(panelID, returnValue, xmlReporting);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteIPReporting: " + PanelResults.GetResultCode((UInt32)returnValue));
                }
                else
                {
                    NotifyTaskCompleted(panelID, returnValue, (UInt32)panelIPReporting.ReceiverNo, PanelObjectTypes.OT_IP_RECEIVER, AT_WRITE);
                }

                return returnValue;
            }

            return -1;
        }

        public static int ReadIPReporting(UInt32 panelID, UInt32 receiverID, PanelIPReporting panelIPReporting)
        {
            if (panelIPReporting != null)
            {
                string xmlReporting = "";

                int returnValue = ParadoxAPIImport.ReadIPReporting(panelID, receiverID, out xmlReporting);

                panelIPReporting.parseXML(xmlReporting);

                Log(panelID, returnValue, xmlReporting);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadIPReporting: " + PanelResults.GetResultCode((UInt32)returnValue));
                }
                else
                {
                    NotifyTaskCompleted(panelID, returnValue, (UInt32)panelIPReporting.ReceiverNo, PanelObjectTypes.OT_IP_RECEIVER, AT_READ, panelIPReporting);
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }

            return -1;
        }

        public static int StartIPDOX(IPDOXSettings settings)
        {
            if (settings != null)
            {
                string xmlSetting = "";

                settings.serializeXML(ref xmlSetting);

                int returnValue = ParadoxAPIImport.StartIPDOX(xmlSetting);

                Log(0, returnValue, xmlSetting);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartIPDOX: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int StopIPDOX()
        {
            int returnValue = ParadoxAPIImport.StopIPDOX();

            Log(0, returnValue, "StopIPDOX");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "StopIPDOX: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static int DeleteIPDOXAccount(string macAddress)
        {
            int returnValue = ParadoxAPIImport.DeleteIPDOXAccount(macAddress);

            Log(0, returnValue, "DeleteIPDOXAccount");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "DeleteIPDOXAccount: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static int IPReportingStatus(UInt32 panelID, PanelIPReportingStatusList panelIPReportingStatusList)
        {
            if (panelIPReportingStatusList != null)
            {
                string xmlStatus = "";

                int returnValue = ParadoxAPIImport.IPReportingStatus(panelID, out xmlStatus);

                panelIPReportingStatusList.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "IPReportingStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }

            return -1;
        }

        public static int GetSiteFromPMH(string panelSerialNo, SiteInfo siteInfo)
        {
            if (siteInfo != null)
            {
                string xmlSiteInfo = "";

                int returnValue = ParadoxAPIImport.GetSiteFromPMH(panelSerialNo, out xmlSiteInfo);

                siteInfo.parseXML(xmlSiteInfo);

                Log(0, returnValue, xmlSiteInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "GetSiteFromPMH: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int ConfigureVideoServer(VideoSettings videoSettings)
        {
            if (videoSettings != null)
            {
                string xmlVideoSettings = "";

                videoSettings.serializeXML(ref xmlVideoSettings);

                int returnValue = ParadoxAPIImport.ConfigureVideoServer(xmlVideoSettings);

                Log(0, returnValue, xmlVideoSettings);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "ConfigureVideoServer: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int GetVideoAlarmFiles(string accountNo, UInt32 zoneNo, DateTime dateTime, VideoFileList videoFileList)
        {
            if (videoFileList != null)
            {
                string XMLVideoFiles = "";

                Double dt = dateTime.ToOADate();

                int returnValue = ParadoxAPIImport.GetVideoAlarmFiles(accountNo, zoneNo, dt, out XMLVideoFiles);

                videoFileList.parseXML(XMLVideoFiles);

                Log(0, returnValue, XMLVideoFiles);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "GetVideoAlarmFiles: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int StartVideoOnDemand(string ipAddress, UInt32 ipPort, string sessionKey, VideoFileList videoFileList)
        {
            if (videoFileList != null)
            {
                string xmlVideoFile = "";

                int returnValue = ParadoxAPIImport.StartVideoOnDemand(ipAddress, ipPort, sessionKey, out xmlVideoFile);

                videoFileList.parseXML(xmlVideoFile);

                Log(0, returnValue, xmlVideoFile);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartVideoOnDemand: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        public static int StartVideoOnDemandEx(UInt32 panelID, VODSettings vodSettings, VideoFileList videoFileList)
        {
            if ((videoFileList != null) && (vodSettings != null))
            {
                string xmlVideoFile = "";
                string xmlVODSettings = "";

                vodSettings.serializeXML(ref xmlVODSettings);

                int returnValue = ParadoxAPIImport.StartVideoOnDemandEx(panelID, xmlVODSettings, out xmlVideoFile);

                videoFileList.parseXML(xmlVideoFile);

                Log(0, returnValue, xmlVideoFile);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartVideoOnDemandEx: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }

            return -1;
        }

        #endregion
    }
}