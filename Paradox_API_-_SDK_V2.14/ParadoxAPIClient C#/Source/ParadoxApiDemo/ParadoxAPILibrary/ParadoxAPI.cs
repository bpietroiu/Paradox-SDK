using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Xml;
using System.Windows.Forms;

namespace ParadoxAPILibrary
{
    public class ParadoxAPI
    {
        public static Form formRef = null;
        const UInt32 E_FAIL = 0x80004005;

        const UInt32 S_PN_RESULT_PID_BASE = 0x02000000;
        const UInt32 E_PN_RESULT_PID_BASE = 0x82000000;

        // Paradox API and CMSI result codes
        public const UInt32 E_PID002A_RESULT_FAILURE = E_PN_RESULT_PID_BASE + 0x00002A00;
        public const UInt32 S_PID002A_RESULT_SUCCESS = S_PN_RESULT_PID_BASE + 0x00002A00;


        public const UInt32 S_PID002A_REQUEST_IN_PROGRESS               = S_PID002A_RESULT_SUCCESS + 0x01;
        public const UInt32 S_PID002A_SMS_CALLBACK_MODEM_SUCCESS        = S_PID002A_RESULT_SUCCESS + 0x02; // TX SMS (callback message) via modem successfull
        public const UInt32 S_PID002A_SMS_CALLBACK_CONNECTION_SUCCESS   = S_PID002A_RESULT_SUCCESS + 0x03; // SMS callback successfull (PCS connection has been accepted)

        public const UInt32 E_PID002A_INVALID_COMMAND                   = E_PID002A_RESULT_FAILURE + 0x01;
        public const UInt32 E_PID002A_INVALID_SESSION                   = E_PID002A_RESULT_FAILURE + 0x02;
        public const UInt32 E_PID002A_ALREADY_CONNECTED                 = E_PID002A_RESULT_FAILURE + 0x08;
        public const UInt32 E_PID002A_FAILED_TO_CREATE_KEY              = E_PID002A_RESULT_FAILURE + 0x09;
        public const UInt32 E_PID002A_FAILED_TO_CRYPT_PACKET            = E_PID002A_RESULT_FAILURE + 0x0B;

        public const UInt32 E_PID002A_INVALID_PANEL_ID                  = E_PID002A_RESULT_FAILURE + 0x0C;
        public const UInt32 E_PID002A_INVALID_PANEL_NOT_CONNECTED       = E_PID002A_RESULT_FAILURE + 0x0E;
        public const UInt32 E_PID002A_INVALID_COMMAND_NOT_SUPPORTED     = E_PID002A_RESULT_FAILURE + 0x0F;
        public const UInt32 E_PID002A_SESSION_TIMEOUT                   = E_PID002A_RESULT_FAILURE + 0x10;
        public const UInt32 E_PID002A_INVALID_DATETIME_FORMAT           = E_PID002A_RESULT_FAILURE + 0x11;

        public const UInt32 E_PID002A_SMS_CALLBACK_MODEM_FAILURE        = E_PID002A_RESULT_FAILURE + 0x12; // TX SMS via modem failed
        public const UInt32 E_PID002A_SMS_CALLBACK_CONNECTION_FAILURE   = E_PID002A_RESULT_FAILURE + 0x13;

        public const UInt32 E_PID002A_INVALID_EVENT_NOT_FOUND           = E_PID002A_RESULT_FAILURE + 0x16;
        public const UInt32 E_PID002A_INVALID_PANEL_USER_FULL           = E_PID002A_RESULT_FAILURE + 0x24;
        public const UInt32 E_PID002A_INVALID_PANEL_USER_ID             = E_PID002A_RESULT_FAILURE + 0x25;
        public const UInt32 E_PID002A_INVALID_PANEL_USER_UNASSIGNED     = E_PID002A_RESULT_FAILURE + 0x27;
        
        public const UInt32 E_PID002A_INVALID_XML_FORMAT                = E_PID002A_RESULT_FAILURE + 0x28;

        public const UInt32 E_PID002A_INVALID_COM_PORT                  = E_PID002A_RESULT_FAILURE + 0x29;
        public const UInt32 E_PID002A_INVALID_BAUD_RATE                 = E_PID002A_RESULT_FAILURE + 0x2A;
        public const UInt32 E_PID002A_INVALID_IP_ADDRESS                = E_PID002A_RESULT_FAILURE + 0x2B;
        public const UInt32 E_PID002A_INVALID_IP_PORT                   = E_PID002A_RESULT_FAILURE + 0x2C;
        public const UInt32 E_PID002A_INVALID_SITE_ID                   = E_PID002A_RESULT_FAILURE + 0x2D;
        public const UInt32 E_PID002A_INVALID_IP_PASSWORD               = E_PID002A_RESULT_FAILURE + 0x2E;
        public const UInt32 E_PID002A_INVALID_COM_TYPE                  = E_PID002A_RESULT_FAILURE + 0x2F; // com type not supported
        public const UInt32 E_PID002A_INVALID_USERCODE                  = E_PID002A_RESULT_FAILURE + 0x30;

        public const UInt32 E_PID002A_INVALID_USER_RIGHTS               = E_PID002A_RESULT_FAILURE + 0x31;

        public const UInt32 E_PID002A_COMMAND_TIMEOUT                   = E_PID002A_RESULT_FAILURE + 0x32;
        public const UInt32 E_PID002A_PANEL_NOT_FOUND                   = E_PID002A_RESULT_FAILURE + 0x33;
        public const UInt32 E_PID002A_TASK_CANCELLED                    = E_PID002A_RESULT_FAILURE + 0x34;
        public const UInt32 E_PID002A_NO_SYSTEM_TROUBLES                = E_PID002A_RESULT_FAILURE + 0x35;

        public const UInt32 S_IP100_RESULT_SUCCESS                      = 0x03000000;
        public const UInt32 E_IP100_RESULT_FAILURE                      = 0x83000000;

        public const UInt32 E_IP100_BAD_RESULT                          = E_IP100_RESULT_FAILURE + 0x01;
        public const UInt32 E_IP100_WEB_PAGE_IN_USE                     = E_IP100_RESULT_FAILURE + 0x02;
        public const UInt32 E_IP100_BAD_USERNAME                        = E_IP100_RESULT_FAILURE + 0x03;
        public const UInt32 E_IP100_BAD_PASSWORD                        = E_IP100_RESULT_FAILURE + 0x04;
        public const UInt32 E_IP100_NEWARE_IN_USE                       = E_IP100_RESULT_FAILURE + 0x05;
        public const UInt32 E_IP100_COMMAND_TIMEOUT                     = E_IP100_RESULT_FAILURE + 0x06;
        public const UInt32 E_IP100_PACKET_FORMAT                       = E_IP100_RESULT_FAILURE + 0x07;
        public const UInt32 E_IP100_PACKET_LENGTH                       = E_IP100_RESULT_FAILURE + 0x08;
        public const UInt32 E_IP100_PACKET_STARTBYTE                    = E_IP100_RESULT_FAILURE + 0x09;
        public const UInt32 E_IP100_PACKET_DESTINATION                  = E_IP100_RESULT_FAILURE + 0x0A;
        public const UInt32 E_IP100_PACKET_WRONG_COMMAND                = E_IP100_RESULT_FAILURE + 0x0B;
        public const UInt32 E_IP100_USE_CONNECT_EX                      = E_IP100_RESULT_FAILURE + 0x0C;
        public const UInt32 E_IP100_DATA_LENGTH                         = E_IP100_RESULT_FAILURE + 0x0D;
        public const UInt32 E_IP100_ACQUIRE_REFUSED                     = E_IP100_RESULT_FAILURE + 0x0E;
        public const UInt32 E_IP100_IPDUMP_ALREADY                      = E_IP100_RESULT_FAILURE + 0x10;
        public const UInt32 E_IP100_IPDUMP_TOO_MANY                     = E_IP100_RESULT_FAILURE + 0x11;
        public const UInt32 E_IP100_IPDUMP_NO_RIGHTS                    = E_IP100_RESULT_FAILURE + 0x12;
        public const UInt32 E_IP100_IPDUMP_INVALID_TYPE                 = E_IP100_RESULT_FAILURE + 0x13;
        public const UInt32 E_IP100_IPDUMP_CANNOT_SOCK                  = E_IP100_RESULT_FAILURE + 0x14;
        public const UInt32 E_IP100_USUPPORTED_ENCRYPTION               = E_IP100_RESULT_FAILURE + 0x15;

