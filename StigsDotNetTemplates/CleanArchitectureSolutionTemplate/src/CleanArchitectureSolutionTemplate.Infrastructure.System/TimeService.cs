// Copyright © 2023 TradingLens. All Rights Reserved.

using CleanArchitectureSolutionTemplate.Domain;
using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;

namespace CleanArchitectureSolutionTemplate.Infrastructure.System;

[Singleton]
public class TimeService : ITimeService {
  private DateTime? _utcNow;
  public DateTime UtcNow {
    get => _utcNow ?? DateTime.UtcNow;
    internal set => _utcNow = value;
  }
}