// Copyright © 2023 TradingLens. All Rights Reserved.

using Microsoft.Extensions.DependencyInjection;

namespace Stigs.Utils.Common;

public class Sys {
  private static readonly AsyncLocal<Sys> _value = new();
  private readonly IServiceProvider _serviceProvider;
  private readonly ITimeService _timeService;
  private readonly string _correlationId;
  private readonly string _userId;
  private Sys(IServiceProvider serviceProvider, ITimeService timeService, string? userId, string? correlationId) {
    _serviceProvider = serviceProvider;
    _timeService = timeService;
    _userId = userId ?? Thread.CurrentPrincipal?.Identity?.Name ?? "-"; //throw new InvalidOperationException("UserId not defined.");
    _correlationId = correlationId ?? Guid.NewGuid().ToString("N");
  }
  private const string NotInitializedMsg = "Sys.Initialize (or IserviceProvider.SysInitialize) has not been called";
  public static ITimeService TimeService => TimeServiceOrNull ?? throw new InvalidOperationException(NotInitializedMsg);
  public static ITimeService? TimeServiceOrNull => _value.Value?._timeService;
  public static string CorrelationId => _value.Value?._correlationId ?? throw new InvalidOperationException(NotInitializedMsg);
  public static string UserId => _value.Value?._userId ?? throw new InvalidOperationException(NotInitializedMsg);
  public static IServiceProvider ServiceProvider => _value.Value?._serviceProvider ?? throw new InvalidOperationException(NotInitializedMsg);
  public static Sys Initialize(IServiceProvider serviceProvider, string? userId = null, string? correlationId = null)
    => _value.Value = new Sys(serviceProvider, serviceProvider.GetRequiredService<ITimeService>(), userId, correlationId);
  public static T Resolve<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();
  public static IEnumerable<T> ResolveAll<T>() where T : notnull => ServiceProvider.GetServices<T>();
  public static T Resolve<T>(Type serviceType) => (T)ServiceProvider.GetRequiredService(serviceType);
}

public static class SysExtensions {
  public static Sys SysInitialize(this IServiceProvider @this, string? userId = null, string? correlationId = null) => Sys.Initialize(@this, userId, correlationId);
}

