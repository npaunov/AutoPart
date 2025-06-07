using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using AutoPartApp.ViewModels;

namespace AutoPartApp.Views
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