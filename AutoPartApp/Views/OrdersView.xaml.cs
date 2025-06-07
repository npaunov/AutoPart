using AutoPartApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace AutoPartApp.Views
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    /// <summary>
    /// This handles limitation in WPF DataGrid where the headers are not updated.
    /// Creates dynamic DataGrid column headers for sales columns.
    /// Sets and updates the headers in response to ViewModel property changes,
    /// ensuring that the DataGrid reflects the current header values at runtime.
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
            var vm = App.AppHost.Services.GetRequiredService<OrdersViewModel>();
            DataContext = vm;

            // Set initial headers for sales columns
            SetSalesHeaders(vm);

            // Update headers dynamically when ViewModel properties change
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is "Sales1Header" or "Sales2Header" or "Sales3Header" or "Sales4Header" or "Sales5Header")
                {
                    SetSalesHeaders(vm);
                }
            };
        }

        /// <summary>
        /// Sets the DataGrid sales column headers to the current values from the ViewModel.
        /// </summary>
        private void SetSalesHeaders(OrdersViewModel vm)
        {
            Sales1Column.Header = vm.Sales1Header;
            Sales2Column.Header = vm.Sales2Header;
            Sales3Column.Header = vm.Sales3Header;
            Sales4Column.Header = vm.Sales4Header;
            Sales5Column.Header = vm.Sales5Header;
        }
    }

}