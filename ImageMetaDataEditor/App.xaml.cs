using System.Windows;

namespace ImageMetaDataEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindowViewModel mainVm = new MainWindowViewModel();
            MainWindow main = new MainWindow { DataContext = mainVm };
            main.Show();
        }
    }
}
