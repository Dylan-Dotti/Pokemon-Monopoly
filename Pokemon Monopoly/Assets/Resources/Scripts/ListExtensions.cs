using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Randomize<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    public static List<T> Randomized<T>(this IEnumerable<T> sequence)
    {
        List<T> newList = new List<T>(sequence);
        newList.Randomize(); 
        return newList;
    }

    public static int IndexOf<T>(this IReadOnlyList<T> list, T item)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(list[i], item))
            {
                return i;
            }
        }
        return -1;
    }

    public static void ForEach<T>(this IEnumerable<T> items, System.Action<T> action)
    {
        foreach (T item in items)
        {
            action(item);
        }
    }
}
