// // Copyright © 2023 TradingLens. All Rights Reserved.
//
// using Microsoft.Extensions.Configuration;
//
// namespace CleanArchitectureSolutionTemplate.Domain.Utils;
//
// public static class Extensions {
//   public static T? GetOrDefault<T>(this IConfiguration configuration, string key, Func<string, T> parser, T? defaultValue = default) {
//     var value = configuration[key];
//     return value == null ? defaultValue : parser(value);
//   }
//   public static TimeSpan GetOrDefault(this IConfiguration configuration, string key, TimeSpan defaultValue = default) =>
//     configuration.GetOrDefault(key, TimeSpan.Parse, defaultValue);
//
//   public static bool StartsWith(this string x, params string[] prefixes) {
//     foreach (var prefix in prefixes) {
//       if (x.StartsWith(prefix)) return true;
//     }
//     return false;
//   }
//
//   public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> @this) => @this.Where(x => x != null).Select(x => x!);
//   public static IEnumerable<TResult> SelectNotNull<T, TResult>(this IEnumerable<T> @this, Func<T, TResult> selector) => @this.Select(selector).Where(x => x != null);
//
//   public static IEnumerable<T>? NullIfEmpty<T>(this IEnumerable<T> @this) {
//     IEnumerable<T> nullIfEmpty = @this as T[] ?? @this.ToArray();
//     return nullIfEmpty.Any() ? nullIfEmpty : null;
//   }
//
//   public static IEnumerable<T>? NullIfEmpty<T>(this IEnumerable<T> @this, Func<T, bool> predicate) {
//     IEnumerable<T> nullIfEmpty = @this as T[] ?? @this.ToArray();
//     return nullIfEmpty.Any(predicate) ? nullIfEmpty : null;
//   }
//
//   public static T ThrowIfDefault<T>(this T @this, string? message = null) {
//     if (@this == null) throw new NullReferenceException(message);
//     if (@this.Equals(default)) throw new InvalidOperationException(message);
//     return @this;
//   }
//
//   public static IEnumerable<T> ThrowIf<T>(this IEnumerable<T> @this, Func<IEnumerable<T>, bool> predicate, string? message = null) {
//     IEnumerable<T> throwIf = @this as T[] ?? @this.ToArray();
//     if (predicate(throwIf)) throw new InvalidOperationException(message);
//     return throwIf;
//   }
//
//   public static IDictionary<TKey, TVal> ToDictionaryWhereValueNotNull<TKey, TVal>(this IEnumerable<TVal> @this, Func<TVal, TKey?> keySelector) where TKey : notnull => @this.ToDictionaryWhereValueNotNull(keySelector, x => x);
//   public static IDictionary<TKey, TVal> ToDictionaryWhereValueNotNull<T, TKey, TVal>(this IEnumerable<T> @this, Func<T, TKey?> keySelector, Func<T, TVal?> valueSelector) where TKey : notnull {
//     var dictionary = new Dictionary<TKey, TVal>();
//     foreach (T x in @this) {
//       var key = keySelector(x);
//       if (key == null) continue;
//       var val = valueSelector(x);
//       if (val == null) continue;
//       dictionary.Add(key, val);
//     }
//     return dictionary;
//   }
// s
// }