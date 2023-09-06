// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Text.Json;

namespace Stigs.Utils.Common.Extensions;

public static class ObjectExtensions {
  public static bool UseSystemJsonByDefault { get; set; } = true;
  public static string ToJson(this object @this, bool indented = true, bool? useSystemJson = null)
    => useSystemJson??UseSystemJsonByDefault ? SystemJsonExtensions.ToJson(@this, indented):throw new Exception("System.Json alternative not implemented yet.");

  public static string ToJson(this object @this, JsonSerializerOptions options) => SystemJsonExtensions.ToJson(@this, options);

}