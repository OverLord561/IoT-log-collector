using System;
using System.Windows;
using WPF_Client.ViewModels;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = new MainWindow();
            mainView.Show();
            mainView.DataContext = new MainViewModel();
        }
    }
}
