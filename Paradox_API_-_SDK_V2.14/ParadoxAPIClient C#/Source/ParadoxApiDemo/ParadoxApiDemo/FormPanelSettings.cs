using System;
using System.Windows.Forms;
using Harmony.SDK.Paradox;
using Harmony.SDK.Paradox.Model;

namespace ParadoxApiDemo
{
    public partial class FormPanelSettings : Form
    {
        public PanelSettings mSettings = new PanelSettings();

        private void FillPanelType()
        {
            cbxPanelType.Items.Clear();
            cbxPanelType.Items.Add("Auto-Detect");
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_SP4000);
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_SP5500);
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_SP6000);
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_SP7000);
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_MG5000);
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_MG5050);

            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_DGP_EVO_192);
            cbxPanelType.Items.Add(PanelTypes.CP_PROD_NAME_DGP_EVO_VHD);

            cbxPanelType.SelectedIndex = 0;
        }

        private void FillSystemAlarmLanguage()
        {
            cbxSystemAlarmLanguage.Items.Clear();
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ENGLISH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_FRENCH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_SPANISH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ITALIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_SWEDISH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_POLISH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_PORTUGUESE);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_GERMAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_TURKISH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_HUNGARIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_CZECH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_DUTCH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_CROATIAN); ;
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_GREEK);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_HEBREW);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_RUSSIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_BULGARIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ROMANIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_SLOVAK);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_CHINESE);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_SERBIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_MALAY);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_SLOVENIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_LITHUANIAN); ;
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_FINNISH);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ESTONIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_BELGIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_LATVIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_MACEDONIA); ;
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ALBANIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_NORWEGIAN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ARABIC_PERSAIN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ARABIC_WESTERN);
            cbxSystemAlarmLanguage.Items.Add(PanelLanguages.SYS_ALARM_LANGUAGE_ARABIC_EASTERN);
            cbxSystemAlarmLanguage.Sorted = true;
            cbxSystemAlarmLanguage.SelectedIndex = cbxSystemAlarmLanguage.Items.IndexOf(PanelLanguages.SYS_ALARM_LANGUAGE_ENGLISH);
        }

        public FormPanelSettings()
        {
            InitializeComponent();

            FillPanelType();

            FillSystemAlarmLanguage();

            cbxConnectionType.SelectedIndex = 0;
            cbxSerialComPort.SelectedIndex = 0;
            cbxSerialBaudRate.SelectedIndex = 0;
            txbDNSSiteID.Text = "";
            ipAddressControl.Text = "...";
            txbIPPort.Text = "10000";
            txtIPModulePassword.Text = "paradox";
            txbUserPassword.Text = "1234";
        }

        public FormPanelSettings(PanelSettings settings)
        {
            InitializeComponent();

            FillPanelType();

            if (settings.PanelType != null)
            {
                cbxPanelType.SelectedIndex = cbxPanelType.Items.IndexOf(settings.PanelType);
            }

            if (cbxPanelType.SelectedIndex == -1)
            {
                cbxPanelType.SelectedIndex = 0;
            }

            FillSystemAlarmLanguage();

            if (settings.SystemAlarmLanguage != null)
            {
                cbxSystemAlarmLanguage.SelectedIndex = cbxSystemAlarmLanguage.Items.IndexOf(settings.SystemAlarmLanguage);
            }

            if (cbxSystemAlarmLanguage.SelectedIndex == -1)
            {
                cbxSystemAlarmLanguage.SelectedIndex = cbxSystemAlarmLanguage.Items.IndexOf(PanelLanguages.SYS_ALARM_LANGUAGE_ENGLISH);
            }

            cbxConnectionType.SelectedIndex = -1;

            mSettings.PanelType = settings.PanelType;
            mSettings.ComType = settings.ComType;
            mSettings.SiteID = settings.SiteID;
            mSettings.SerialNo = settings.SerialNo;
            mSettings.IPAddress = settings.IPAddress;
            mSettings.IPPort = settings.IPPort;
            mSettings.ComPort = settings.ComPort;
            mSettings.BaudRate = settings.BaudRate;
            mSettings.SMSCallback = settings.SMSCallback;
            mSettings.IPPassword = settings.IPPassword;
            mSettings.UserCode = settings.UserCode;
            mSettings.SystemAlarmLanguage = settings.SystemAlarmLanguage;

            txbUserPassword.Text = mSettings.UserCode;

            if ((mSettings.ComPort - 1) < cbxSerialComPort.Items.Count)
            {
                cbxSerialComPort.SelectedIndex = mSettings.ComPort - 1;
            }
            else
            {
                cbxSerialComPort.SelectedIndex = 0;
            }

            switch (mSettings.BaudRate)
            {
                case 921600: { cbxSerialBaudRate.SelectedIndex = 1; break; }
                case 115200: { cbxSerialBaudRate.SelectedIndex = 2; break; }
                case 57600: { cbxSerialBaudRate.SelectedIndex = 3; break; }
                case 38400: { cbxSerialBaudRate.SelectedIndex = 4; break; }
                case 19200: { cbxSerialBaudRate.SelectedIndex = 5; break; }
                case 9600: { cbxSerialBaudRate.SelectedIndex = 6; break; }
                default: { cbxSerialBaudRate.SelectedIndex = 0; break; }
            }

            txbDNSSiteID.Text = mSettings.SiteID;
            ipAddressControl.Text = mSettings.IPAddress;
            txbIPPort.Text = Convert.ToString(mSettings.IPPort);
            txtIPModulePassword.Text = mSettings.IPPassword;
            txbUserPassword.Text = mSettings.UserCode;


            if (mSettings.ComType == "SERIAL")
            {
                cbxConnectionType.SelectedIndex = 0;

                txbDNSSiteID.Enabled = false;
                ipAddressControl.Enabled = false;
                txbIPPort.Enabled = false;
                txtIPModulePassword.Enabled = false;

            }
            else if (mSettings.ComType == "IP")
            {
                cbxConnectionType.SelectedIndex = 1;

                txbDNSSiteID.Enabled = false;
                cbxSerialComPort.Enabled = false;
                cbxSerialBaudRate.Enabled = false;

            }
            else if (mSettings.ComType == "GPRSSTATIC")
            {
                //GPRS callback

                cbxConnectionType.SelectedIndex = 2;

                txbDNSSiteID.Enabled = false;
                cbxSerialComPort.Enabled = false;
                cbxSerialBaudRate.Enabled = false;

            }
            else if (mSettings.ComType == "GPRSPRIVATE")
            {
                //GPRS Private
                cbxConnectionType.SelectedIndex = 3;

                txbDNSSiteID.Enabled = false;
                cbxSerialComPort.Enabled = false;
                cbxSerialBaudRate.Enabled = false;

            }
            else if (mSettings.ComType == "DNS")
            {
                cbxConnectionType.SelectedIndex = 4;

                cbxSerialComPort.Enabled = false;
                cbxSerialBaudRate.Enabled = false;
                ipAddressControl.Enabled = false;
                txbIPPort.Enabled = false;

            }
            else
            {
                cbxConnectionType.SelectedIndex = 0;

                txbDNSSiteID.Enabled = false;
                ipAddressControl.Enabled = false;
                txbIPPort.Enabled = false;
                txtIPModulePassword.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cbxPanelType.SelectedIndex == 0) //Auto-Detect selected
            {
                mSettings.PanelType = "";
            }
            else
            {
                mSettings.PanelType = cbxPanelType.Items[cbxPanelType.SelectedIndex].ToString();
            }


            mSettings.SystemAlarmLanguage = cbxSystemAlarmLanguage.Items[cbxSystemAlarmLanguage.SelectedIndex].ToString();


            switch (cbxConnectionType.SelectedIndex)
            {
                case 0:
                    {
                        mSettings.ComType = "SERIAL";
                        break;
                    }
                case 1:
                    {
                        mSettings.ComType = "IP";
                        break;
                    }
                case 2:
                    {
                        mSettings.ComType = "GPRSSTATIC";
                        break;
                    }
                case 3:
                    {
                        mSettings.ComType = "GPRSPRIVATE";
                        break;
                    }
                case 4:
                    {
                        mSettings.ComType = "DNS";
                        break;
                    }
                default:
                    {
                        mSettings.ComType = "SERIAL";
                        break;
                    }
            }

            mSettings.ComPort = cbxSerialComPort.SelectedIndex + 1;

            switch (cbxSerialBaudRate.SelectedIndex)
            {
                case 0: { mSettings.BaudRate = 0; break; }
                case 1: { mSettings.BaudRate = 921600; break; }
                case 2: { mSettings.BaudRate = 115200; break; }
                case 3: { mSettings.BaudRate = 57600; break; }
                case 4: { mSettings.BaudRate = 38400; break; }
                case 5: { mSettings.BaudRate = 19200; break; }
                case 6: { mSettings.BaudRate = 9600; break; }
                default: { mSettings.BaudRate = 0; break; }
            }

            mSettings.SiteID = txbDNSSiteID.Text;
            mSettings.IPAddress = ipAddressControl.Text;
            mSettings.IPPort = Convert.ToInt32(txbIPPort.Text);
            mSettings.IPPassword = txtIPModulePassword.Text;
            mSettings.UserCode = txbUserPassword.Text;
        }

        private void cbxConnectionType_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (cbxConnectionType.SelectedIndex)
            {
                case 0:
                    {
                        cbxSerialComPort.Enabled = true;
                        cbxSerialBaudRate.Enabled = true;
                        txbDNSSiteID.Enabled = false;
                        ipAddressControl.Enabled = false;
                        txbIPPort.Enabled = false;
                        txtIPModulePassword.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        cbxSerialComPort.Enabled = false;
                        cbxSerialBaudRate.Enabled = false;
                        txbDNSSiteID.Enabled = false;
                        ipAddressControl.Enabled = true;
                        txbIPPort.Enabled = true;
                        txtIPModulePassword.Enabled = true;
                        mSettings.SMSCallback = false;
                        break;
                    }
                case 2:
                    {
                        cbxSerialComPort.Enabled = false;
                        cbxSerialBaudRate.Enabled = false;
                        txbDNSSiteID.Enabled = false;
                        ipAddressControl.Enabled = true;
                        txbIPPort.Enabled = true;
                        txtIPModulePassword.Enabled = true;
                        mSettings.SMSCallback = true;
                        break;
                    }
                case 3:
                    {
                        cbxSerialComPort.Enabled = false;
                        cbxSerialBaudRate.Enabled = false;
                        txbDNSSiteID.Enabled = false;
                        ipAddressControl.Enabled = true;
                        txbIPPort.Enabled = true;
                        txtIPModulePassword.Enabled = true;
                        mSettings.SMSCallback = false;
                        break;
                    }
                case 4:
                    {
                        cbxSerialComPort.Enabled = false;
                        cbxSerialBaudRate.Enabled = false;
                        txbDNSSiteID.Enabled = true;
                        ipAddressControl.Enabled = false;
                        txbIPPort.Enabled = false;
                        txtIPModulePassword.Enabled = true;
                        break;
                    }
                default:
                    {
                        cbxSerialComPort.Enabled = true;
                        cbxSerialBaudRate.Enabled = true;
                        txbDNSSiteID.Enabled = false;
                        ipAddressControl.Enabled = false;
                        txbIPPort.Enabled = false;
                        txtIPModulePassword.Enabled = false;
                        break;
                    }
            }
        }
    }
}
