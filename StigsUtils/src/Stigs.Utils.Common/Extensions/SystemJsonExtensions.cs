// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Text.Json;
using System.Text.Json.Serialization;
using JsonCons.JsonPath;

namespace Stigs.Utils.Common.Extensions;

public static class SystemJsonExtensions
{
  public static JsonSerializerOptions DefaultIndentedSerializerOptions { get; set; } = new() {
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    ReadCommentHandling = JsonCommentHandling.Skip,
    MaxDepth = 100,
    AllowTrailingCommas = true
  };
  public static JsonSerializerOptions DefaultSerializerOptions { get; set; } = new() {
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    ReadCommentHandling = JsonCommentHandling.Skip,
    MaxDepth = 100,
    AllowTrailingCommas = true
  };
  public static string ToJson(object x, bool indented = true) => JsonSerializer.Serialize(x, indented ? DefaultIndentedSerializerOptions : DefaultSerializerOptions);
  public static string ToJson(object x, JsonSerializerOptions options) => JsonSerializer.Serialize(x, options);

  public static T? FromJson<T>(this string? @this, JsonSerializerOptions? options = null) {
    if (@this == null) return default;
    options ??= DefaultSerializerOptions;
    return JsonSerializer.Deserialize<T>(@this, options);
  }

  public static object? FromJson(this string? @this, JsonSerializerOptions? options = null) {
    if (@this == null) return default;
    options ??= DefaultSerializerOptions;
    return JsonSerializer.Deserialize<object>(@this, options);
  }

  public static T? FromJson<T>(this Stream? @this, JsonSerializerOptions? options = null) {
    if (@this == null) return default;
    options ??= DefaultSerializerOptions;
    return JsonSerializer.Deserialize<T>(@this, options);
  }


}