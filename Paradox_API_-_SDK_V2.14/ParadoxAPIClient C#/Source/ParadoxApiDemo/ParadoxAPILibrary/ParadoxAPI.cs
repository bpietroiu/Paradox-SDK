using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ParadoxAPILibrary.Model;

namespace ParadoxAPILibrary
{
    public class ParadoxAPI
    {
        #region Constants

        public static Form formRef = null;
        
        // Action Type
        public const string AT_READ = "Read";
        public const string AT_WRITE = "Write";
        
        // Control area
        public const string C_CONTROL_AREA_ARM       = "Arm";
        public const string C_CONTROL_AREA_FORCE     = "Force";
        public const string C_CONTROL_AREA_STAY      = "Stay";
        public const string C_CONTROL_AREA_SLEEP     = "Sleep";
        public const string C_CONTROL_AREA_INSTANT   = "Instant";
        public const string C_CONTROL_AREA_DISARM    = "Disarm";

        // Control Zone
        public const string C_CONTROL_ZONE_BYPASS    = "Bypass";
        public const string C_CONTROL_ZONE_UNBYPASS  = "Unbypass";

        // Control PGM
        public const string C_CONTROL_PGM_ON         = "On";
        public const string C_CONTROL_PGM_OFF        = "Off";
        public const string C_CONTROL_PGM_TEST       = "Test";

        // Control Door
        public const string C_CONTROL_DOOR_LOCK      = "Lock";
        public const string C_CONTROL_DOOR_UNLOCK    = "Unlock";

        #endregion

        #region Externs

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetDriverVersion")]
        public static extern Int32 GetDriverVersion([MarshalAs(UnmanagedType.BStr)] out string version);

        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DiscoverModule")]
        public static extern Int32 DiscoverModule([MarshalAs(UnmanagedType.BStr)] out string xmlInfo);
        
