using System;
using System.Collections.Generic;

namespace Dust.Extensions
{
  // ReSharper disable once InconsistentNaming - cannot add this to abbreviations list from some reason
  public static class IEnumerableExtensions
  {
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
      where TKey : IComparable
    {
      HashSet<TKey> seenKeys = new HashSet<TKey>();
      List<TSource> elements = new List<TSource>();

      foreach (TSource element in source)
      {
        if (seenKeys.Add(keySelector(element)))
        {
          elements.Add(element);
        }
        else
        {
          elements.RemoveAll((e) => keySelector(e).Equals(keySelector(element)));
        }
      }

      return elements;
    }
  }
}