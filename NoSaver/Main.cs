using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Mk0.Software.OnlineUpdater;

namespace NoSaver
{
    public partial class Main : Form
    {
        [FlagsAttribute()]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x40,
            ES_CONTINUOUS = 0x80000000u,
            ES_DISPLAY_REQUIRED = 0x2,
            ES_SYSTEM_REQUIRED = 0x1
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport("user32", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const Int32 SPI_SETSCREENSAVETIMEOUT = 15;

        public Main()
        {
            InitializeComponent();
        }

        public void KeepMonitorActive()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        }

        public void RestoreMonitorSettings()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            KeepMonitorActive();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RestoreMonitorSettings();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.Start("https://www.kmpr.at/update/nosaver.xml");
        }
    }
}
