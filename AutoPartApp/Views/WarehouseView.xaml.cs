using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace AutoPartApp
{
    /// <summary>
    /// Interaction logic for WarehouseView.xaml
    /// </summary>
    public partial class WarehouseView : UserControl
    {
        public WarehouseView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<WarehouseViewModel>();
        }
    }
}