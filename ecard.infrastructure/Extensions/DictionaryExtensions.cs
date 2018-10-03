using System;
using System.Collections.Generic;

namespace Ecard
{
    public static class DictionaryExtensions
    {
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dst, IDictionary<TKey, TValue> src)
        {
            if (dst == null) throw new ArgumentNullException("dst");
            if (src == null) throw new ArgumentNullException("src");

            foreach (var pair in src)
            {
                if (!dst.ContainsKey(pair.Key))
                    dst.Add(pair.Key, pair.Value);
            }
        }
    }
}