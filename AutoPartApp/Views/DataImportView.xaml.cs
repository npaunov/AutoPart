using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using AutoPartApp.ViewModels;

namespace AutoPartApp.Views
{
    /// <summary>
    /// Interaction logic for DataImportView.xaml
    /// </summary>
    public partial class DataImportView : UserControl
    {
        public DataImportView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<DataImportViewModel>();
        }
    }
}