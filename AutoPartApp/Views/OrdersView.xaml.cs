using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace AutoPartApp
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<OrdersViewModel>();
        }
    }
}