        public const UInt32 E_PID0004_ERROR                             = E_PN_RESULT_PID_BASE + 0x00000400;
        public const UInt32 S_PID0004_RESULT                            = S_PN_RESULT_PID_BASE + 0x00000400;

        public const UInt32 E_PID0004_ERROR_CHECKSUM                    = E_PID0004_ERROR + 0x01;
        public const UInt32 E_PID0004_ERROR_PROCESSCMD                  = E_PID0004_ERROR + 0x02;
        public const UInt32 E_PID0004_ERROR_AUTHFAIL_PCPSWD             = E_PID0004_ERROR + 0x03;
        public const UInt32 E_PID0004_ERROR_AUTHFAIL_PNLID              = E_PID0004_ERROR + 0x04;
        public const UInt32 E_PID0004_ERROR_AUTHFAIL_USRID              = E_PID0004_ERROR + 0x05;
        public const UInt32 E_PID0004_ERROR_AUTHFAIL_PNLREPORTING       = E_PID0004_ERROR + 0x06;
        public const UInt32 E_PID0004_ERROR_AUTHFAIL_FUTUREUSE          = E_PID0004_ERROR + 0x07;
        public const UInt32 E_PID0004_ERROR_WRITEPROTECTED              = E_PID0004_ERROR + 0x08;
        public const UInt32 E_PID0004_ERROR_WRITEEXCEEDEDBLOCKSIZE      = E_PID0004_ERROR + 0x09;
        public const UInt32 E_PID0004_ERROR_PARTITIONINLOCKOUT          = E_PID0004_ERROR + 0x0A;
        public const UInt32 E_PID0004_ERROR_PANELWILLDISCONNECT         = E_PID0004_ERROR + 0x0B;
        public const UInt32 E_PID0004_ERROR_PANELNOTCONNECTED           = E_PID0004_ERROR + 0x0C;
        public const UInt32 E_PID0004_ERROR_PANELALREADYCONNECTED       = E_PID0004_ERROR + 0x0D;
        public const UInt32 E_PID0004_ERROR_SOFTWAREOVERMODEM           = E_PID0004_ERROR + 0x0E;
        public const UInt32 E_PID0004_ERROR_INVALIDBUSADDRESS           = E_PID0004_ERROR + 0x0F;
        public const UInt32 E_PID0004_ERROR_CANNOTWRITETORAM            = E_PID0004_ERROR + 0x10;
        public const UInt32 E_PID0004_ERROR_UPDATEFAILED                = E_PID0004_ERROR + 0x11;
        public const UInt32 E_PID0004_ERROR_RECORDOUTOFRANGE            = E_PID0004_ERROR + 0x12;
        public const UInt32 E_PID0004_ERROR_HASTATUSNOTSUPPORTED        = E_PID0004_ERROR + 0x13;
        public const UInt32 E_PID0004_ERROR_INVALIDRECORDTYPE           = E_PID0004_ERROR + 0x14;
        public const UInt32 E_PID0004_ERROR_MULTIBUSNOTSUPPORTED        = E_PID0004_ERROR + 0x15;
        public const UInt32 E_PID0004_ERROR_INVALIDUSERCOUNT            = E_PID0004_ERROR + 0x16;
        public const UInt32 E_PID0004_ERROR_INVALIDLABELID              = E_PID0004_ERROR + 0x17;
        public const UInt32 E_PID0004_ERROR_INVALIDACTION               = E_PID0004_ERROR + 0x18;
        public const UInt32 E_PID0004_ERROR_INVALIDRECEIVERID           = E_PID0004_ERROR + 0x19;
        public const UInt32 E_PID0004_ERROR_PANELNOTSUPPORTED           = E_PID0004_ERROR + 0x20;
        public const UInt32 E_PID0004_ERROR_INVALIDUSERCODELENGTH       = E_PID0004_ERROR + 0x21;


        public static Boolean Succeeded(UInt32 Status)
        {
            return (Status & 0x80000000) == 0;
        }

        public static Boolean Failed(UInt32 Status)
        {
            return (Status & 0x80000000) != 0;
        }

