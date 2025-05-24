using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace AutoPartApp
{
    /// <summary>
    /// Interaction logic for StoresView.xaml
    /// </summary>
    public partial class StoresView : UserControl
    {
        public StoresView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<StoresViewModel>();
        }
    }
}