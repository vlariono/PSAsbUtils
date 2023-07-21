using System.Collections;

namespace PsAsbUtils.Cmdlets;

public static class DictionaryExtension
{
    public static void AddHashtable<TKey,TValue>(this IDictionary<TKey, TValue> dictionary, Hashtable hashtable)
    {
        foreach (var key in hashtable.Keys.Cast<TKey>())
        {
            dictionary.Add(key, (TValue)hashtable[key]);
        }
    }
}
