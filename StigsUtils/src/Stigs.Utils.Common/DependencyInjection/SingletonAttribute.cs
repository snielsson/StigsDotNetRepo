// Copyright © 2023 TradingLens. All Rights Reserved.

namespace Stigs.Utils.Common.DependencyInjection;

/// <summary>
/// Marks the class to be included in dependency injection registration by assembly scanning using the logic for the singleton lifetime.
/// </summary>
public class SingletonAttribute : DependencyInjectionAttributeBase { }