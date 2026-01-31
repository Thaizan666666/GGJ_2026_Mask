using System.Collections.Generic;
using UnityEngine;

public static class RandomUtil
{
    /// <summary>
    /// Returns a random key from the given dictionary.
    /// </summary>
    public static TKey RandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dic)
    {
        if (dic == null || dic.Count == 0)
        {
            Debug.LogWarning("RandomKey: Dictionary is null or empty.");
            return default;
        }

        int index = Random.Range(0, dic.Count);
        foreach (var key in dic.Keys)
        {
            if (index == 0) return key;
            index--;
        }

        return default; // Should never reach here
    }

    /// <summary>
    /// Returns the value associated with the given key.
    /// Returns default if the key is not found.
    /// </summary>
    public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
    {
        if (dic == null)
        {
            Debug.LogWarning("GetValue: Dictionary is null.");
            return default;
        }

        if (dic.TryGetValue(key, out TValue value))
        {
            return value;
        }

        Debug.LogWarning($"GetValue: Key '{key}' not found.");
        return default;
    }

    /// <summary>
    /// Returns a random element from the given list.
    /// </summary>
    public static T RandomValue<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("RandomValue: List is null or empty.");
            return default;
        }

        return list[Random.Range(0, list.Count)];
    }
}