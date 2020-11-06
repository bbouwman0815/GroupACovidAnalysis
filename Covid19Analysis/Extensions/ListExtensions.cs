using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Covid19Analysis.Extensions
{
    /// <summary>
    /// The class List Extensions
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Converts a standard collection into an observable list
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ObservableCollection<T>
            ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
    
}
