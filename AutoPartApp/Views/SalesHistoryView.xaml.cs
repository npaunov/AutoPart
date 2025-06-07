using System.Windows.Controls;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using AutoPartApp.ViewModels;

namespace AutoPartApp.Views
{
    public partial class SalesHistoryView : UserControl
    {
        public SalesHistoryView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<SalesHistoryViewModel>();
        }

        private void SalesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as SalesHistoryViewModel;
            if (vm == null) return;

            // Remove old month columns if reloaded
            while (SalesGrid.Columns.Count > 3)
                SalesGrid.Columns.RemoveAt(3);

            // Add month columns dynamically
            foreach (var month in vm.MonthHeaders)
            {
                var col = new DataGridTextColumn
                {
                    Header = month,
                    Binding = new System.Windows.Data.Binding($"MonthlySales[{month}]")
                };
                SalesGrid.Columns.Add(col);
            }
        }
    }
}