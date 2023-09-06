// Copyright © 2023 TradingLens. All Rights Reserved.

using Stigs.Utils.Common.DependencyInjection;

namespace Stigs.Utils.Common;

public interface ITimeService {
  DateTime UtcNow { get; }
}

[Singleton]
internal sealed class TimeService : ITimeService {
  private DateTime? _utcNow;
  public DateTime UtcNow {
    get => _utcNow ?? DateTime.UtcNow;
    internal set => _utcNow = value;
  }
}