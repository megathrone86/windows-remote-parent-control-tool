using System.Windows;
using WRPCT.Services;
using WRPCT.Views;

namespace WRPCT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            string command = e.Args.Length > 0 ? e.Args[0] : null;
            command = "start";
            switch (command)
            {
                case "start":
                    Start();
                    break;
                case "stop":
                    Stop();
                    System.Windows.Application.Current.Shutdown();
                    break;
                default:
                    ShowSettings();
                    break;
            };
        }

        void ShowSettings()
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
        }

        void Stop()
        {
        }

        void Start()
        {
            MainService.Instance.Start();
        }
    }
}
