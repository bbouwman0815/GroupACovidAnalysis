using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     The Covid Data for all regions
    /// </summary>
    public class TotalCovidStats : ICollection<DailyCovidStat>
    {
        #region Properties

        private ICollection<DailyCovidStat> CovidData { get; }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public int Count => this.CovidData.Count;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        public bool IsReadOnly => true;

        #endregion

        #region Constructors


        /// <summary>
        ///     Initializes a new instance of the <see cref="TotalCovidStats" /> class.
        /// </summary>
        public TotalCovidStats()
        {
            this.CovidData = new Collection<DailyCovidStat>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <Precondition>statistic != null</Precondition>
        /// <Postcondition>Count == Count@prev + 1</Postcondition>
        /// <param name="statistic">
        ///     The object to add to the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </param>
        /// <exception cref="NullReferenceException">Cannot add null CovidStat</exception>
        public void Add(DailyCovidStat statistic)
        {
            if (statistic == null)
            {
                throw new NullReferenceException("Cannot add null statistic");
            }

            this.CovidData.Add(statistic);
        }

        /// <summary>
        ///     Removes all items from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <Precondition>none</Precondition>
        /// <Postcondition>Count == 0</Postcondition>
        /// <exception cref="NullReferenceException">Cannot clear null collection</exception>
        public void Clear()
        {
            this.CovidData.Clear();
        }

        /// <summary>
        ///     Determines whether this instance contains the object.
        /// </summary>
        /// <Precondition>statistic != null</Precondition>
        /// <param name="statistic">
        ///     The object to locate in the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </param>
        /// <returns>
        ///     true if item is found in the
        ///     <see cref="T:System.Collections.Generic.ICollection`1">
        ///     </see>
        ///     ; otherwise, false.
        /// </returns>
        /// <exception cref="NullReferenceException">Item cannot be null</exception>
        public bool Contains(DailyCovidStat statistic)
        {
            var containsStatistic = false;
            if (statistic == null)
            {
                throw new NullReferenceException("Item cannot be null");
            }

            if (this.CovidData.Contains(statistic))
            {
                containsStatistic = true;
            }

            return containsStatistic;
        }

        /// <summary>
        ///     Copies the elements of the
        ///     <see cref="T:System.Collections.Generic.ICollection`1">
        ///     </see>
        ///     to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional
        ///     <see cref="T:System.Array">
        ///     </see>
        ///     that is the destination of the elements copied from
        ///     <see cref="T:System.Collections.Generic.ICollection`1">
        ///     </see>
        ///     . The <see cref="T:System.Array"></see> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="IndexOutOfRangeException">Collection cannot be empty</exception>
        /// <exception cref="NullReferenceException">Collection cannot be null</exception>
        /// <exception cref="ArgumentException">Index cannot be negative</exception>
        public void CopyTo(DailyCovidStat[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
            {
                throw new ArgumentException("Index cannot be negative");
            }

            this.CovidData.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <Precondition>statistic != null</Precondition>
        /// <Postcondition>Count == Count@prev - 1</Postcondition>
        /// <param name="statistic">
        ///     The object to remove from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </param>
        /// <returns>
        ///     true if item was successfully removed from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1">
        ///     </see>
        ///     ; otherwise, false. This method also returns false if item is not found in the original
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="NullReferenceException">Statistic cannot be null</exception>
        public bool Remove(DailyCovidStat statistic)
        {
            if (statistic == null)
            {
                throw new NullReferenceException("Statistic cannot be null");
            }

            return this.CovidData.Remove(statistic);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DailyCovidStat> GetEnumerator()
        {
            return this.CovidData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Finds the regions from a set of data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// the different regions of a set of data
        /// </returns>
        public static string[] FindRegions(IEnumerable<DailyCovidStat> data)
        {
            var regionalData = new Dictionary<string, List<DailyCovidStat>>();

            foreach (var currentDay in data.Where(currentDay => !regionalData.ContainsKey(currentDay.Region)))
            {
                regionalData.TryAdd(currentDay.Region, new List<DailyCovidStat>());
            }

            regionalData.TryAdd("All", new List<DailyCovidStat>());

            var regions = regionalData.Keys.ToArray();

            return regions;
        }

        /// <summary>
        ///     Creates a regional dictionary from a set of Data.
        /// </summary>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     creates a regional dictionary from a set of Data
        /// </returns>
        public Dictionary<string, List<DailyCovidStat>> CreateRegionalDictionary(List<DailyCovidStat> data)
        {
            var regions = FindRegions(data);
            var dictionary = new Dictionary<string, List<DailyCovidStat>>();

            foreach (var t in regions)
            {
                var regionalData = this.CovidData.Where(currentDay => t.Equals(currentDay.Region)).ToList();
                dictionary.TryAdd(t, regionalData);
            }

            foreach (var currentDay in data)
            {
                dictionary["All"].Add(currentDay);
            }

            return dictionary;
        }

        /// <summary>
        ///     Creates the monthly dictionary.
        /// </summary>
        /// <Precondition>Collection != null AND Count > 0</Precondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     A dictionary with the monthly Data of a Region
        /// </returns>
        public Dictionary<string, List<DailyCovidStat>> CreateMonthlyDictionary(List<DailyCovidStat> data)
        {
            if (this.CovidData.Count <= 0)
            {
                throw new IndexOutOfRangeException("Collection cannot be empty");
            }

            if (this.CovidData == null)
            {
                throw new NullReferenceException("Collection cannot be null");
            }

            var dictionary = new Dictionary<string, List<DailyCovidStat>>();

            for (var i = 1; i < 13; i++)
            {
                dictionary.TryAdd(i.ToString(), new List<DailyCovidStat>());
            }

            foreach (var currentDay in data)
            {
                var month = currentDay.Date.Month.ToString();
                dictionary[month].Add(currentDay);
            }

            return dictionary;
        }

        /// <summary>
        ///     Replaces the daily stat.
        /// </summary>
        /// <param name="day">The Day.</param>
        /// <returns>
        ///     true if duplicate found and replaced, false otherwise
        /// </returns>
        public bool ReplaceDuplicate(DailyCovidStat day)
        {
            var wasReplaced = false;

            foreach (var currentDay in this.CovidData.ToList()
                                           .Where(currentDay =>
                                               currentDay.Date == day.Date && currentDay.Region == day.Region))
            {
                this.CovidData.Remove(currentDay);
                this.CovidData.Add(day);
                wasReplaced = true;
            }

            return wasReplaced;
        }

        /// <summary>
        ///     Determines whether this instance contains Data.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance contains Data; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsData()
        {
            return this.CovidData.Count > 0;
        }

        /// <summary>
        ///     Determines whether the collection has any days with positive/negative tests
        /// </summary>
        /// <returns>
        ///     <c>true</c> if has positive/negative days; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsTestData()
        {
            var hasData = false;
            foreach (var _ in this
                              .CovidData.Where(currentDay => currentDay.HasPositiveCases || currentDay.HasNegativeCases)
                              .Select(currentDay => new { }))
            {
                hasData = true;
            }

            return hasData;
        }

        #endregion
    }
}