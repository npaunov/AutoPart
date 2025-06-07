using System.Collections.ObjectModel;

namespace AutoPartApp.Extensions
{
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Converts an IEnumerable<T> to an ObservableCollection<T>.
        /// </summary>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }
    }
}