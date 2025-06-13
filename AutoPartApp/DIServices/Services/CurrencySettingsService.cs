using System.ComponentModel;

namespace AutoPartApp.DIServices.Services
{
    /// <summary>
    /// Provides a globally accessible, observable Euro exchange rate for the application.
    /// </summary>
    public class CurrencySettingsService : INotifyPropertyChanged
    {
        private decimal _euroRate = 1.95583m; // Default BGN to EUR rate

        /// <summary>
        /// Gets or sets the current Euro exchange rate.
        /// Notifies listeners when changed.
        /// </summary>
        public decimal EuroRate
        {
            get => _euroRate;
            set
            {
                if (_euroRate != value)
                {
                    _euroRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EuroRate)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
