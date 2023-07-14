using System.Globalization;
using System.Threading;

namespace Figures.UI
{
    public partial class App
    {
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }
}