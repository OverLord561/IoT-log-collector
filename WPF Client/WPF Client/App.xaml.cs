using System.Windows;
using Unity;
using WPF_Client.Helpers;
using WPF_Client.Services;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            container = new UnityContainer();
            container.RegisterSingleton<GlobalSynchroObject>();
            container.RegisterSingleton<IHttpClient, RestSharpHttpClient>();

            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();            
        }
    }
}
