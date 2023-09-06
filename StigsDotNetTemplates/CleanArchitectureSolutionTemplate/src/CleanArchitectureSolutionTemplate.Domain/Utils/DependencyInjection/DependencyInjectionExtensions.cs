// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;

public static class DependencyInjectionExtensions {

  public static WebApplicationBuilder AssemblyScan(
    this WebApplicationBuilder @this,
    Func<string?, bool>? includeFilter = null,
    Func<string?, bool>? excludeFilter = null,
    IEnumerable<AssemblyLoadContext>? assemblyLoadContexts = null) => @this.AssemblyScan(null, includeFilter, excludeFilter, assemblyLoadContexts);

  /// <summary>
  ///   Register dependencies by assembly scanning.
  ///   Scans all referenced assemblies filtered by include and exclude filter to get classes marked
  ///   with the Singleton, Scoped or Transient attribute.
  ///   All interfaces implemented by the class is are registered as dependencies, and for singletons
  ///   all interfaces will resolve to the same instance.
  ///   By default all assemblies with names starting with "System." or "Microsoft." are excluded from scanning.
  /// </summary>
  public static WebApplicationBuilder AssemblyScan(
    this WebApplicationBuilder @this,
    List<(Type,Type,string)>? scannedDependencies,
    Func<string?, bool>? includeFilter = null,
    Func<string?, bool>? excludeFilter = null,
    IEnumerable<AssemblyLoadContext>? assemblyLoadContexts = null) {
    assemblyLoadContexts ??= new[] { AssemblyLoadContext.Default };
    excludeFilter ??= x => x != null && (x.StartsWith("System.") || x.StartsWith("Microsoft."));
    includeFilter ??= _ => false;
      IDictionary<string, Assembly> loadedAssemblies =
        assemblyLoadContexts
         .SelectMany(x => x.Assemblies)
         .ToDictionary(x => x.FullName!);

    var assemblyPathResolver = new AssemblyDependencyResolver(Assembly.GetEntryAssembly()!.Location);
    HashSet<string> notLoadedAssemblies =
      DependencyContext.Default!.RuntimeLibraries
                       .Where(x => includeFilter(x.Name) || !excludeFilter(x.Name))
                       .Select(x => new AssemblyName(x.Name))
                       .Where(x => includeFilter(x.FullName) || !excludeFilter(x.FullName))
                       .Where(x => !loadedAssemblies.ContainsKey(x.FullName))
                       .Select(x => assemblyPathResolver.ResolveAssemblyToPath(x))
                       .Where(x => x != null && File.Exists(x))
                       .ToHashSet()!;

    foreach (var notLoadedAssembly in notLoadedAssemblies) {
      loadedAssemblies.Add(notLoadedAssembly, Assembly.LoadFrom(notLoadedAssembly));
    }

    IDictionary<Type, DependencyInjectionAttributeBase[]> classesWithAttributes =
      loadedAssemblies.Values
                      .Where(x => includeFilter(x.FullName) || !excludeFilter(x.FullName))
                      .SelectMany(x => x.GetTypes())
                      .Where(type => type.IsClass)
                      .Where(type => type.GetCustomAttributes<DependencyInjectionAttributeBase>().Any())
                      .Distinct()
                      .ToDictionary(t => t, t => t.GetCustomAttributes<DependencyInjectionAttributeBase>().ToArray());
    foreach ((Type key, DependencyInjectionAttributeBase[] attributes) in classesWithAttributes) {
      @this.Services.Add(key, @this.Configuration, attributes, scannedDependencies);
    }
    return @this;
  }


  /// <summary>
  ///   Make registrations:
  ///   Classes annotated with a [Singleton], [Scoped] or [Transient] attribute are registered in service collection
  ///   using all interfaces implemented by the class as ServiceTypes and the class as implementation type. The lifetime
  ///   corresponding to the attribute name is used and for singletons all interfaces will be registered to the same instance.
  ///
  ///   Classes annotated with a [Config] attributes are registered using IServiceCollection.Configure so that they
  ///   can be resolved using the Options pattern interfaces like
  ///   IOptions\<T\>, IOptionsSnapshot\<T\> and IOptionsMonitor\<T\>.
  /// </summary>
  private static IServiceCollection Add(this IServiceCollection @this, Type implementationType, ConfigurationManager configurationManager, DependencyInjectionAttributeBase[] attributes, List<(Type serviceType, Type implementationType, String Description)>? scannedDependencies) {
    if(attributes.Length > 1) throw new InvalidOperationException($"Multiple dependency injection attributes on {implementationType}. Only one is allowed.");
    var interfaces = implementationType.GetInterfaces();
    switch (attributes.Single()) {
      case SingletonAttribute:
        @this.Add(new ServiceDescriptor(implementationType, implementationType, ServiceLifetime.Singleton));
        scannedDependencies?.Add((implementationType,implementationType, "Singleton"));
        foreach (Type serviceType in interfaces) {
          @this.Add(new ServiceDescriptor(serviceType, x => x.GetRequiredService(implementationType), ServiceLifetime.Singleton));
          scannedDependencies?.Add((serviceType,implementationType, "Singleton"));
        }
        break;
      case ScopedAttribute:
        foreach (Type serviceType in interfaces) {
          @this.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));
          scannedDependencies?.Add((serviceType,implementationType,"Scoped"));
        }
        break;
      case TransientAttribute:
        foreach (Type serviceType in interfaces) {
          @this.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
          scannedDependencies?.Add((serviceType,implementationType,"Transient"));
        }
        break;
      case ConfigKeyAttribute configKeyAttribute:
          @this.Configure(implementationType, configKeyAttribute, configurationManager);
          scannedDependencies?.Add((implementationType,implementationType,"Configure (options pattern)"));
          break;
      default:
        throw new ArgumentException($"Unsupported attribute type: {attributes.Single().GetType()}");
    }
    return @this;
  }

  private static bool HasParameterTypes<T1, T2, T3>(this MethodInfo @this) => @this.GetParameters().Matches<T1, T2, T3>();
  private static bool Matches<T1, T2, T3>(this ParameterInfo[] @this) {
    if (@this.Length != 3) return false;
    if (@this[0].ParameterType != typeof(T1)) return false;
    if (@this[1].ParameterType != typeof(T2)) return false;
    if (@this[2].ParameterType != typeof(T3)) return false;
    return true;
  }

  /// <summary>
  /// Reflection based invocation of generic the method OptionsConfigurationServiceCollectionExtensions.Configure\<TOptions\>(IServiceCollection,string?,IConfiguration)
  /// </summary>
  /// <param name="services"></param>
  /// <param name="optionsType"></param>
  /// <param name="configKeyAttribute"></param>
  /// <param name="config"></param>
  private static IServiceCollection Configure(this IServiceCollection services, Type optionsType, ConfigKeyAttribute configKeyAttribute, IConfiguration config) =>
    (IServiceCollection)typeof(OptionsConfigurationServiceCollectionExtensions)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .First(m => m.Name == "Configure" && m.HasParameterTypes<IServiceCollection, string?, IConfiguration>())
                        .MakeGenericMethod(optionsType)
                        .Invoke(null, new object[] { services, configKeyAttribute.Name ?? string.Empty, config.GetSection(configKeyAttribute.ConfigurationKey) })!;
}