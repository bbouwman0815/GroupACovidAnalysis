using System;
using System.Collections.Generic;

namespace Covid19Analysis.Comparers
{
    /// <summary>
    ///     General Lambda Comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IComparer{T}" />
    public class LambdaComparer<T> : IComparer<T>
    {
        #region Data members

        private readonly Func<T, T, int> compareFunction;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LambdaComparer{T}" /> class.
        /// </summary>
        /// <param name="compareFunction">The compare function.</param>
        public LambdaComparer(Func<T, T, int> compareFunction)
        {
            this.compareFunction = compareFunction;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>int with difference</returns>
        public int Compare(T x, T y)
        {
            return this.compareFunction(x, y);
        }

        #endregion
    }
}