        public static String GetResultCode(UInt32 returnValue)
        {
            if (Succeeded(returnValue))
            {
                return "SUCCESS";
            }
            else
            {
                switch (returnValue)
                {
                    case E_FAIL: return "E_FAIL";
                    case E_PID002A_INVALID_COMMAND: return "E_PID002A_INVALID_COMMAND";
                    case E_PID002A_INVALID_SESSION: return "E_PID002A_INVALID_SESSION";
                    case E_PID002A_ALREADY_CONNECTED: return "E_PID002A_ALREADY_CONNECTED";
                    case E_PID002A_FAILED_TO_CREATE_KEY: return "E_PID002A_FAILED_TO_CREATE_KEY";
                    case E_PID002A_FAILED_TO_CRYPT_PACKET: return "E_PID002A_FAILED_TO_CRYPT_PACKET";

                    case E_PID002A_INVALID_PANEL_ID: return "E_PID002A_INVALID_PANEL_ID";
                    case E_PID002A_INVALID_PANEL_NOT_CONNECTED: return "E_PID002A_INVALID_PANEL_NOT_CONNECTED";
                    case E_PID002A_INVALID_COMMAND_NOT_SUPPORTED: return "E_PID002A_INVALID_COMMAND_NOT_SUPPORTED";
                    case E_PID002A_SESSION_TIMEOUT: return "E_PID002A_SESSION_TIMEOUT";
                    case E_PID002A_INVALID_DATETIME_FORMAT: return "E_PID002A_INVALID_DATETIME_FORMAT";

                    case E_PID002A_SMS_CALLBACK_MODEM_FAILURE: return "E_PID002A_SMS_CALLBACK_MODEM_FAILURE";
                    case E_PID002A_SMS_CALLBACK_CONNECTION_FAILURE: return "E_PID002A_SMS_CALLBACK_CONNECTION_FAILURE";

                    case E_PID002A_INVALID_EVENT_NOT_FOUND: return "E_PID002A_INVALID_EVENT_NOT_FOUND";
                    case E_PID002A_INVALID_PANEL_USER_FULL: return "E_PID002A_INVALID_PANEL_USER_FULL";
                    case E_PID002A_INVALID_PANEL_USER_ID: return "E_PID002A_INVALID_PANEL_USER_ID";
                    case E_PID002A_INVALID_PANEL_USER_UNASSIGNED: return "E_PID002A_INVALID_PANEL_USER_UNASSIGNED";

                    case E_PID002A_INVALID_XML_FORMAT: return "E_PID002A_INVALID_XML_FORMAT";

                    case E_PID002A_INVALID_COM_PORT: return "E_PID002A_INVALID_COM_PORT";
                    case E_PID002A_INVALID_BAUD_RATE: return "E_PID002A_INVALID_BAUD_RATE";
                    case E_PID002A_INVALID_IP_ADDRESS: return "E_PID002A_INVALID_IP_ADDRESS";
                    case E_PID002A_INVALID_IP_PORT: return "E_PID002A_INVALID_IP_PORT";
                    case E_PID002A_INVALID_SITE_ID: return "E_PID002A_INVALID_SITE_ID";
                    case E_PID002A_INVALID_IP_PASSWORD: return "E_PID002A_INVALID_IP_PASSWORD";
                    case E_PID002A_INVALID_COM_TYPE: return "E_PID002A_INVALID_COM_TYPE";
                    case E_PID002A_INVALID_USERCODE: return "E_PID002A_INVALID_USERCODE";

                    case E_PID002A_INVALID_USER_RIGHTS: return "E_PID002A_INVALID_USER_RIGHTS";

                    case E_PID002A_COMMAND_TIMEOUT: return "E_PID002A_COMMAND_TIMEOUT";
                    case E_PID002A_PANEL_NOT_FOUND: return "E_PID002A_PANEL_NOT_FOUND";
                    case E_PID002A_TASK_CANCELLED: return "E_PID002A_TASK_CANCELLED";
                    case E_PID002A_NO_SYSTEM_TROUBLES: return "E_PID002A_NO_SYSTEM_TROUBLES";

                    case E_IP100_BAD_RESULT: return "E_IP100_BAD_RESULT";
                    case E_IP100_WEB_PAGE_IN_USE: return "E_IP100_WEB_PAGE_IN_USE";
                    case E_IP100_BAD_USERNAME: return "E_IP100_BAD_USERNAME";
                    case E_IP100_BAD_PASSWORD: return "E_IP100_BAD_PASSWORD";
                    case E_IP100_NEWARE_IN_USE: return "E_IP100_NEWARE_IN_USE";
                    case E_IP100_COMMAND_TIMEOUT: return "E_IP100_COMMAND_TIMEOUT";
                    case E_IP100_PACKET_FORMAT: return "E_IP100_PACKET_FORMAT";
                    case E_IP100_PACKET_LENGTH: return "E_IP100_PACKET_LENGTH";
                    case E_IP100_PACKET_STARTBYTE: return "E_IP100_PACKET_STARTBYTE";
                    case E_IP100_PACKET_DESTINATION: return "E_IP100_PACKET_DESTINATION";
                    case E_IP100_PACKET_WRONG_COMMAND: return "E_IP100_PACKET_WRONG_COMMAND";
                    case E_IP100_USE_CONNECT_EX: return "E_IP100_USE_CONNECT_EX";
                    case E_IP100_DATA_LENGTH: return "E_IP100_DATA_LENGTH";
                    case E_IP100_ACQUIRE_REFUSED: return "E_IP100_ACQUIRE_REFUSED";
                    case E_IP100_IPDUMP_ALREADY: return "E_IP100_IPDUMP_ALREADY";
                    case E_IP100_IPDUMP_TOO_MANY: return "E_IP100_IPDUMP_TOO_MANY";
                    case E_IP100_IPDUMP_NO_RIGHTS: return "E_IP100_IPDUMP_NO_RIGHTS";
                    case E_IP100_IPDUMP_INVALID_TYPE: return "E_IP100_IPDUMP_INVALID_TYPE";
                    case E_IP100_IPDUMP_CANNOT_SOCK: return "E_IP100_IPDUMP_CANNOT_SOCK";
                    case E_IP100_USUPPORTED_ENCRYPTION: return "E_IP100_USUPPORTED_ENCRYPTION";

                    case E_PID0004_ERROR_CHECKSUM: return "E_PID0004_ERROR_CHECKSUM";
                    case E_PID0004_ERROR_PROCESSCMD: return "E_PID0004_ERROR_PROCESSCMD";
                    case E_PID0004_ERROR_AUTHFAIL_PCPSWD: return "E_PID0004_ERROR_AUTHFAIL_PCPSWD";
                    case E_PID0004_ERROR_AUTHFAIL_PNLID: return "E_PID0004_ERROR_AUTHFAIL_PNLID";
                    case E_PID0004_ERROR_AUTHFAIL_USRID: return "E_PID0004_ERROR_AUTHFAIL_USRID";
                    case E_PID0004_ERROR_AUTHFAIL_PNLREPORTING: return "E_PID0004_ERROR_AUTHFAIL_PNLREPORTING";
                    case E_PID0004_ERROR_AUTHFAIL_FUTUREUSE: return "E_PID0004_ERROR_AUTHFAIL_FUTUREUSE";
                    case E_PID0004_ERROR_WRITEPROTECTED: return "E_PID0004_ERROR_WRITEPROTECTED";
                    case E_PID0004_ERROR_WRITEEXCEEDEDBLOCKSIZE: return "E_PID0004_ERROR_WRITEEXCEEDEDBLOCKSIZE";
                    case E_PID0004_ERROR_PARTITIONINLOCKOUT: return "E_PID0004_ERROR_PARTITIONINLOCKOUT";
                    case E_PID0004_ERROR_PANELWILLDISCONNECT: return "E_PID0004_ERROR_PANELWILLDISCONNECT";
                    case E_PID0004_ERROR_PANELNOTCONNECTED: return "E_PID0004_ERROR_PANELNOTCONNECTED";
                    case E_PID0004_ERROR_PANELALREADYCONNECTED: return "E_PID0004_ERROR_PANELALREADYCONNECTED";
                    case E_PID0004_ERROR_SOFTWAREOVERMODEM: return "E_PID0004_ERROR_SOFTWAREOVERMODEM";
                    case E_PID0004_ERROR_INVALIDBUSADDRESS: return "E_PID0004_ERROR_INVALIDBUSADDRESS";
                    case E_PID0004_ERROR_CANNOTWRITETORAM: return "E_PID0004_ERROR_CANNOTWRITETORAM";
                    case E_PID0004_ERROR_UPDATEFAILED: return "E_PID0004_ERROR_UPDATEFAILED";
                    case E_PID0004_ERROR_RECORDOUTOFRANGE: return "E_PID0004_ERROR_RECORDOUTOFRANGE";
                    case E_PID0004_ERROR_HASTATUSNOTSUPPORTED: return "E_PID0004_ERROR_HASTATUSNOTSUPPORTED";
                    case E_PID0004_ERROR_INVALIDRECORDTYPE: return "E_PID0004_ERROR_INVALIDRECORDTYPE";
                    case E_PID0004_ERROR_MULTIBUSNOTSUPPORTED: return "E_PID0004_ERROR_MULTIBUSNOTSUPPORTED";
                    case E_PID0004_ERROR_INVALIDUSERCOUNT: return "E_PID0004_ERROR_INVALIDUSERCOUNT";
                    case E_PID0004_ERROR_INVALIDLABELID: return "E_PID0004_ERROR_INVALIDLABELID";
                    case E_PID0004_ERROR_INVALIDACTION: return "E_PID0004_ERROR_INVALIDACTION";
                    case E_PID0004_ERROR_INVALIDRECEIVERID: return "E_PID0004_ERROR_INVALIDRECEIVERID";
                    case E_PID0004_ERROR_PANELNOTSUPPORTED: return "E_PID0004_ERROR_PANELNOTSUPPORTED";
                    case E_PID0004_ERROR_INVALIDUSERCODELENGTH: return "E_PID0004_ERROR_INVALIDUSERCODELENGTH";
                                                                        
                    default: return "FAILURE";                         
                }                                                       
            }
        }

