using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using Orientation = System.Windows.Controls.Orientation;

namespace CADPythonShell
{
    public class IronPythonConsoleApp
    {
        public const string RibbonTitle = "Python Shell";
        public const string RibbonId = "PythonShell";

        [CommandMethod("InitPythonConsole")]
        public void Execute()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab(RibbonId);
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }
                rtab = new RibbonTab();
                rtab.Title = RibbonTitle;
                rtab.Id = RibbonId;
                ribbon.Tabs.Add(rtab);
                AddContentToTab(rtab);
            }
        }

        private void AddContentToTab(RibbonTab rtab)
        {
            rtab.Panels.Add(AddOnePanel());
        }

        private static RibbonPanel AddOnePanel()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Cad Python Shell";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "Python Shell Console";
            rps.DialogLauncher = rci;
            var addinAssembly = typeof(IronPythonConsoleApp).Assembly;
            //create button1
            RibbonButton rb = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "Run CPS",
                ShowText = true,
                Text = "Run CPS",
                Description = "Start Write Python Console\nCommand: PythonShellConsole",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Python-16.png"),
                LargeImage = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Python-32.png"),
                CommandHandler = new RelayCommand(new IronPythonConsoleCommand().Execute)
            };
            rps.Items.Add(rb);
            //create button2
            RibbonButton rb2 = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                ShowText = true,
                Text = "Configure CPS",
                Description = "Configure Cad Python Shell\nCommand: PythonShellSetting",
                Image = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-16.png"),
                LargeImage = CADPythonShellApplication.GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.Settings-32.png"),
                Name = "Configure CPS",
                CommandHandler = new RelayCommand(new ConfigureCommand().Execute)
            };

            //Add the Button to the Tab
            rps.Items.Add(rb2);
            return rp;
        }
    }
}