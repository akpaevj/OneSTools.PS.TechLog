using System.Collections.Generic;

namespace OneSTools.PS.TechLog
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue defaultValue)
        {
            if (d.TryGetValue(key, out TValue value))
                return value;
            else
                return defaultValue;
        }
    }
}