        // MG/SP Control Panel
        public const String CP_PROD_NAME_SP4000 = "SP4000";
        public const String CP_PROD_NAME_SP5500 = "SP5500";
        public const String CP_PROD_NAME_SP6000 = "SP6000";
        public const String CP_PROD_NAME_SP7000 = "SP7000";        
        public const String CP_PROD_NAME_MG5000 = "MG5000";
        public const String CP_PROD_NAME_MG5050 = "MG5050";

        // EVO Control Panel
        public const String CP_PROD_NAME_DGP_EVO_192 = "EVO192";
        public const String CP_PROD_NAME_DGP_EVO_VHD = "EVOHD";
        
        // System Alarm Language

        public const String SYS_ALARM_LANGUAGE_ENGLISH = "ENGLISH";
        public const String SYS_ALARM_LANGUAGE_FRENCH = "FRENCH";
        public const String SYS_ALARM_LANGUAGE_SPANISH = "SPANISH";
        public const String SYS_ALARM_LANGUAGE_ITALIAN = "ITALIAN";
        public const String SYS_ALARM_LANGUAGE_SWEDISH = "SWEDISH";
        public const String SYS_ALARM_LANGUAGE_POLISH = "POLISH";
        public const String SYS_ALARM_LANGUAGE_PORTUGUESE = "PORTUGUESE";
        public const String SYS_ALARM_LANGUAGE_GERMAN = "GERMAN";
        public const String SYS_ALARM_LANGUAGE_TURKISH = "TURKISH";
        public const String SYS_ALARM_LANGUAGE_HUNGARIAN = "HUNGARIAN";
        public const String SYS_ALARM_LANGUAGE_CZECH = "CZECH";
        public const String SYS_ALARM_LANGUAGE_DUTCH = "DUTCH";
        public const String SYS_ALARM_LANGUAGE_CROATIAN = "CROATIAN";
        public const String SYS_ALARM_LANGUAGE_GREEK = "GREEK";
        public const String SYS_ALARM_LANGUAGE_HEBREW = "HEBREW";
        public const String SYS_ALARM_LANGUAGE_RUSSIAN = "RUSSIAN";
        public const String SYS_ALARM_LANGUAGE_BULGARIAN = "BULGARIAN";
        public const String SYS_ALARM_LANGUAGE_ROMANIAN = "ROMANIAN";
        public const String SYS_ALARM_LANGUAGE_SLOVAK = "SLOVAK";
        public const String SYS_ALARM_LANGUAGE_CHINESE = "CHINESE";
        public const String SYS_ALARM_LANGUAGE_SERBIAN = "SERBIAN";
        public const String SYS_ALARM_LANGUAGE_MALAY = "MALAY";
        public const String SYS_ALARM_LANGUAGE_SLOVENIAN = "SLOVENIAN";
        public const String SYS_ALARM_LANGUAGE_LITHUANIAN = "LITHUANIAN";
        public const String SYS_ALARM_LANGUAGE_FINNISH = "FINNISH";
        public const String SYS_ALARM_LANGUAGE_ESTONIAN = "ESTONIAN";        
        public const String SYS_ALARM_LANGUAGE_BELGIAN = "BELGIAN";
        public const String SYS_ALARM_LANGUAGE_LATVIAN = "LATVIAN";
        public const String SYS_ALARM_LANGUAGE_MACEDONIA = "MACEDONIA";
        public const String SYS_ALARM_LANGUAGE_ALBANIAN = "ALBANIAN";
        public const String SYS_ALARM_LANGUAGE_NORWEGIAN = "NORWEGIAN";
        public const String SYS_ALARM_LANGUAGE_ARABIC_PERSAIN = "ARABICPERSAIN";
        public const String SYS_ALARM_LANGUAGE_ARABIC_WESTERN = "ARABICWESTERN";
        public const String SYS_ALARM_LANGUAGE_ARABIC_EASTERN = "ARABICEASTERN";
    
        
        // Object Type
        public const String OT_AREA = "Area";
        public const String OT_AREAS = "Areas";
        public const String OT_ZONE = "Zone";
        public const String OT_ZONES = "Zones";
        public const String OT_DOOR = "Door";
        public const String OT_DOORS = "Doors";        
        public const String OT_PGM = "PGM";
        public const String OT_PGMS = "PGMs"; 
        public const String OT_USER = "User";
        public const String OT_USERS = "Users";
        public const String OT_IP_RECEIVER = "IPReceiver";
        public const String OT_PANEL_INFO_EX = "PanelInfoEx";
        public const String OT_SCHEDULE = "Schedule";
        public const String OT_SCHEDULES = "Schedules";
        public const String OT_HOLIDAYS = "Holidays";
        public const String OT_ACCESS_LEVEL = "Access Level";
        public const String OT_ACCESS_LEVELS = "Access Levels";

        // Action Type
        public const String AT_READ = "Read";
        public const String AT_WRITE = "Write";


        // Control area
        public const String C_CONTROL_AREA_ARM       = "Arm";
        public const String C_CONTROL_AREA_FORCE     = "Force";
        public const String C_CONTROL_AREA_STAY      = "Stay";
        public const String C_CONTROL_AREA_SLEEP     = "Sleep";
        public const String C_CONTROL_AREA_INSTANT   = "Instant";
        public const String C_CONTROL_AREA_DISARM    = "Disarm";

        // Control Zone
        public const String C_CONTROL_ZONE_BYPASS    = "Bypass";
        public const String C_CONTROL_ZONE_UNBYPASS  = "Unbypass";

        // Control PGM
        public const String C_CONTROL_PGM_ON         = "On";
        public const String C_CONTROL_PGM_OFF        = "Off";
        public const String C_CONTROL_PGM_TEST       = "Test";

