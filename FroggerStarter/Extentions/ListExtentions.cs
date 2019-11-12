﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroggerStarter.Extentions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}

