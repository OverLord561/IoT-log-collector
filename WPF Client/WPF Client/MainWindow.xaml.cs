using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity.Attributes;
using WPF_Client.ViewModels;

namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public MainViewModel ViewModel
        {
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Blah(Task t)
        {
            var delayTask = Task.Delay(5000);
            var task = await Task.WhenAny(t, delayTask);
        }
    }

   
}