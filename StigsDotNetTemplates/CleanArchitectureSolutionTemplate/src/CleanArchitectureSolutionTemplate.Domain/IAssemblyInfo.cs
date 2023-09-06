// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Reflection;
using NullReferenceException = System.NullReferenceException;

namespace CleanArchitectureSolutionTemplate.Domain;

public interface IAssemblyInfo {
  public string AssemblyName { get; }
  public string Name { get; }
  public string Version { get;  }
  public object Health { get; }
}

public abstract class AssemblyInfoBase : IAssemblyInfo
{
  public enum HealthStatus {
    Unknown,
    Healthy,
    Unhealthy,
    Degraded
  }

  protected AssemblyInfoBase() {
    AssemblyName assemblyName = GetType().Assembly.GetName()??throw new NullReferenceException("AssemblyName");
    AssemblyName = assemblyName.FullName;
    Name = assemblyName.Name??"name ???";
    Version = assemblyName.Version?.ToString()??"version ???";
  }
  public string AssemblyName { get; }
  public string Name { get; }
  public string Version { get; }
  public object Health => CheckHealth();
  protected abstract (HealthStatus HealthStatus, object? HealthData) CheckHealth();
}