        // Control Door
        public const String C_CONTROL_DOOR_LOCK      = "Lock";
        public const String C_CONTROL_DOOR_UNLOCK    = "Unlock";


        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "GetDriverVersion")]
        public static extern Int32 GetDriverVersion(
                                                [MarshalAs(UnmanagedType.BStr)] out String version
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "DiscoverModule")]
        public static extern Int32 DiscoverModule(
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlInfo
                                                );


        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "DetectPanel")]
        public static extern Int32 DetectPanel(
                                                [MarshalAs(UnmanagedType.BStr)] String xmlSettings,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlInfo
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ConnectPanel")]
        public static extern Int32 ConnectPanel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlSettings,
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
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlInfo
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "RegisterPanel")]
        public static extern Int32 RegisterPanel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlAction
                                                );


        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ControlArea")]
        public static extern Int32 ControlArea(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlArea
                                                );


        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "AreaStatus")]
        public static extern Int32 AreaStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ControlZone")]
        public static extern Int32 ControlZone(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlZone
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ZoneStatus")]
        public static extern Int32 ZoneStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ControlPGM")]
        public static extern Int32 ControlPGM(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlPGM
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "PGMStatus")]
        public static extern Int32 PGMStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 PanelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlStatus
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ControlDoor")]
        public static extern Int32 ControlDoor(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlDoor
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "DoorStatus")]
        public static extern Int32 DoorStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlStatus
                                                );


        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadTimeStamp")]
        public static extern Int32 ReadTimeStamp(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 blockID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlTimeStamp
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
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlArea
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllAreas")]
        public static extern Int32 ReadAllAreas(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlAreas
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadZone")]
        public static extern Int32 ReadZone(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 zoneNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlZone
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllZones")]
        public static extern Int32 ReadAllZones(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlZones
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadPGM")]
        public static extern Int32 ReadPGM(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 pgmNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlPGM
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllPGMs")]
        public static extern Int32 ReadAllPGMs(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlPGMs
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadDoor")]
        public static extern Int32 ReadDoor(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 doorNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlDoor
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteDoor")]
        public static extern Int32 WriteDoor(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 doorNo,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlDoor
                                                );

        [DllImport("ParadoxAPI.dll",
          CallingConvention = CallingConvention.StdCall,
          CharSet = CharSet.Unicode,
          EntryPoint = "ReadAllDoors")]
        public static extern Int32 ReadAllDoors(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlDoors
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadUser")]
        public static extern Int32 ReadUser(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 userNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlUser
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteUser")]
        public static extern Int32 WriteUser(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 userNo,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlUser
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAllUsers")]
        public static extern Int32 ReadAllUsers(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlUsers
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteMultipleUsers")]
        public static extern Int32 WriteMultipleUsers(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlUsers
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadSchedule")]
        public static extern Int32 ReadSchedule(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 scheduleNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlSchedule
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteSchedule")]
        public static extern Int32 WriteSchedule(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 scheduleNo,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlSchedule
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAllSchedules")]
        public static extern Int32 ReadAllSchedules(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String XMLSchedules
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadHolidays")]
        public static extern Int32 ReadHolidays(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlHolidays
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteHolidays")]
        public static extern Int32 WriteHolidays(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlHolidays
                                                );
         
        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAccessLevel")]
        public static extern Int32 ReadAccessLevel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 accessLevelNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlAccessLevel
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteAccessLevel")]
        public static extern Int32 WriteAccessLevel(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 accessLevelNo,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlAccessLevel
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadAllAccessLevels")]
        public static extern Int32 ReadAllAccessLevels(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlAccessLevels
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "ReadIPReporting")]
        public static extern Int32 ReadIPReporting(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 receiverNo,            
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlReporting
                                                );
        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "WriteIPReporting")]
        public static extern Int32 WriteIPReporting(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 receiverNo,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlReporting
                                                );

        [DllImport("ParadoxAPI.dll",
         CallingConvention = CallingConvention.StdCall,
         CharSet = CharSet.Unicode,
         EntryPoint = "IPReportingStatus")]
        public static extern Int32 IPReportingStatus(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlStatus
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
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlMonitoring
                                                );

        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "SystemTroubles")]
        public static extern Int32 SystemTroubles(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlTroubles
                                                );

        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "StartIPDOX")]
        public static extern Int32 startIPDOX([MarshalAs(UnmanagedType.BStr)] String xmlSetting);

        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "StopIPDOX")]
        public static extern Int32 stopIPDOX();
        
        [DllImport("ParadoxAPI.dll",
       CallingConvention = CallingConvention.StdCall,
       CharSet = CharSet.Unicode,
       EntryPoint = "DeleteIPDOXAccount")]
        public static extern Int32 deleteIPDOXAccount([MarshalAs(UnmanagedType.BStr)] String macAddress);
        
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
                                                [MarshalAs(UnmanagedType.BStr)] String panelSerialNo,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlSiteInfo
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "ConfigureVideoServer")]
        public static extern Int32 ConfigureVideoServer(
                                                [MarshalAs(UnmanagedType.BStr)] String xmlVideoSettings                                                
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "GetVideoAlarmFiles")]
        public static extern Int32 GetVideoAlarmFiles(
                                                [MarshalAs(UnmanagedType.BStr)] String accountNo,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 zoneNo,
                                                [MarshalAs(UnmanagedType.R8)] Double dateTime,
                                                [MarshalAs(UnmanagedType.BStr)] out String XMLVideoFiles
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "StartVideoOnDemand")]
        public static extern Int32 StartVideoOnDemand(
                                                [MarshalAs(UnmanagedType.BStr)] String ipAddress,
                                                [MarshalAs(UnmanagedType.U4)] UInt32 ipPort,
                                                [MarshalAs(UnmanagedType.BStr)] String SessionKey,
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlVideoFile
                                                );

        [DllImport("ParadoxAPI.dll",
           CallingConvention = CallingConvention.StdCall,
           CharSet = CharSet.Unicode,
           EntryPoint = "StartVideoOnDemandEx")]
        public static extern Int32 StartVideoOnDemandEx(
                                                [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                [MarshalAs(UnmanagedType.BStr)] String xmlVODSettings,                                                
                                                [MarshalAs(UnmanagedType.BStr)] out String xmlVideoFile
                                                );
                                                                     
        /// <summary>
        /// Callbacks RegisterConnectionStatusChangedCallback 
        /// </summary>
        private static ProcConnectionStatusChangedDelegate procConnectionStatusChangedDelegate;

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ProcConnectionStatusChangedDelegate([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr,
                                                                 [MarshalAs(UnmanagedType.U4)] UInt32 panelID,
                                                                 [MarshalAs(UnmanagedType.BStr)] String status
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
                                                         [MarshalAs(UnmanagedType.BStr)] String description,
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
                                                         [MarshalAs(UnmanagedType.BStr)] String errorMsg
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
                                                            [MarshalAs(UnmanagedType.BStr)] String xmlEvents
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
                                                            [MarshalAs(UnmanagedType.BStr)] String xmlEvents
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
                                                          [MarshalAs(UnmanagedType.BStr)] String xmlEvents
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
                                                                 [MarshalAs(UnmanagedType.BStr)] String xmlStatus
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
                                                          [MarshalAs(UnmanagedType.BStr)] String xmlModule
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
                                                    [MarshalAs(UnmanagedType.BStr)] String sms
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
                                                             [MarshalAs(UnmanagedType.BStr)] String xmlAccounts
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
                                                       [MarshalAs(UnmanagedType.BStr)] String xmlAccounts
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
                                                     [MarshalAs(UnmanagedType.BStr)] String xmlAccounts
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
                                                     [MarshalAs(UnmanagedType.BStr)] String description
                                                    );        
        [DllImport("ParadoxAPI.dll",
        CallingConvention = CallingConvention.StdCall,
        CharSet = CharSet.Unicode,
        EntryPoint = "RegisterIPDOXSocketChangedCallback")]
        public static extern void RegisterIPDOXSocketChangedCallback([MarshalAs(UnmanagedType.FunctionPtr)] ProcIPDOXSocketChangedDelegate callBackHandle);
        

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
        public static void MonitoringStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] String xmlStatus)
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
        public static void ReceiveLiveEventCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] String xmlEvents)
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
        public static void ReceiveBufferEventCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] String xmlEvents)
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
        public static void ReceiveReportingEventCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.BStr)] String xmlEvents)
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
                              
        public delegate void ProgressChangedDelegate(UInt32 panelID, UInt32 task, String description, UInt32 percent);

        public static ProgressChangedDelegate progressChangedDelegate = null;

        // Callback
        public static void ProgressChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 task, [MarshalAs(UnmanagedType.BStr)] String description, [MarshalAs(UnmanagedType.U4)] UInt32 percent)
        {
            if (progressChangedDelegate != null)
            {
                progressChangedDelegate(panelID, task, description, percent);
            }
        }


        public delegate void ProgressErrorDelegate(UInt32 panelID, UInt32 task, UInt32 errorCode, String errorMsg);

        public static ProgressErrorDelegate progressErrorDelegate = null;

        // Callback
        public static void ProgressErrorCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.U4)] UInt32 task, [MarshalAs(UnmanagedType.U4)] UInt32 errorCode, [MarshalAs(UnmanagedType.BStr)] String errorMsg)
        {
            if (progressErrorDelegate != null)
            {
                progressErrorDelegate(panelID, task, errorCode, errorMsg);
            }
        }

        public delegate void ConnectionStatusChangedDelegate(UInt32 panelID, String status);

        public static ConnectionStatusChangedDelegate connectionStatusChangedDelegate = null;

        // Callback
        public static void ConnectionStatusChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)]UInt32 ptr, [MarshalAs(UnmanagedType.U4)] UInt32 panelID, [MarshalAs(UnmanagedType.BStr)] String status)
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
        public static void IPModuleDetectedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, String xmlModule)
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

        public delegate void SMSRequestDelegate(UInt32 panelID, String sms);

        public static SMSRequestDelegate smsRequestDelegate = null;

        // Callback
        public static void SMSRequestCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 panelID, String sms)
        {
            if (smsRequestDelegate != null)
            {
                smsRequestDelegate(panelID, sms);
            }
        }


        public delegate void AccountRegistrationDelegate(PanelReportingAccount panelReportingAccount);

        public static AccountRegistrationDelegate accountRegistrationDelegate = null;

        // Callback
        public static void AccountRegistrationCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, String xmlAccounts)
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
        public static void AccountUpdateCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, String xmlAccounts)
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
        public static void AccountLinkCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, String xmlAccounts)
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

        public delegate void IPDOXSocketChangedDelegate(UInt32 port, UInt32 status, String description);

        public static IPDOXSocketChangedDelegate ipDOXSocketChangedDelegate = null;

        // Callback
        public static void IPDOXSocketChangedCalledFromParadoxAPI([MarshalAs(UnmanagedType.U4)] UInt32 fncPtr, UInt32 port, UInt32 status, String description)
        {
            if (ipDOXSocketChangedDelegate != null)
            {
                ipDOXSocketChangedDelegate(port, status, description);                
            }
        }
              
        public delegate void LogDelegate(UInt32 panelID, Int32 returnValue, String logs);

        public static LogDelegate logDelegate = null;

        private static void Log(UInt32 panelID, Int32 returnValue, String value)
        {            
            if ((formRef != null) && (logDelegate != null))
            {
                formRef.Invoke(logDelegate, new object[] {panelID, returnValue, value});                
            }
        }

        public delegate void NotifyTaskCompletedDelegate(UInt32 panelID, Int32 returnValue, UInt32 ItemNo, String ItemType, String ActionType, object obj = null);

        public static NotifyTaskCompletedDelegate notifyTaskCompletedDelegate = null;

        private static void NotifyTaskCompleted(UInt32 panelID, Int32 returnValue, UInt32 ItemNo, String ItemType, String ActionType, object obj = null)
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

        public delegate void ErrorNotificationDelegate(UInt32 panelID, Int32 returnValue, String ErrorMsg);

        public static ErrorNotificationDelegate errorNotificationDelegate = null;

        private static void errorNotification(UInt32 panelID, Int32 returnValue, String ErrorMsg)
        {
            if ((formRef != null) && (errorNotificationDelegate != null))
            {
                formRef.Invoke(errorNotificationDelegate, new object[] { panelID, returnValue, ErrorMsg });
            }
        }

        public static Int32 GetAPIVersion(ref String version)
        {
            Int32 returnValue = GetDriverVersion(out version);                      

            if (!Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "GetDriverVersion: " + GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }
                
        public static Int32 DiscoverModules(ModuleInfoList moduleInfoList)
        {
            if (moduleInfoList != null)
            {
                String xmlInfo = "";

                Int32 returnValue = DiscoverModule(out xmlInfo);

                moduleInfoList.parseXML(xmlInfo);

                Log(0, returnValue, xmlInfo);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "DiscoverModules: " + GetResultCode((UInt32)returnValue));
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
                String xmlSettings = "";

                panelSettings.serializeXML(ref xmlSettings);

                Int32 returnValue = ConnectPanel(panelID, xmlSettings, WaitTimeOut);

                Log(panelID, returnValue, xmlSettings);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ConnectToPanel: " + GetResultCode((UInt32)returnValue));
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

            if (!Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "DisconnectFromPanel: " + GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static Int32 DetectPanel(UInt32 panelID, PanelSettings panelSettings, PanelInfo panelInfo)
        {
            if ((panelSettings != null) && (panelInfo != null)) 
            {
                String xmlSettings = "";
                String xmlInfo = "";
                
                panelSettings.serializeXML(ref xmlSettings);

                Int32 returnValue = DetectPanel(xmlSettings, out xmlInfo);

                panelInfo.parseXML(xmlInfo);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "DetectPanel: " + GetResultCode((UInt32)returnValue));
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
                String xmlInfo = "";

                Int32 returnValue = RetrievePanelInfo(panelID, out xmlInfo);
                
                panelInfoEx.parseXML(xmlInfo);

                Log(panelID, returnValue, xmlInfo);
                
                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "RetrievePanelInfo: " + GetResultCode((UInt32)returnValue));
                }
                else
                {
                    NotifyTaskCompleted(panelID, returnValue, 0, OT_PANEL_INFO_EX, AT_READ, panelInfoEx);
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
                String xmlAction = "";

                panelControl.serializeXML(ref xmlAction);
                
                Int32 returnValue = RegisterPanel(panelID, xmlAction);

                Log(panelID, returnValue, xmlAction);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "RegisterPanel: " + GetResultCode((UInt32)returnValue));
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
                String xmlArea = "";

                panelControl.serializeXML(ref xmlArea);                

                Int32 returnValue = ControlArea(panelID, xmlArea);

                Log(panelID, returnValue, xmlArea);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlArea: " + GetResultCode((UInt32)returnValue));
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
                String xmlStatus = "";

                Int32 returnValue = AreaStatus(panelID, out xmlStatus);

                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "AreaStatus: " + GetResultCode((UInt32)returnValue));
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
                String xmlZone = "";

                panelControl.serializeXML(ref xmlZone);                

                Int32 returnValue = ControlZone(panelID, xmlZone);

                Log(panelID, returnValue, xmlZone);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlZone: " + GetResultCode((UInt32)returnValue));
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
                String xmlStatus = "";

                Int32 returnValue = ZoneStatus(panelID, out xmlStatus);
                
                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ZoneStatus: " + GetResultCode((UInt32)returnValue));
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
                String xmlPGM = "";

                panelControl.serializeXML(ref xmlPGM);               

                Int32 returnValue = ControlPGM(panelID, xmlPGM);

                Log(panelID, returnValue, xmlPGM);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlPGM: " + GetResultCode((UInt32)returnValue));
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
                String xmlStatus = "";

                Int32 returnValue = PGMStatus(panelID, out xmlStatus);
              
                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "PGMStatus: " + GetResultCode((UInt32)returnValue));
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
                String xmlDoor = "";

                panelControl.serializeXML(ref xmlDoor);               

                Int32 returnValue = ControlDoor(panelID, xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ControlDoor: " + GetResultCode((UInt32)returnValue));
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
                String xmlStatus = "";

                Int32 returnValue = DoorStatus(panelID, out xmlStatus);
                
                panelMonitoring.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "DoorStatus: " + GetResultCode((UInt32)returnValue));
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
                String xmlTimeStamp = "";

                Int32 returnValue = ReadTimeStamp(panelID, blockID, out xmlTimeStamp);                

                panelTimeStamp.parseXML(xmlTimeStamp);

                Log(panelID, returnValue, xmlTimeStamp);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadTimeStamp: " + GetResultCode((UInt32)returnValue));
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

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadDateTime: " + GetResultCode((UInt32)returnValue));
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

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteDateTime: " + GetResultCode((UInt32)returnValue));
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
                String xmlArea = "";

                Int32 returnValue = ReadArea(panelID, areaNo, out xmlArea);
               
                panelArea.parseXML(xmlArea);

                Log(panelID, returnValue, xmlArea);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadArea: " + GetResultCode((UInt32)returnValue));
                }
                
                NotifyTaskCompleted(panelID, returnValue, areaNo, OT_AREA, AT_READ);
                                
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
                String xmlAreas = "";

                Int32 returnValue = ReadAllAreas(panelID, out xmlAreas);

                panelAreas.parseXML(xmlAreas);

                Log(panelID, returnValue, xmlAreas);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllAreas: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_AREAS, AT_READ);

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
                String xmlZone = "";

                Int32 returnValue = ReadZone(panelID, zoneNo, out xmlZone);
                                
                panelZone.parseXML(xmlZone);

                Log(panelID, returnValue, xmlZone);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadZone: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, zoneNo, OT_ZONE, AT_READ);

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
                String xmlZones = "";

                Int32 returnValue = ReadAllZones(panelID, out xmlZones);

                panelZones.parseXML(xmlZones);

                Log(panelID, returnValue, xmlZones);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllZones: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_ZONES, AT_READ);

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
                String xmlPGM = "";

                Int32 returnValue = ReadPGM(panelID, pgmNo, out xmlPGM);

                panelPGM.parseXML(xmlPGM);

                Log(panelID, returnValue, xmlPGM);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadPGM: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, pgmNo, OT_PGM, AT_READ);

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
                String xmlPGMs = "";

                Int32 returnValue = ReadAllPGMs(panelID, out xmlPGMs);

                panelPGMs.parseXML(xmlPGMs);

                Log(panelID, returnValue, xmlPGMs);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllPGMs: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_PGMS, AT_READ);

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
                String xmlDoor = "";
            
                Int32 returnValue = ReadDoor(panelID, doorNo, out xmlDoor);
               
                panelDoor.parseXML(xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadDoor: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, doorNo, OT_DOOR, AT_READ);
                
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
                String xmlDoor = "";

                panelDoor.serializeXML(ref xmlDoor);

                Int32 returnValue = WriteDoor(panelID, doorNo, xmlDoor);

                Log(panelID, returnValue, xmlDoor);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteDoor: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, doorNo, OT_DOOR, AT_WRITE);

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
                String xmlDoors = "";

                Int32 returnValue = ReadAllDoors(panelID, out xmlDoors);

                panelDoors.parseXML(xmlDoors);

                Log(panelID, returnValue, xmlDoors);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllDoors: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_DOORS, AT_READ);

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
                String xmlUser = "";
                
                Int32 returnValue = ReadUser(panelID, userNo, out xmlUser);                          
                             
                panelUser.parseXML(xmlUser);

                Log(panelID, returnValue, xmlUser);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadUser: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, userNo, OT_USER, AT_READ);

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
                String xmlUser = "";

                panelUser.serializeXML(ref xmlUser);                
                
                Int32 returnValue = WriteUser(panelID, userNo, xmlUser);

                Log(panelID, returnValue, xmlUser);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteUser: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, userNo, OT_USER, AT_WRITE);

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
                String xmlUsers = "";

                Int32 returnValue = ReadAllUsers(panelID, out xmlUsers);

                panelUsers.parseXML(xmlUsers);

                Log(panelID, returnValue, xmlUsers);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllUsers: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_USERS, AT_READ);

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
                String xmlUsers = "";

                panelUsers.serializeXML(ref xmlUsers);
                              
                Int32 returnValue = WriteMultipleUsers(panelID, xmlUsers);                               

                Log(panelID, returnValue, xmlUsers);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteMultipleUsers: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_USERS, AT_WRITE);

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
                String xmlSchedule = "";

                Int32 returnValue = ReadSchedule(panelID, scheduleNo, out xmlSchedule);

                panelSchedule.parseXML(xmlSchedule);

                Log(panelID, returnValue, xmlSchedule);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadSchedule: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, scheduleNo, OT_SCHEDULE, AT_READ);

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
                String xmlSchedule = "";

                panelSchedule.serializeXML(ref xmlSchedule);

                Int32 returnValue = WriteSchedule(panelID, scheduleNo, xmlSchedule);

                Log(panelID, returnValue, xmlSchedule);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteSchedule: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, scheduleNo, OT_SCHEDULE, AT_WRITE);

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
                String xmlSchedules = "";

                Int32 returnValue = ReadAllSchedules(panelID, out xmlSchedules);                              
                
                panelSchedules.parseXML(xmlSchedules);
                                              
                Log(panelID, returnValue, xmlSchedules);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllSchedules: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_SCHEDULES, AT_READ);

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
                String xmlAccessLevel = "";

                Int32 returnValue = ReadAccessLevel(panelID, accessLevelNo, out xmlAccessLevel);

                panelAccessLevel.parseXML(xmlAccessLevel);

                Log(panelID, returnValue, xmlAccessLevel);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAccessLevel: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, accessLevelNo, OT_ACCESS_LEVEL, AT_READ);

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
                String xmlAccessLevel = "";

                panelAccessLevel.serializeXML(ref xmlAccessLevel);

                Int32 returnValue = WriteAccessLevel(panelID, accessLevelNo, xmlAccessLevel);

                Log(panelID, returnValue, xmlAccessLevel);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteAccessLevel: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, accessLevelNo, OT_ACCESS_LEVEL, AT_WRITE);

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
                String xmlAccessLevels = "";

                Int32 returnValue = ReadAllAccessLevels(panelID, out xmlAccessLevels);

                panelAccessLevels.parseXML(xmlAccessLevels);

                Log(panelID, returnValue, xmlAccessLevels);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadAllAccessLevels: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_ACCESS_LEVELS, AT_READ);

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
                String xmlHolidays = "";

                Int32 returnValue = ReadHolidays(panelID, out xmlHolidays);

                panelHolidayList.parseXML(xmlHolidays);

                Log(panelID, returnValue, xmlHolidays);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadHolidays: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_HOLIDAYS, AT_READ);

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
                String xmlHolidays = "";

                panelHolidayList.serializeXML(ref xmlHolidays);

                Int32 returnValue = WriteHolidays(panelID, xmlHolidays);

                Log(panelID, returnValue, xmlHolidays);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteHolidays: " + GetResultCode((UInt32)returnValue));
                }

                NotifyTaskCompleted(panelID, returnValue, 0, OT_HOLIDAYS, AT_WRITE);

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

            Log(panelID, returnValue, String.Format("Read Buffered Event Count: {0}", eventCount));

            if (!Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "ReadBufferedEvents: " + GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static Int32 ReadMonitoringStatus(UInt32 panelID, PanelMonitoring panelMonitoring)
        {
            if (panelMonitoring != null)
            {
                String xmlMonitoring = "";

                Int32 returnValue = ReadMonitoring(panelID, out xmlMonitoring);
                
                panelMonitoring.parseXML(xmlMonitoring);

                Log(panelID, returnValue, xmlMonitoring);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadMonitoringStatus: " + GetResultCode((UInt32)returnValue));
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
                String xmlTroubles = "";

                Int32 returnValue = SystemTroubles(panelID, out xmlTroubles);                

                panelTroubleList.parseXML(xmlTroubles);

                Log(panelID, returnValue, xmlTroubles);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "GetSystemTroubles: " + GetResultCode((UInt32)returnValue));
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

            if (!Succeeded((UInt32)returnValue))
            {
                errorNotification(panelID, returnValue, "StartControlPanelMonitoring: " + GetResultCode((UInt32)returnValue));
            }

            return returnValue;            
        }

        public static Int32 WriteIPReporting(UInt32 panelID, UInt32 receiverID, PanelIPReporting panelIPReporting)
        {
            if (panelIPReporting != null)
            {
                String xmlReporting = "";

                panelIPReporting.serializeXML(ref xmlReporting);

                Int32 returnValue = WriteIPReporting(panelID, receiverID, xmlReporting);

                Log(panelID, returnValue, xmlReporting);
                               
                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "WriteIPReporting: " + GetResultCode((UInt32)returnValue));
                }
                else
                {
                    NotifyTaskCompleted(panelID, returnValue, (UInt32)panelIPReporting.ReceiverNo, OT_IP_RECEIVER, AT_WRITE);
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
                String xmlReporting = "";

                Int32 returnValue = ReadIPReporting(panelID, receiverID, out xmlReporting);

                panelIPReporting.parseXML(xmlReporting);

                Log(panelID, returnValue, xmlReporting);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "ReadIPReporting: " + GetResultCode((UInt32)returnValue));
                }
                else                
                {
                    NotifyTaskCompleted(panelID, returnValue, (UInt32)panelIPReporting.ReceiverNo, OT_IP_RECEIVER, AT_READ, panelIPReporting);
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
                String xmlSetting = "";

                ipDOXSettings.serializeXML(ref xmlSetting);

                Int32 returnValue = startIPDOX(xmlSetting);

                Log(0, returnValue, xmlSetting);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartIPDOX: " + GetResultCode((UInt32)returnValue));
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

            if (!Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "StopIPDOX: " + GetResultCode((UInt32)returnValue));
            }

            return returnValue;            
        }

        public static Int32 DeleteIPDOXAccount(String macAddress)
        {
            Int32 returnValue = DeleteIPDOXAccount(macAddress);

            Log(0, returnValue, "DeleteIPDOXAccount");

            if (!Succeeded((UInt32)returnValue))
            {
                errorNotification(0, returnValue, "DeleteIPDOXAccount: " + GetResultCode((UInt32)returnValue));
            }

            return returnValue;
        }

        public static Int32 IPReportingStatus(UInt32 panelID, PanelIPReportingStatusList panelIPReportingStatusList)
        {
            if (panelIPReportingStatusList != null)
            {
                String xmlStatus = "";

                Int32 returnValue = IPReportingStatus(panelID, out xmlStatus);

                panelIPReportingStatusList.parseXML(xmlStatus);

                Log(panelID, returnValue, xmlStatus);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(panelID, returnValue, "IPReportingStatus: " + GetResultCode((UInt32)returnValue));
                }

                RefreshNotification(panelID, returnValue);

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 GetSiteFromPMH(String panelSerialNo, SiteInfo siteInfo)
        {
            if (siteInfo != null)
            {
                String xmlSiteInfo = "";                                

                Int32 returnValue = GetSiteFromPMH(panelSerialNo, out xmlSiteInfo);

                siteInfo.parseXML(xmlSiteInfo);

                Log(0, returnValue, xmlSiteInfo);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "GetSiteFromPMH: " + GetResultCode((UInt32)returnValue));
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
                String xmlVideoSettings = "";

                videoSettings.serializeXML(ref xmlVideoSettings);

                Int32 returnValue = ConfigureVideoServer(xmlVideoSettings);

                Log(0, returnValue, xmlVideoSettings);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "ConfigureVideoServer: " + GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }

        public static Int32 GetVideoAlarmFiles(String accountNo, UInt32 zoneNo, DateTime dateTime, VideoFileList videoFileList)
        {
            if (videoFileList != null)
            {
                String XMLVideoFiles = "";

                Double dt = dateTime.ToOADate();

                Int32 returnValue = GetVideoAlarmFiles(accountNo, zoneNo, dt, out XMLVideoFiles);

                videoFileList.parseXML(XMLVideoFiles);

                Log(0, returnValue, XMLVideoFiles);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "GetVideoAlarmFiles: " + GetResultCode((UInt32)returnValue));
                }

                return returnValue;
            }
            else
            {
                return -1;
            }
        }


        public static Int32 StartVideoOnDemand(String ipAddress, UInt32 ipPort, String SessionKey, VideoFileList videoFileList)
        {
            if (videoFileList != null)
            {
                String xmlVideoFile = "";                               

                Int32 returnValue = StartVideoOnDemand(ipAddress, ipPort, SessionKey, out xmlVideoFile);

                videoFileList.parseXML(xmlVideoFile);

                Log(0, returnValue, xmlVideoFile);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartVideoOnDemand: " + GetResultCode((UInt32)returnValue));
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
                String xmlVideoFile = "";
                String xmlVODSettings = "";

                vodSettings.serializeXML(ref xmlVODSettings);

                Int32 returnValue = StartVideoOnDemandEx(panelID, xmlVODSettings, out xmlVideoFile);

                videoFileList.parseXML(xmlVideoFile);

                Log(0, returnValue, xmlVideoFile);

                if (!Succeeded((UInt32)returnValue))
                {
                    errorNotification(0, returnValue, "StartVideoOnDemandEx: " + GetResultCode((UInt32)returnValue));
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
