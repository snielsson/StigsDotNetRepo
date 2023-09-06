// Copyright © 2023 TradingLens. All Rights Reserved.

using CleanArchitectureSolutionTemplate.Domain;
using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;
using static CleanArchitectureSolutionTemplate.Domain.AssemblyInfoBase.HealthStatus;

namespace CleanArchitectureSolutionTemplate.Presentation;

[Singleton]
public class PresentationAssemblyInfo : AssemblyInfoBase {
  protected override (HealthStatus HealthStatus, object? HealthData) CheckHealth() => (Healthy, null);
}