        [DllImport("ParadoxAPI.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DetectPanel")]
        public static extern Int32 DetectPanel([MarshalAs(UnmanagedType.BStr)] string xmlSettings, [MarshalAs(UnmanagedType.BStr)] out string xmlInfo);

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ConnectPanel")]
        public static extern Int32 ConnectPanel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlSettings,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 WaitTimeOut
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "DisconnectPanel")]
        public static extern Int32 DisconnectPanel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "RetrievePanelInfo")]
        public static extern Int32 RetrievePanelInfo(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlInfo
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "RegisterPanel")]
        public static extern Int32 RegisterPanel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlAction
                                                );


        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ControlArea")]
        public static extern Int32 ControlArea(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlArea
                                                );


        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "AreaStatus")]
        public static extern Int32 AreaStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ControlZone")]
        public static extern Int32 ControlZone(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlZone
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ZoneStatus")]
        public static extern Int32 ZoneStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ControlPGM")]
        public static extern Int32 ControlPGM(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlPGM
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "PGMStatus")]
        public static extern Int32 PGMStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 PanelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ControlDoor")]
        public static extern Int32 ControlDoor(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlDoor
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "DoorStatus")]
        public static extern Int32 DoorStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlStatus
                                                );


        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadTimeStamp")]
        public static extern Int32 ReadTimeStamp(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 blockID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlTimeStamp
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadDateTime")]
        public static extern Int32 ReadDateTime(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.R8)] ref Double DateTime
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "WriteDateTime")]
        public static extern Int32 WriteDateTime(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.R8)] Double DateTime
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadArea")]
        public static extern Int32 ReadArea(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 areaNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlArea
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllAreas")]
        public static extern Int32 ReadAllAreas(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlAreas
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadZone")]
        public static extern Int32 ReadZone(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 zoneNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlZone
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllZones")]
        public static extern Int32 ReadAllZones(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlZones
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadPGM")]
        public static extern Int32 ReadPGM(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 pgmNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlPGM
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllPGMs")]
        public static extern Int32 ReadAllPGMs(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlPGMs
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadDoor")]
        public static extern Int32 ReadDoor(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 doorNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlDoor
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteDoor")]
        public static extern Int32 WriteDoor(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 doorNo,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlDoor
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllDoors")]
        public static extern Int32 ReadAllDoors(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlDoors
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadUser")]
        public static extern Int32 ReadUser(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 userNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlUser
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteUser")]
        public static extern Int32 WriteUser(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 userNo,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlUser
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAllUsers")]
        public static extern Int32 ReadAllUsers(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlUsers
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteMultipleUsers")]
        public static extern Int32 WriteMultipleUsers(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlUsers
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadSchedule")]
        public static extern Int32 ReadSchedule(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 scheduleNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlSchedule
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteSchedule")]
        public static extern Int32 WriteSchedule(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 scheduleNo,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlSchedule
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAllSchedules")]
        public static extern Int32 ReadAllSchedules(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string XMLSchedules
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadHolidays")]
        public static extern Int32 ReadHolidays(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlHolidays
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteHolidays")]
        public static extern Int32 WriteHolidays(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlHolidays
                                                );
         
        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAccessLevel")]
        public static extern Int32 ReadAccessLevel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 accessLevelNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlAccessLevel
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteAccessLevel")]
        public static extern Int32 WriteAccessLevel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 accessLevelNo,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlAccessLevel
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAllAccessLevels")]
        public static extern Int32 ReadAllAccessLevels(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlAccessLevels
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadIPReporting")]
        public static extern Int32 ReadIPReporting(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 receiverNo,            
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlReporting
                                                );
        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteIPReporting")]
        public static extern Int32 WriteIPReporting(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 receiverNo,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlReporting
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "IPReportingStatus")]
        public static extern Int32 IPReportingStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadBufferEvents")]
        public static extern Int32 ReadBufferEvents(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 eventCount
                                                );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "ReadMonitoring")]
        public static extern Int32 ReadMonitoring(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlMonitoring
                                                );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "SystemTroubles")]
        public static extern Int32 SystemTroubles(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlTroubles
                                                );

        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "StartIPDOX")]
        public static extern Int32 startIPDOX([MarshalAs(UnmanagedType.BStr)] string xmlSetting);

        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "StopIPDOX")]
        public static extern Int32 stopIPDOX();
        
        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "DeleteIPDOXAccount")]
        public static extern Int32 deleteIPDOXAccount([MarshalAs(UnmanagedType.BStr)] string macAddress);
        
        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "StartMonitoring")]
        public static extern Int32 StartMonitoring(
                                                  [MarshalAs(UnmanagedType.U4)] UInt32 panelID
                                                  );        
        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "UnregisterAllCallback")]
        public static extern void UnregisterAllCallback();

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "GetSiteFromPMH")]
        public static extern Int32 GetSiteFromPMH(
                                                [MarshalAs(UnmanagedType.BStr)] string panelSerialNo,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlSiteInfo
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ConfigureVideoServer")]
        public static extern Int32 ConfigureVideoServer(
                                                [MarshalAs(UnmanagedType.BStr)] string xmlVideoSettings                                                
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "GetVideoAlarmFiles")]
        public static extern Int32 GetVideoAlarmFiles(
                                                [MarshalAs(UnmanagedType.BStr)] string accountNo,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 zoneNo,
                                                [MarshalAs(UnmanagedType.R8)] Double dateTime,
                                                [MarshalAs(UnmanagedType.BStr)] out string XMLVideoFiles
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "StartVideoOnDemand")]
        public static extern Int32 StartVideoOnDemand(
                                                [MarshalAs(UnmanagedType.BStr)] string ipAddress,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 ipPort,
                                                [MarshalAs(UnmanagedType.BStr)] string SessionKey,
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlVideoFile
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "StartVideoOnDemandEx")]
        public static extern Int32 StartVideoOnDemandEx(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] string xmlVODSettings,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out string xmlVideoFile
                                                );
                                                                     
        /// <summary>
        /// Callbacks RegisterConnectionStatusChangedCallback 
        /// </summary>
        private static ProcConnectionStatusChangedDelegate procConnectionStatusChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcConnectionStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                                 [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                                 [MarshalAs(UnmanagedType.BStr)] string status
                                                                 );
        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Ansi,
        EntryPoint = "RegisterConnectionStatusChangedCallback")]
        public static extern void RegisterConnectionStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcConnectionStatusChangedDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterProgressChangedCallback 
        /// </summary>
        private static ProcProgressChangedDelegate procProgressChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcProgressChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 task,
                                                         [MarshalAs(UnmanagedType.BStr)] string description,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 percent
                                                         );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterProgressChangedCallback")]
        public static extern void RegisterProgressChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcProgressChangedDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterProgressErrorCallback 
        /// </summary>
        private static ProcProgressErrorDelegate procProgressErrorDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcProgressErrorDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 task,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 errorCode,
                                                         [MarshalAs(UnmanagedType.BStr)] string errorMsg
                                                         );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterProgressErrorCallback")]
        public static extern void RegisterProgressErrorCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcProgressErrorDelegate callBackHandle);

        /// <summary>
        /// Callbacks RegisterReceiveReportingEventCallback 
        /// </summary>
        private static ProcReceiveReportingEventDelegate procReceiveReportingEventDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcReceiveReportingEventDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,                                                            
                                                            [MarshalAs(UnmanagedType.BStr)] string xmlEvents
                                                            );
        
        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterReceiveReportingEventCallback")]
        public static extern void RegisterReceiveReportingEventCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcReceiveReportingEventDelegate callBackHandle);

        /// <summary>
        /// Callbacks RegisterReceiveBufferEventCallback 
        /// </summary>
        private static ProcReceiveBufferEventDelegate procReceiveBufferEventDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcReceiveBufferEventDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                            [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                            [MarshalAs(UnmanagedType.BStr)] string xmlEvents
                                                            );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterReceiveBufferEventCallback")]
        public static extern void RegisterReceiveBufferEventCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcReceiveBufferEventDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterReceiveLiveEventCallback 
        /// </summary>
        private static ProcReceiveLiveEventDelegate procReceiveLiveEventDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcReceiveLiveEventDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                          [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                          [MarshalAs(UnmanagedType.BStr)] string xmlEvents
                                                          );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterReceiveLiveEventCallback")]
        public static extern void RegisterReceiveLiveEventCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcReceiveLiveEventDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterMonitoringStatusChangedCallback 
        /// </summary>
        private static ProcMonitoringStatusChangedDelegate procMonitoringStatusChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcMonitoringStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                                 [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                                 [MarshalAs(UnmanagedType.BStr)] string xmlStatus
                                                                 );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterMonitoringStatusChangedCallback")]
        public static extern void RegisterMonitoringStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcMonitoringStatusChangedDelegate callBackHandle);

        /// <summary>
        /// Callbacks RegisterHeartbeatCallback 
        /// </summary>
        private static ProcHeartbeatDelegate procHeartbeatDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcHeartbeatDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                   [MarshalAs(UnmanagedType.U4)] UInt32 panelID
                                                   );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterHeartbeatCallback")]
        public static extern void RegisterHeartbeatCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcHeartbeatDelegate callBackHandle);

        
        /// <summary>
        /// Callbacks RegisterRxStatusChangedCallback 
        /// </summary>
        private static ProcRxStatusChangedDelegate procRxStatusChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcRxStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                         [MarshalAs(UnmanagedType.U4)] Int32 byteCount
                                                        );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterRxStatusChangedCallback")]
        public static extern void RegisterRxStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcRxStatusChangedDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterTxStatusChangedCallback 
        /// </summary>
        private static ProcTxStatusChangedDelegate procTxStatusChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcTxStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                         [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                         [MarshalAs(UnmanagedType.U4)] Int32 byteCount
                                                        );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterTxStatusChangedCallback")]
        public static extern void RegisterTxStatusChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcTxStatusChangedDelegate callBackHandle);               

        /// <summary>
        /// Callbacks RegisterIPModuleDetectedCallback 
        /// </summary>
        private static ProcIPModuleDetectedDelegate procIPModuleDetectedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcIPModuleDetectedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,                                                          
                                                          [MarshalAs(UnmanagedType.BStr)] string xmlModule
                                                         );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterIPModuleDetectedCallback")]
        public static extern void RegisterIPModuleDetectedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcIPModuleDetectedDelegate callBackHandle);

        /// <summary>
        /// Callbacks RegisterSMSRequestCallback 
        /// </summary>
        private static ProcSMSRequestDelegate procSMSRequestDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcSMSRequestDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                    [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                    [MarshalAs(UnmanagedType.BStr)] string sms
                                                   );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterSMSRequestCallback")]
        public static extern void RegisterSMSRequestCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcSMSRequestDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterAccountRegistrationCallback 
        /// </summary>
        private static ProcAccountRegistrationDelegate procAccountRegistrationDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcAccountRegistrationDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,                                                             
                                                             [MarshalAs(UnmanagedType.BStr)] string xmlAccounts
                                                            );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterAccountRegistrationCallback")]
        public static extern void RegisterAccountRegistrationCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcAccountRegistrationDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterAccountUpdateCallback 
        /// </summary>
        private static ProcAccountUpdateDelegate procAccountUpdateDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcAccountUpdateDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,                                                       
                                                       [MarshalAs(UnmanagedType.BStr)] string xmlAccounts
                                                      );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterAccountUpdateCallback")]
        public static extern void RegisterAccountUpdateCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcAccountUpdateDelegate callBackHandle);


        /// <summary>
        /// Callbacks RegisterAccountLinkCallback 
        /// </summary>
        private static ProcAccountLinkDelegate procAccountLinkDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcAccountLinkDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,                                                     
                                                     [MarshalAs(UnmanagedType.BStr)] string xmlAccounts
                                                    );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterAccountLinkCallback")]
        public static extern void RegisterAccountLinkCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcAccountLinkDelegate callBackHandle);
        
        
        /// <summary>
        /// Callbacks RegisterIPDOXSocketChangedCallback 
        /// </summary>
        private static ProcIPDOXSocketChangedDelegate procIPDOXSocketChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcIPDOXSocketChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                     [MarshalAs(UnmanagedType.U4)] UInt32 port,
                                                     [MarshalAs(UnmanagedType.U4)] UInt32 status,
                                                     [MarshalAs(UnmanagedType.BStr)] string description
                                                    );        
        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterIPDOXSocketChangedCallback")]
        public static extern void RegisterIPDOXSocketChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcIPDOXSocketChangedDelegate callBackHandle);

        #endregion

        // Callbacks
        public static void RegisterAllCallback()
        {
            procConnectionStatusChangedDelegate = new ProcConnectionStatusChangedDelegate(ConnectionStatusChangedCalledFromParadoxAPI);
            RegisterConnectionStatusChangedCallback(procConnectionStatusChangedDelegate);

            procProgressChangedDelegate = new ProcProgressChangedDelegate(ProgressChangedCalledFromParadoxAPI);
            RegisterProgressChangedCallback(procProgressChangedDelegate);

            procProgressErrorDelegate = new ProcProgressErrorDelegate(ProgressErrorCalledFromParadoxAPI);
            RegisterProgressErrorCallback(procProgressErrorDelegate);

            procMonitoringStatusChangedDelegate = new ProcMonitoringStatusChangedDelegate(MonitoringStatusChangedCalledFromParadoxAPI);
            RegisterMonitoringStatusChangedCallback(procMonitoringStatusChangedDelegate);

            procReceiveLiveEventDelegate = new ProcReceiveLiveEventDelegate(ReceiveLiveEventCalledFromParadoxAPI);
            RegisterReceiveLiveEventCallback(procReceiveLiveEventDelegate);

            procReceiveBufferEventDelegate = new ProcReceiveBufferEventDelegate(ReceiveBufferEventCalledFromParadoxAPI);
            RegisterReceiveBufferEventCallback(procReceiveBufferEventDelegate);

            procReceiveReportingEventDelegate = new ProcReceiveReportingEventDelegate(ReceiveReportingEventCalledFromParadoxAPI);
            RegisterReceiveReportingEventCallback(procReceiveReportingEventDelegate);

            procHeartbeatDelegate = new ProcHeartbeatDelegate(HeartbeatCalledFromParadoxAPI);
            RegisterHeartbeatCallback(procHeartbeatDelegate);

            procRxStatusChangedDelegate = new ProcRxStatusChangedDelegate(RxStatusChangedCalledFromParadoxAPI);
            RegisterRxStatusChangedCallback(procRxStatusChangedDelegate);

            procTxStatusChangedDelegate = new ProcTxStatusChangedDelegate(TxStatusChangedCalledFromParadoxAPI);
            RegisterTxStatusChangedCallback(procTxStatusChangedDelegate);

            procIPModuleDetectedDelegate = new ProcIPModuleDetectedDelegate(IPModuleDetectedCalledFromParadoxAPI);
            RegisterIPModuleDetectedCallback(procIPModuleDetectedDelegate);

            procSMSRequestDelegate = new ProcSMSRequestDelegate(SMSRequestCalledFromParadoxAPI);
            RegisterSMSRequestCallback(procSMSRequestDelegate);

            procAccountRegistrationDelegate = new ProcAccountRegistrationDelegate(AccountRegistrationCalledFromParadoxAPI);
            RegisterAccountRegistrationCallback(procAccountRegistrationDelegate);

            procAccountUpdateDelegate = new ProcAccountUpdateDelegate(AccountUpdateCalledFromParadoxAPI);
            RegisterAccountUpdateCallback(procAccountUpdateDelegate);

            procAccountLinkDelegate = new ProcAccountLinkDelegate(AccountLinkCalledFromParadoxAPI);
            RegisterAccountLinkCallback(procAccountLinkDelegate);

            procIPDOXSocketChangedDelegate = new ProcIPDOXSocketChangedDelegate(IPDOXSocketChangedCalledFromParadoxAPI);
            RegisterIPDOXSocketChangedCallback(procIPDOXSocketChangedDelegate);                     
        }

        public delegate void MonitoringStatusChangesDelegate(UInt32 panelID, PanelMonitoring panelMonitoring);

        public static MonitoringStatusChangesDelegate monitoringStatusChangesDelegate = null;

        // Callback
        public static void MonitoringStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] string xmlStatus)
        {

            if (monitoringStatusChangesDelegate != null)
            {
                PanelMonitoring panelMonitoring = new PanelMonitoring();
                try
                {
                    panelMonitoring.parseXML(xmlStatus);

                    monitoringStatusChangesDelegate(panelID, panelMonitoring);
                }
                finally
                {
                    panelMonitoring.Dispose();
                }                
            }
        }

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


        public delegate void RxStatusChangedDelegate(UInt32 panelID, Int32 byteCount);

        public static RxStatusChangedDelegate rxStatusChangedDelegate = null;

        // Callback
        public static void RxStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID, Int32 byteCount)
        {
            if (rxStatusChangedDelegate != null)
            {
                rxStatusChangedDelegate(panelID, byteCount);
            }
        }

        public delegate void TxStatusChangedDelegate(UInt32 panelID, Int32 byteCount);

        public static TxStatusChangedDelegate txStatusChangedDelegate = null;

        // Callback
        public static void TxStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID, Int32 byteCount)
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
              
        public delegate void LogDelegate(UInt32 panelID, Int32 returnValue, string logs);

        public static LogDelegate logDelegate = null;

        private static void Log(UInt32 panelID, Int32 returnValue, string value)
        {            
            if ((formRef != null) && (logDelegate != null))
            {
                formRef.Invoke(logDelegate, new object[] {panelID, returnValue, value});                
            }
        }

        public delegate void NotifyTaskCompletedDelegate(UInt32 panelID, Int32 returnValue, UInt32 ItemNo, string ItemType, string ActionType, object obj = null);

        public static NotifyTaskCompletedDelegate notifyTaskCompletedDelegate = null;

        private static void NotifyTaskCompleted(UInt32 panelID, Int32 returnValue, UInt32 ItemNo, string ItemType, string ActionType, object obj = null)
        {
            if ((formRef != null) && (notifyTaskCompletedDelegate != null))
            {
                formRef.Invoke(notifyTaskCompletedDelegate, new object[] { panelID, returnValue, ItemNo, ItemType, ActionType, obj });
            }
        }

        public delegate void RefreshNotificationDelegate(UInt32 panelID, Int32 returnValue);

        public static RefreshNotificationDelegate refreshNotificationDelegate = null;

        private static void RefreshNotification(UInt32 panelID, Int32 returnValue)
        {
            if ((formRef != null) && (refreshNotificationDelegate != null))
            {
                formRef.Invoke(refreshNotificationDelegate, new object[] { panelID, returnValue });                
            }
        }

        public delegate void ErrorNotificationDelegate(UInt32 panelID, Int32 returnValue, string ErrorMsg);

        public static ErrorNotificationDelegate errorNotificationDelegate = null;

        private static void errorNotification(UInt32 panelID, Int32 returnValue, string ErrorMsg)
        {
            if ((formRef != null) && (errorNotificationDelegate != null))
            {
                formRef.Invoke(errorNotificationDelegate, new object[] { panelID, returnValue, ErrorMsg });
            }
        }

        public static Int32 GetAPIVersion(ref string version)
        {
            Int32 returnValue = GetDriverVersion(out version);                      

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "GetDriverVersion: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }
                
        public static Int32 DiscoverModules(ModuleInfoList moduleInfoList)
        {
            if (moduleInfoList != null)
            {
                string xmlInfo = "";

                Int32 returnValue = DiscoverModule(out xmlInfo);

                moduleInfoList.parseXML(xmlInfo);

                Log(0, returnValue, xmlInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "DiscoverModules: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ConnectToPanel(UInt32 panelID, PanelSettings panelSettings, UInt32 WaitTimeOut = 30)
        {
            if (panelSettings != null)
            {
                string xmlSettings = "";

                panelSettings.serializeXML(ref xmlSettings);

                Int32 returnValue = ConnectPanel(panelID, xmlSettings, WaitTimeOut);

                Log(panelID, returnValue, xmlSettings);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ConnectToPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }                       
        }

        public static Int32 DisconnectFromPanel(UInt32 panelID)
        {
            Int32 returnValue = DisconnectPanel(panelID);

            Log(panelID, returnValue, "Disconnet");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "DisconnectFromPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static Int32 DetectPanel(UInt32 panelID, PanelSettings panelSettings, PanelInfo panelInfo)
        {
            if ((panelSettings != null) && (panelInfo != null)) 
            {
                string xmlSettings = "";
                string xmlInfo = "";
                
                panelSettings.serializeXML(ref xmlSettings);

                Int32 returnValue = DetectPanel(xmlSettings, out xmlInfo);

                panelInfo.parseXML(xmlInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "DetectPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }                      
        }

        public static Int32 RetrievePanelInfo(UInt32 panelID, PanelInfoEx panelInfoEx)
        {
            if (panelInfoEx != null)
            {
                string xmlInfo = "";

                Int32 returnValue = RetrievePanelInfo(panelID, out xmlInfo);
                
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
            else
            {
                return -1;
            }     
        }

        public static Int32 RegisterPanel(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlAction = "";

                panelControl.serializeXML(ref xmlAction);
                
                Int32 returnValue = RegisterPanel(panelID, xmlAction);

                Log(panelID, returnValue, xmlAction);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "RegisterPanel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }   
        }

        public static Int32 ControlArea(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlArea = "";

                panelControl.serializeXML(ref xmlArea);                

                Int32 returnValue = ControlArea(panelID, xmlArea);

                Log(panelID, returnValue, xmlArea);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlArea: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }               
        }

        public static Int32 AreaStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                Int32 returnValue = AreaStatus(panelID, out xmlStatus);

                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "AreaStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }   
        }

        public static Int32 ControlZone(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlZone = "";

                panelControl.serializeXML(ref xmlZone);                

                Int32 returnValue = ControlZone(panelID, xmlZone);

                Log(panelID, returnValue, xmlZone);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlZone: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }                   
        }

        public static Int32 ZoneStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                Int32 returnValue = ZoneStatus(panelID, out xmlStatus);
                
                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ZoneStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }              
        }

        public static Int32 ControlPGM(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlPGM = "";

                panelControl.serializeXML(ref xmlPGM);               

                Int32 returnValue = ControlPGM(panelID, xmlPGM);

                Log(panelID, returnValue, xmlPGM);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlPGM: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }           
        }

        public static Int32 PGMStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                Int32 returnValue = PGMStatus(panelID, out xmlStatus);
              
                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "PGMStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }                          
        }

        public static Int32 ControlDoor(UInt32 panelID, PanelControl panelControl)
        {
            if (panelControl != null)
            {
                string xmlDoor = "";

                panelControl.serializeXML(ref xmlDoor);               

                Int32 returnValue = ControlDoor(panelID, xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlDoor: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }         
        }

        public static Int32 DoorStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlStatus = "";

                Int32 returnValue = DoorStatus(panelID, out xmlStatus);
                
                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "DoorStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }                            
        }

        public static Int32 ReadTimeStamp(UInt32 panelID, UInt32 blockID, PanelTimeStamp panelTimeStamp)
        {
            if (panelTimeStamp != null)
            {
                string xmlTimeStamp = "";

                Int32 returnValue = ReadTimeStamp(panelID, blockID, out xmlTimeStamp);                

                panelTimeStamp.parseXML(xmlTimeStamp);

                Log(panelID, returnValue, xmlTimeStamp);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadTimeStamp: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }
            else
            {
                return -1;
            }     
        }

        public static Int32 ReadDateTime(UInt32 panelID, DateTime dateTime)
        {
            if (dateTime != null)
            {
                Double dt = 0.0;

                Int32 returnValue = ReadDateTime(panelID, ref dt);

                dateTime = DateTime.FromOADate(dt);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadDateTime: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }
            else
            {
                return -1;
            }    
        }

        public static Int32 WriteDateTime(UInt32 panelID, DateTime dateTime)
        {
            if (dateTime != null)
            {
                Double dt = dateTime.ToOADate();                               

                Int32 returnValue = WriteDateTime(panelID, dt);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteDateTime: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }   
        }

        public static Int32 ReadArea(UInt32 panelID, UInt32 areaNo, PanelArea panelArea)
        {
            if (panelArea != null)
            {
                string xmlArea = "";

                Int32 returnValue = ReadArea(panelID, areaNo, out xmlArea);
               
                panelArea.parseXML(xmlArea);

                Log(panelID, returnValue, xmlArea);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadArea: " + PanelResults.GetResultCode((UInt32)returnValue));
                }
                
                NotifyTaskCompleted(panelID, returnValue, areaNo, PanelObjectTypes.OT_AREA, AT_READ);
                                
                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadAllAreas(UInt32 panelID, PanelAreaList panelAreas)
        {
            if (panelAreas != null)
            {
                string xmlAreas = "";

                Int32 returnValue = ReadAllAreas(panelID, out xmlAreas);

                panelAreas.parseXML(xmlAreas);

                Log(panelID, returnValue, xmlAreas);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllAreas: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_AREAS, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadZone(UInt32 panelID, UInt32 zoneNo, PanelZone panelZone)
        {
            if (panelZone != null)
            {
                string xmlZone = "";

                Int32 returnValue = ReadZone(panelID, zoneNo, out xmlZone);
                                
                panelZone.parseXML(xmlZone);

                Log(panelID, returnValue, xmlZone);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadZone: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, zoneNo, PanelObjectTypes.OT_ZONE, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }
        
        public static Int32 ReadAllZones(UInt32 panelID, PanelZoneList panelZones)
        {
            if (panelZones != null)
            {
                string xmlZones = "";

                Int32 returnValue = ReadAllZones(panelID, out xmlZones);

                panelZones.parseXML(xmlZones);

                Log(panelID, returnValue, xmlZones);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllZones: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_ZONES, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadPGM(UInt32 panelID, UInt32 pgmNo, PanelPGM panelPGM)
        {
            if (panelPGM != null)
            {
                string xmlPGM = "";

                Int32 returnValue = ReadPGM(panelID, pgmNo, out xmlPGM);

                panelPGM.parseXML(xmlPGM);

                Log(panelID, returnValue, xmlPGM);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadPGM: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, pgmNo, PanelObjectTypes.OT_PGM, AT_READ);

                return returnValue;
            }
            else 
            {
                return -1;
            }
        }

        public static Int32 ReadAllPGMs(UInt32 panelID, PanelPGMList panelPGMs)
        {
            if (panelPGMs != null)
            {
                string xmlPGMs = "";

                Int32 returnValue = ReadAllPGMs(panelID, out xmlPGMs);

                panelPGMs.parseXML(xmlPGMs);

                Log(panelID, returnValue, xmlPGMs);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllPGMs: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_PGMS, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadDoor(UInt32 panelID, UInt32 doorNo, PanelDoor panelDoor)
        {
            if (panelDoor != null)
            {
                string xmlDoor = "";
            
                Int32 returnValue = ReadDoor(panelID, doorNo, out xmlDoor);
               
                panelDoor.parseXML(xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadDoor: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, doorNo, PanelObjectTypes.OT_DOOR, AT_READ);
                
                return returnValue;
            }
            else 
            {
                return -1;
            }
        }

        public static Int32 WriteDoor(UInt32 panelID, UInt32 doorNo, PanelDoor panelDoor)
        {
            if (panelDoor != null)
            {
                string xmlDoor = "";

                panelDoor.serializeXML(ref xmlDoor);

                Int32 returnValue = WriteDoor(panelID, doorNo, xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteDoor: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, doorNo, PanelObjectTypes.OT_DOOR, AT_WRITE);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadAllDoors(UInt32 panelID, PanelDoorList panelDoors)
        {
            if (panelDoors != null)
            {
                string xmlDoors = "";

                Int32 returnValue = ReadAllDoors(panelID, out xmlDoors);

                panelDoors.parseXML(xmlDoors);

                Log(panelID, returnValue, xmlDoors);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllDoors: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_DOORS, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadUser(UInt32 panelID, UInt32 userNo, PanelUser panelUser)
        {
            if (panelUser != null)
            {
                string xmlUser = "";
                
                Int32 returnValue = ReadUser(panelID, userNo, out xmlUser);                          
                             
                panelUser.parseXML(xmlUser);

                Log(panelID, returnValue, xmlUser);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadUser: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, userNo, PanelObjectTypes.OT_USER, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 WriteUser(UInt32 panelID, UInt32 userNo, PanelUser panelUser)
        {
            if (panelUser != null)
            {
                string xmlUser = "";

                panelUser.serializeXML(ref xmlUser);                
                
                Int32 returnValue = WriteUser(panelID, userNo, xmlUser);

                Log(panelID, returnValue, xmlUser);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteUser: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, userNo, PanelObjectTypes.OT_USER, AT_WRITE);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadAllUsers(UInt32 panelID, PanelUserList panelUsers)
        {
            if (panelUsers != null)
            {
                string xmlUsers = "";

                Int32 returnValue = ReadAllUsers(panelID, out xmlUsers);

                panelUsers.parseXML(xmlUsers);

                Log(panelID, returnValue, xmlUsers);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllUsers: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_USERS, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 WriteMultipleUsers(UInt32 panelID, PanelUserList panelUsers)
        {
            if (panelUsers != null)
            {
                string xmlUsers = "";

                panelUsers.serializeXML(ref xmlUsers);
                              
                Int32 returnValue = WriteMultipleUsers(panelID, xmlUsers);                               

                Log(panelID, returnValue, xmlUsers);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteMultipleUsers: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_USERS, AT_WRITE);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadSchedule(UInt32 panelID, UInt32 scheduleNo, PanelSchedule panelSchedule)
        {
            if (panelSchedule != null)
            {
                string xmlSchedule = "";

                Int32 returnValue = ReadSchedule(panelID, scheduleNo, out xmlSchedule);

                panelSchedule.parseXML(xmlSchedule);

                Log(panelID, returnValue, xmlSchedule);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadSchedule: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, scheduleNo, PanelObjectTypes.OT_SCHEDULE, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 WriteSchedule(UInt32 panelID, UInt32 scheduleNo, PanelSchedule panelSchedule)
        {
            if (panelSchedule != null)
            {
                string xmlSchedule = "";

                panelSchedule.serializeXML(ref xmlSchedule);

                Int32 returnValue = WriteSchedule(panelID, scheduleNo, xmlSchedule);

                Log(panelID, returnValue, xmlSchedule);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteSchedule: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, scheduleNo, PanelObjectTypes.OT_SCHEDULE, AT_WRITE);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadAllSchedules(UInt32 panelID, PanelScheduleList panelSchedules)
        {
            if (panelSchedules != null)
            {
                string xmlSchedules = "";

                Int32 returnValue = ReadAllSchedules(panelID, out xmlSchedules);                              
                
                panelSchedules.parseXML(xmlSchedules);
                                              
                Log(panelID, returnValue, xmlSchedules);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllSchedules: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_SCHEDULES, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadAccessLevel(UInt32 panelID, UInt32 accessLevelNo, PanelAccessLevel panelAccessLevel)
        {
            if (panelAccessLevel != null)
            {
                string xmlAccessLevel = "";

                Int32 returnValue = ReadAccessLevel(panelID, accessLevelNo, out xmlAccessLevel);

                panelAccessLevel.parseXML(xmlAccessLevel);

                Log(panelID, returnValue, xmlAccessLevel);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAccessLevel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, accessLevelNo, PanelObjectTypes.OT_ACCESS_LEVEL, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 WriteAccessLevel(UInt32 panelID, UInt32 accessLevelNo, PanelAccessLevel panelAccessLevel)
        {
            if (panelAccessLevel != null)
            {
                string xmlAccessLevel = "";

                panelAccessLevel.serializeXML(ref xmlAccessLevel);

                Int32 returnValue = WriteAccessLevel(panelID, accessLevelNo, xmlAccessLevel);

                Log(panelID, returnValue, xmlAccessLevel);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteAccessLevel: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, accessLevelNo, PanelObjectTypes.OT_ACCESS_LEVEL, AT_WRITE);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadAllAccessLevels(UInt32 panelID, PanelAccessLevelList panelAccessLevels)
        {
            if (panelAccessLevels != null)
            {
                string xmlAccessLevels = "";

                Int32 returnValue = ReadAllAccessLevels(panelID, out xmlAccessLevels);

                panelAccessLevels.parseXML(xmlAccessLevels);

                Log(panelID, returnValue, xmlAccessLevels);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllAccessLevels: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_ACCESS_LEVELS, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadHolidays(UInt32 panelID, PanelHolidayList panelHolidayList)
        {
            if (panelHolidayList != null)
            {
                string xmlHolidays = "";

                Int32 returnValue = ReadHolidays(panelID, out xmlHolidays);

                panelHolidayList.parseXML(xmlHolidays);

                Log(panelID, returnValue, xmlHolidays);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadHolidays: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_HOLIDAYS, AT_READ);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 WriteHolidays(UInt32 panelID, PanelHolidayList panelHolidayList)
        {
            if (panelHolidayList != null)
            {
                string xmlHolidays = "";

                panelHolidayList.serializeXML(ref xmlHolidays);

                Int32 returnValue = WriteHolidays(panelID, xmlHolidays);

                Log(panelID, returnValue, xmlHolidays);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteHolidays: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, PanelObjectTypes.OT_HOLIDAYS, AT_WRITE);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 ReadBufferedEvents(UInt32 panelID, UInt32 eventCount)
        {
            Int32 returnValue = ReadBufferEvents(panelID, eventCount);

            Log(panelID, returnValue, string.Format("Read Buffered Event Count: {0}", eventCount));

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "ReadBufferedEvents: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static Int32 ReadMonitoringStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                string xmlMonitoring = "";

                Int32 returnValue = ReadMonitoring(panelID, out xmlMonitoring);
                
                panelMonitoring.parseXML(xmlMonitoring);

                Log(panelID, returnValue, xmlMonitoring);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadMonitoringStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 GetSystemTroubles(UInt32 panelID, PanelTroubleList panelTroubleList)
        {
            if (panelTroubleList != null)
            {
                string xmlTroubles = "";

                Int32 returnValue = SystemTroubles(panelID, out xmlTroubles);                

                panelTroubleList.parseXML(xmlTroubles);

                Log(panelID, returnValue, xmlTroubles);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "GetSystemTroubles: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 StartControlPanelMonitoring(UInt32 panelID)
        {
            
            Int32 returnValue = StartMonitoring(panelID);

            Log(panelID, returnValue, "Start Monitoring");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "StartControlPanelMonitoring: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;            
        }

        public static Int32 WriteIPReporting(UInt32 panelID, UInt32 receiverID, PanelIPReporting panelIPReporting)
        {
            if (panelIPReporting != null)
            {
                string xmlReporting = "";

                panelIPReporting.serializeXML(ref xmlReporting);

                Int32 returnValue = WriteIPReporting(panelID, receiverID, xmlReporting);

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
            else
            {
                return -1;
            }
        }

        public static Int32 ReadIPReporting(UInt32 panelID, UInt32 receiverID, PanelIPReporting panelIPReporting)
        {
            if (panelIPReporting != null)
            {
                string xmlReporting = "";

                Int32 returnValue = ReadIPReporting(panelID, receiverID, out xmlReporting);

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
            else
            {
                return -1;
            }
        }

        public static Int32 StartIPDOX(IPDOXSettings ipDOXSettings)
        {
            if (ipDOXSettings != null)
            {
                string xmlSetting = "";

                ipDOXSettings.serializeXML(ref xmlSetting);

                Int32 returnValue = startIPDOX(xmlSetting);

                Log(0, returnValue, xmlSetting);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartIPDOX: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 StopIPDOX()
        {                 
            Int32 returnValue = stopIPDOX();

            Log(0, returnValue, "StopIPDOX");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "StopIPDOX: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;            
        }

        public static Int32 DeleteIPDOXAccount(string macAddress)
        {
            Int32 returnValue = ParadoxAPI.DeleteIPDOXAccount(macAddress);

            Log(0, returnValue, "DeleteIPDOXAccount");

            if (!PanelResults.Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "DeleteIPDOXAccount: " + PanelResults.GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static Int32 IPReportingStatus(UInt32 panelID, PanelIPReportingStatusList panelIPReportingStatusList)
        {
            if (panelIPReportingStatusList != null)
            {
                string xmlStatus = "";

                Int32 returnValue = IPReportingStatus(panelID, out xmlStatus);

                panelIPReportingStatusList.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "IPReportingStatus: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 GetSiteFromPMH(string panelSerialNo, SiteInfo siteInfo)
        {
            if (siteInfo != null)
            {
                string xmlSiteInfo = "";                                

                Int32 returnValue = GetSiteFromPMH(panelSerialNo, out xmlSiteInfo);

                siteInfo.parseXML(xmlSiteInfo);

                Log(0, returnValue, xmlSiteInfo);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "GetSiteFromPMH: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }
        
        public static Int32 ConfigureVideoServer(VideoSettings videoSettings)
        {
            if (videoSettings != null)
            {
                string xmlVideoSettings = "";

                videoSettings.serializeXML(ref xmlVideoSettings);

                Int32 returnValue = ConfigureVideoServer(xmlVideoSettings);

                Log(0, returnValue, xmlVideoSettings);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "ConfigureVideoServer: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 GetVideoAlarmFiles(string accountNo, UInt32 zoneNo, DateTime dateTime, VideoFileList videoFileList)
        {
            if (videoFileList != null)
            {
                string XMLVideoFiles = "";

                Double dt = dateTime.ToOADate();

                Int32 returnValue = GetVideoAlarmFiles(accountNo, zoneNo, dt, out XMLVideoFiles);

                videoFileList.parseXML(XMLVideoFiles);

                Log(0, returnValue, XMLVideoFiles);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "GetVideoAlarmFiles: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }


        public static Int32 StartVideoOnDemand(string ipAddress, UInt32 ipPort, string SessionKey, VideoFileList videoFileList)
        {
            if (videoFileList != null)
            {
                string xmlVideoFile = "";                               

                Int32 returnValue = StartVideoOnDemand(ipAddress, ipPort, SessionKey, out xmlVideoFile);

                videoFileList.parseXML(xmlVideoFile);

                Log(0, returnValue, xmlVideoFile);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartVideoOnDemand: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        } 

        public static Int32 StartVideoOnDemandEx(UInt32 panelID, VODSettings vodSettings, VideoFileList videoFileList)
        {
            if ((videoFileList != null) && (vodSettings != null))
            {
                string xmlVideoFile = "";
                string xmlVODSettings = "";

                vodSettings.serializeXML(ref xmlVODSettings);

                Int32 returnValue = StartVideoOnDemandEx(panelID, xmlVODSettings, out xmlVideoFile);

                videoFileList.parseXML(xmlVideoFile);

                Log(0, returnValue, xmlVideoFile);

                if (!PanelResults.Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartVideoOnDemandEx: " + PanelResults.GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }
    }
}