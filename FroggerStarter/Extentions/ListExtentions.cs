﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FroggerStarter.Extentions
{
    /// <summary>Class for implementing list extensions for Scoreboard</summary>
    public static class ListExtensions
    {
        #region Methods

        /// <summary>Converts to observablecollection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        #endregion
    }
}