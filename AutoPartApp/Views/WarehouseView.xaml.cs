using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using AutoPartApp.ViewModels;

namespace AutoPartApp.Views
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