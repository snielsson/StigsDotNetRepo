// Copyright © 2023 TradingLens. All Rights Reserved.

namespace CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;

/// <summary>
/// The ConfigKey attribute is used when making dependency registrations using assembly scanning.
///
/// Any class marked with the ConfigKey attribute is registered using ServiceCollection.Configure
/// which makes the class available via. the option pattern (IOption\<T\>, IOptionSnapshot\<T\>, IOptionMonitor\<T\>).
///
/// The mandatory config key is used to bind the instance to the configuration.
///
/// </summary>
public class ConfigKeyAttribute : DependencyInjectionAttributeBase {
  public string ConfigurationKey { get; }
  public string? Name { get; set; }
  public ConfigKeyAttribute(string configurationKey) {
    ConfigurationKey = configurationKey;
  }
}

