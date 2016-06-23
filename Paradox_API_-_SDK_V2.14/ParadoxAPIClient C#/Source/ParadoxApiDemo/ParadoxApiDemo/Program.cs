using System;
using System.Windows.Forms;
using Harmony.SDK.Paradox.Model;
using Harmony.SDK.Paradox.Services;

namespace ParadoxApiDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //var t = new PanelMonitoring();

            //t.ItemNo = 777;
            //t.ItemType = "The item type";
            //t.Status = "Status";

            //var xml = SerializeService.Serialize(t);
            //System.Diagnostics.Trace.Write("PanelMonitoring as XML: ", xml);

            //var json = SerializeService.Serialize(t, false);
            //System.Diagnostics.Trace.Write("PanelMonitoring as XML: ", json);

            var pc = new PanelControl
            {
                Command = "the command",
                Items = "the items",
                Timer = 123
            };
            var xml = SerializeService.Serialize(pc);
            System.Diagnostics.Trace.Write(xml);

            var json = SerializeService.Serialize(pc, false);
            System.Diagnostics.Trace.Write(json);
            
            Application.Run(new FormParadoxAPI());
        }
    }
}