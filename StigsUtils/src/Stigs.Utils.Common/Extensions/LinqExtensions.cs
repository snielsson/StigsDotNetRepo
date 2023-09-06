// Copyright © 2023 TradingLens. All Rights Reserved.

namespace Stigs.Utils.Common.Extensions;

public static class LinqExtensions {
  /// <summary>
  ///
  /// </summary>
  /// <param name="items"></param>
  /// <param name="keySelector"></param>
  /// <param name="throwOnDuplicate">Throw if items contains duplicate keys. Defaults to true. If false, duplicates will overwrite previous values.</param>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TKey"></typeparam>
  /// <returns></returns>
  public static SortedList<TKey, T> ToSortedList<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector, bool throwOnDuplicate = true) where TKey : notnull {
    var result = new SortedList<TKey, T>();
    foreach (var item in items) {
      var key = keySelector(item);
      if (throwOnDuplicate) {
        result.Add(key, item);
      }
      else {
        result[key] = item;
      }
    }
    return result;
  }
  public static SortedList<T, T> ToSortedList<T>(IEnumerable<T> items, bool throwOnDuplicate = true) where T : notnull => items.ToSortedList(x => x, throwOnDuplicate);

  public static HashSet<T> Add<T>(this HashSet<T> @this, IEnumerable<T> items) {
    foreach (T item in items) {
      @this.Add(item);
    }
    return @this;
  }
}