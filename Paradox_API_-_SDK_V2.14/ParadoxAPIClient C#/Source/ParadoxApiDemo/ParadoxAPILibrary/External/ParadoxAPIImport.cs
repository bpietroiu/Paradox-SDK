using System;
using System.Runtime.InteropServices;

namespace ParadoxAPILibrary.External
{
    internal static class ParadoxAPIImport
    {
        #region Methods Entry Point

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetDriverVersion")]
        public static extern int GetDriverVersion([MarshalAs(UnmanagedType.BStr)] out string version);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DiscoverModule")]
        public static extern int DiscoverModule([MarshalAs(UnmanagedType.BStr)] out string xmlInfo);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DetectPanel")]
        public static extern int DetectPanel([MarshalAs(UnmanagedType.BStr)] string xmlSettings, [MarshalAs(UnmanagedType.BStr)] out string xmlInfo);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ConnectPanel")]
        public static extern int ConnectPanel([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlSettings, [MarshalAs(UnmanagedType.U4)] UInt32 waitTimeOut);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DisconnectPanel")]
        public static extern int DisconnectPanel([MarshalAs(UnmanagedType.U4)] UInt32 panelID);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RetrievePanelInfo")]
        public static extern int RetrievePanelInfo([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlInfo);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterPanel")]
        public static extern int RegisterPanel([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlAction);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ControlArea")]
        public static extern int ControlArea([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlArea);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "AreaStatus")]
        public static extern int AreaStatus([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlStatus);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ControlZone")]
        public static extern int ControlZone([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlZone);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ZoneStatus")]
        public static extern int ZoneStatus([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlStatus);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ControlPGM")]
        public static extern int ControlPGM([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlPGM);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "PGMStatus")]
        public static extern int PGMStatus([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlStatus);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ControlDoor")]
        public static extern int ControlDoor([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlDoor);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DoorStatus")]
        public static extern int DoorStatus([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlStatus);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadTimeStamp")]
        public static extern int ReadTimeStamp([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 blockID, [MarshalAs(UnmanagedType.BStr)] out string xmlTimeStamp);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadDateTime")]
        public static extern int ReadDateTime([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.R8)] ref double dateTime);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteDateTime")]
        public static extern int WriteDateTime([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.R8)] double dateTime);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadArea")]
        public static extern int ReadArea([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 areaNo, [MarshalAs(UnmanagedType.BStr)] out string xmlArea);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllAreas")]
        public static extern int ReadAllAreas([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlAreas);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadZone")]
        public static extern int ReadZone([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 zoneNo, [MarshalAs(UnmanagedType.BStr)] out string xmlZone);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllZones")]
        public static extern int ReadAllZones([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlZones);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadPGM")]
        public static extern int ReadPGM([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 pgmNo, [MarshalAs(UnmanagedType.BStr)] out string xmlPGM);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllPGMs")]
        public static extern int ReadAllPGMs([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlPGMs);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadDoor")]
        public static extern int ReadDoor([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 doorNo, [MarshalAs(UnmanagedType.BStr)] out string xmlDoor);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteDoor")]
        public static extern int WriteDoor([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 doorNo, [MarshalAs(UnmanagedType.BStr)] string xmlDoor);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllDoors")]
        public static extern int ReadAllDoors([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlDoors);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadUser")]
        public static extern int ReadUser([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 userNo, [MarshalAs(UnmanagedType.BStr)] out string xmlUser);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteUser")]
        public static extern int WriteUser([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 userNo, [MarshalAs(UnmanagedType.BStr)] string xmlUser);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllUsers")]
        public static extern int ReadAllUsers([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlUsers);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteMultipleUsers")]
        public static extern int WriteMultipleUsers([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlUsers);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadSchedule")]
        public static extern int ReadSchedule([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 scheduleNo, [MarshalAs(UnmanagedType.BStr)] out string xmlSchedule);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteSchedule")]
        public static extern int WriteSchedule([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 scheduleNo, [MarshalAs(UnmanagedType.BStr)] string xmlSchedule);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllSchedules")]
        public static extern int ReadAllSchedules([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlSchedules);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadHolidays")]
        public static extern int ReadHolidays([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlHolidays);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteHolidays")]
        public static extern int WriteHolidays([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlHolidays);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAccessLevel")]
        public static extern int ReadAccessLevel([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 accessLevelNo, [MarshalAs(UnmanagedType.BStr)] out string xmlAccessLevel);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteAccessLevel")]
        public static extern int WriteAccessLevel([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 accessLevelNo, [MarshalAs(UnmanagedType.BStr)] string xmlAccessLevel);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadAllAccessLevels")]
        public static extern int ReadAllAccessLevels([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlAccessLevels);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadIPReporting")]
        public static extern int ReadIPReporting([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 receiverNo, [MarshalAs(UnmanagedType.BStr)] out string xmlReporting);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "WriteIPReporting")]
        public static extern int WriteIPReporting([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 receiverNo, [MarshalAs(UnmanagedType.BStr)] string xmlReporting);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "IPReportingStatus")]
        public static extern int IPReportingStatus([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlStatus);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadBufferEvents")]
        public static extern int ReadBufferEvents([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 eventCount);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ReadMonitoring")]
        public static extern int ReadMonitoring([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlMonitoring);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "SystemTroubles")]
        public static extern int SystemTroubles([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] out string xmlTroubles);
        
        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "StartIPDOX")]
        public static extern int StartIPDOX([MarshalAs(UnmanagedType.BStr)] string xmlSetting);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "StopIPDOX")]
        public static extern int StopIPDOX();

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DeleteIPDOXAccount")]
        public static extern int DeleteIPDOXAccount([MarshalAs(UnmanagedType.BStr)] string macAddress);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "StartMonitoring")]
        public static extern int StartMonitoring([MarshalAs(UnmanagedType.U4)] UInt32 panelID);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "UnregisterAllCallback")]
        public static extern void UnregisterAllCallback();

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetSiteFromPMH")]
        public static extern int GetSiteFromPMH([MarshalAs(UnmanagedType.BStr)] string panelSerialNo, [MarshalAs(UnmanagedType.BStr)] out string xmlSiteInfo);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "ConfigureVideoServer")]
        public static extern int ConfigureVideoServer([MarshalAs(UnmanagedType.BStr)] string xmlVideoSettings);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetVideoAlarmFiles")]
        public static extern int GetVideoAlarmFiles([MarshalAs(UnmanagedType.BStr)] string accountNo, [MarshalAs(UnmanagedType.U4)] UInt32 zoneNo, [MarshalAs(UnmanagedType.R8)] Double dateTime, [MarshalAs(UnmanagedType.BStr)] out string xmlVideoFiles);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "StartVideoOnDemand")]
        public static extern int StartVideoOnDemand([MarshalAs(UnmanagedType.BStr)] string ipAddress, [MarshalAs(UnmanagedType.U4)] UInt32 ipPort, [MarshalAs(UnmanagedType.BStr)] string sessionKey, [MarshalAs(UnmanagedType.BStr)] out string xmlVideoFile);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "StartVideoOnDemandEx")]
        public static extern int StartVideoOnDemandEx([MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlVODSettings, [MarshalAs(UnmanagedType.BStr)] out string xmlVideoFile);

        #endregion

        #region Callbacks Entry Points

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcConnectionStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string status);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "RegisterConnectionStatusChangedCallback")]
        public static extern void RegisterConnectionStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcConnectionStatusChangedDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcProgressChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 task, [MarshalAs(UnmanagedType.BStr)] string description, [MarshalAs(UnmanagedType.U4)] UInt32 percent);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterProgressChangedCallback")]
        public static extern void RegisterProgressChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcProgressChangedDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcProgressErrorDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 task, [MarshalAs(UnmanagedType.U4)] UInt32 errorCode, [MarshalAs(UnmanagedType.BStr)] string errorMsg);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterProgressErrorCallback")]
        public static extern void RegisterProgressErrorCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcProgressErrorDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcReceiveReportingEventDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] string xmlEvents);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterReceiveReportingEventCallback")]
        public static extern void RegisterReceiveReportingEventCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcReceiveReportingEventDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcReceiveBufferEventDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlEvents);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterReceiveBufferEventCallback")]
        public static extern void RegisterReceiveBufferEventCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcReceiveBufferEventDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcReceiveLiveEventDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlEvents);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterReceiveLiveEventCallback")]
        public static extern void RegisterReceiveLiveEventCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcReceiveLiveEventDelegate callBackHandle);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcMonitoringStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlStatus);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterMonitoringStatusChangedCallback")]
        public static extern void RegisterMonitoringStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcMonitoringStatusChangedDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcHeartbeatDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterHeartbeatCallback")]
        public static extern void RegisterHeartbeatCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcHeartbeatDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcRxStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] int byteCount);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterRxStatusChangedCallback")]
        public static extern void RegisterRxStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcRxStatusChangedDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcTxStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] int byteCount);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterTxStatusChangedCallback")]
        public static extern void RegisterTxStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcTxStatusChangedDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcIPModuleDetectedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] string xmlModule);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterIPModuleDetectedCallback")]
        public static extern void RegisterIPModuleDetectedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcIPModuleDetectedDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcSMSRequestDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string sms);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterSMSRequestCallback")]
        public static extern void RegisterSMSRequestCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcSMSRequestDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcAccountRegistrationDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] string xmlAccounts);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterAccountRegistrationCallback")]
        public static extern void RegisterAccountRegistrationCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcAccountRegistrationDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcAccountUpdateDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] string xmlAccounts);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterAccountUpdateCallback")]
        public static extern void RegisterAccountUpdateCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcAccountUpdateDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcAccountLinkDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] string xmlAccounts);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterAccountLinkCallback")]
        public static extern void RegisterAccountLinkCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcAccountLinkDelegate callBackHandle);
        
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcIPDOXSocketChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 port, [MarshalAs(UnmanagedType.U4)] UInt32 status, [MarshalAs(UnmanagedType.BStr)] string description);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RegisterIPDOXSocketChangedCallback")]
        public static extern void RegisterIPDOXSocketChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcIPDOXSocketChangedDelegate callBackHandle);

        #endregion
    }
}