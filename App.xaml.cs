using System.Windows;
using NiceLabel.SDK;

namespace AutoGenLabel
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            PrintEngineFactory.PrintEngine.Shutdown();

            base.OnExit(e);
        }
    }
}