using System;
using System.Collections.Generic;
using UnityEngine;

public static class RandomUtil
{
    /// <summary>
    /// Returns a random key from the dictionary.
    /// </summary>
    public static TKey RandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dic)
    {
        if (dic == null || dic.Count == 0)
        {
            Debug.LogWarning("RandomKey: Dictionary is null or empty.");
            return default;
        }
        int index = UnityEngine.Random.Range(0, dic.Count);
        using (var enumerator = dic.Keys.GetEnumerator())
        {
            for (int i = 0; i <= index; i++)
                enumerator.MoveNext();
            return enumerator.Current;
        }
    }

    /// <summary>
    /// Try get value safely.
    /// </summary>
    public static bool TryGetValueSafe<TKey, TValue>(
        this Dictionary<TKey, TValue> dic,
        TKey key,
        out TValue value)
    {
        if (dic == null)
        {
            Debug.LogWarning("Dictionary is null.");
            value = default;
            return false;
        }
        if (dic.TryGetValue(key, out value))
            return true;
        Debug.LogWarning($"Key '{key}' not found.");
        return false;
    }

    /// <summary>
    /// Returns a random element from a list.
    /// </summary>
    public static T RandomValue<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("RandomValue: List is null or empty.");
            return default;
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Returns a random value from an enum type.
    /// Usage: var result = RandomUtil.RandomEnumValue<MyEnum>();
    /// </summary>
    public static T RandomEnumValue<T>() where T : Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        if (values.Length == 0)
        {
            Debug.LogWarning($"RandomEnumValue: Enum '{typeof(T).Name}' has no values.");
            return default;
        }
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    /// <summary>
    /// Returns a random enum value excluding specified values.
    /// Usage: var result = RandomUtil.RandomEnumValue<MyEnum>(MyEnum.None, MyEnum.Invalid);
    /// </summary>
    public static T RandomEnumValue<T>(params T[] exclude) where T : Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        var filtered = new List<T>();

        foreach (T v in values)
        {
            bool isExcluded = false;
            foreach (T ex in exclude)
            {
                if (v.Equals(ex)) { isExcluded = true; break; }
            }
            if (!isExcluded) filtered.Add(v);
        }

        if (filtered.Count == 0)
        {
            Debug.LogWarning($"RandomEnumValue: No valid values left in '{typeof(T).Name}' after exclusions.");
            return default;
        }
        return filtered[UnityEngine.Random.Range(0, filtered.Count)];
    }
}