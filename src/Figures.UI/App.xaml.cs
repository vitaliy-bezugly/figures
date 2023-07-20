using System.Windows;

namespace Figures.UI
{
    public partial class App
    {
        public App()
        { }

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("Log4Net.config"));
            base.OnStartup(e);
        }
    }
}