// Copyright © 2023 TradingLens. All Rights Reserved.

using CleanArchitectureSolutionTemplate.Domain;
using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;
using static CleanArchitectureSolutionTemplate.Domain.AssemblyInfoBase.HealthStatus;

namespace CleanArchitectureSolutionTemplate.Infrastructure.System;

[Singleton]
public class SystemAssemblyInfo : AssemblyInfoBase {
  protected override (HealthStatus HealthStatus, object? HealthData) CheckHealth() {
    return new(Healthy, null);
  }
}