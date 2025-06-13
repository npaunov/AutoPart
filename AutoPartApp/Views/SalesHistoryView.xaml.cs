using System.Windows.Controls;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using AutoPartApp.ViewModels;
using AutoPartApp.DIServices.Messengers;

namespace AutoPartApp.Views
{
    public partial class SalesHistoryView : UserControl
    {
        public SalesHistoryView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<SalesHistoryViewModel>();

            // Subscribe to language change
            WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
            {
                UpdateFirstThreeHeaders();
            });
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

            UpdateFirstThreeHeaders();
        }

        private void UpdateFirstThreeHeaders()
        {
            // Update only the first three column headers with localized strings
            if (SalesGrid.Columns.Count >= 3)
            {
                SalesGrid.Columns[0].Header = AutoPartApp.Properties.Strings.PartIDName;
                SalesGrid.Columns[1].Header = AutoPartApp.Properties.Strings.DescriptionName;
                SalesGrid.Columns[2].Header = AutoPartApp.Properties.Strings.TotalSalesName;
            }
        }
    }
}