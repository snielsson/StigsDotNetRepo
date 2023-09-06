// Copyright © 2023 TradingLens. All Rights Reserved.

namespace Stigs.Utils.Common;

public class Filter<T> {
  public static Func<T, bool> DefaultIncludeFilter { get; } = _ => true;
  public static Func<T, bool> DefaultExcludeFilter { get; } = _ => false;
}