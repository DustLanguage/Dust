#region

using System;
using System.Collections.Generic;

#endregion

namespace Dust.Extensions
{
  // ReSharper disable once InconsistentNaming
  public static class IEnumerableExtensions
  {
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      HashSet<TKey> seenKeys = new HashSet<TKey>();

      foreach (TSource element in source)
        if (seenKeys.Add(keySelector(element)))
          yield return element;
    }
  }
}