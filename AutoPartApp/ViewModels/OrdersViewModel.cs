using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoPartApp;

public partial class OrdersViewModel : ObservableObject
{


    public string OrdersName => Properties.Strings.OrdersName;
}