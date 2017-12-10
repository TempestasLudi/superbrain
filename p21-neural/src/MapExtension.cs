using System;
using System.Collections.Generic;
using System.Linq;

namespace p21_neural {

    public static class MapExtension {

        public static void ForEach<T1, T2>(this Dictionary<T1, T2> dictionary, Action<T2> action) {
            dictionary.ToList().ForEach(entry => action(entry.Value));
        }

    }

}