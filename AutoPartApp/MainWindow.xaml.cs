using System.Windows;

namespace AutoPartApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ViewAutoParts_Click(object sender, RoutedEventArgs e)
    {
        // Navigate to AutoPartsView
        var autoPartsView = new AutoPartsView();
        //autoPartsView.Show();
        this.Close();
    }
}