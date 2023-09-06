// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Text.RegularExpressions;
using Cysharp.Text;

namespace Stigs.Utils.Common.Extensions;

public static class StringExtensions {
  public static string Until(this string @this, char ch, bool includeMatchingChar = false) {
    var index = @this.IndexOf(ch);
    return index == -1 ? @this : @this.Substring(0, index + (includeMatchingChar ? 1 : 0));
  }
  public static string UntilAndIncluding(this string @this, char ch) => @this.Until(ch, true);
  public static IEnumerable<string> Matching(this IEnumerable<string> @this, string pattern) => @this.Matching(new Regex(pattern));
  public static IEnumerable<string> Matching(this IEnumerable<string> @this, Regex regex) {
    foreach (var x in @this) {
      if (regex.IsMatch(x)) yield return x;
    }
  }



  private static readonly (int Start, int Length) NoItem = (-1, 0);
  public static (int Start, int Length) NextItem(this ReadOnlySpan<char> @this, int pos, ReadOnlySpan<char> startDelimiter, ReadOnlySpan<char> endDelimiter) {
    if (pos < 0) throw new ArgumentOutOfRangeException(nameof(pos), $"Negative start index {pos}.");
    if (pos >= @this.Length) throw new ArgumentOutOfRangeException(nameof(pos), $"Start index {pos} outside length of {@this.Length}.");
    int startReturnValue = -1;
    var delimiter = startDelimiter;
    if (startDelimiter.Length == 0) {
      if (endDelimiter.Length == 0) throw new ArgumentOutOfRangeException(nameof(pos), "Start and end delimiters are both empty.");
      startReturnValue = 0;
      delimiter = endDelimiter;
    }
    int delimiterPos = 0;
    while (pos < @this.Length) {
      delimiterPos = @this[pos++] == delimiter[delimiterPos] ? delimiterPos+1 : 0;
      if (delimiterPos == delimiter.Length) {
        if (startReturnValue > -1) return (startReturnValue, pos - startReturnValue);
        startReturnValue = pos - delimiter.Length;
        if (endDelimiter.Length == 0) return (startReturnValue, @this.Length - startReturnValue);
        delimiter = endDelimiter;
        delimiterPos = 0;
      }
    }
    return NoItem;
  }

  public static IReadOnlyList<(int Start, int Length)> GetItemPositions(this string @this, ReadOnlySpan<char> startDelimiter = default, int startPosition = 0) =>
    ((ReadOnlySpan<char>)@this).GetItemPositions(startDelimiter==default?"___":startDelimiter, startDelimiter==default?"___":startDelimiter, startPosition);
  public static IReadOnlyList<(int Start, int Length)> GetItemPositions(this string @this, ReadOnlySpan<char> startDelimiter, ReadOnlySpan<char> endDelimiter, int startPosition = 0) =>
    ((ReadOnlySpan<char>)@this).GetItemPositions(startDelimiter, endDelimiter, startPosition);
  public static IReadOnlyList<(int Start, int Length)> GetItemPositions(this ReadOnlySpan<char> @this, ReadOnlySpan<char> startDelimiter, ReadOnlySpan<char> endDelimiter, int startPosition = 0) {
    var result = new List<(int, int)>();
    while(startPosition < @this.Length) {
      var itemPosition = @this.NextItem(startPosition, startDelimiter, endDelimiter);
      if(itemPosition.Start == -1) break;
      result.Add(itemPosition);
      startPosition = itemPosition.Start + itemPosition.Length;
    }
    return result;
  }


  public static ReadOnlySpan<char> Substitute(this ReadOnlySpan<char> @this, IEnumerable<KeyValuePair<string, string>> symbols, string startDelimiter, string endDelimiter) {
    var sb = ZString.CreateUtf8StringBuilder(true);
    foreach (var symbol in symbols) {
    }
    return @this;
  }

}