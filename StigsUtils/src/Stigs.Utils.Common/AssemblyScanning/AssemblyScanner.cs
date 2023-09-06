// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Collections;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Stigs.Utils.Common.Extensions;

namespace Stigs.Utils.Common.AssemblyScanning;

public class AssemblyScanner : IEnumerable<Assembly> {
  private readonly ILogger<AssemblyScanner> _logger;
  public AssemblyScanner(ILogger<AssemblyScanner> logger) {
    _logger = logger;

  }
  private SortedSet<AssemblyLoadContext> _assemblyLoadContexts = new() { AssemblyLoadContext.Default };
  private HashSet<Assembly> _assemblies = new();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  public IEnumerator<Assembly> GetEnumerator() => _assemblies.GetEnumerator();

  public IReadOnlySet<Assembly> GetReferencedAssembliesStartingWith(string startOfAssemblyName) => GetReferencedAssembliesStartingWith(startOfAssemblyName, Assembly.GetCallingAssembly());
  public IReadOnlySet<Assembly> GetReferencedAssembliesStartingWith(string startOfAssemblyName, params Assembly[] referencingAssemblies) => GetReferencedAssemblies(x => x.Name?.StartsWith(startOfAssemblyName)??false, null, null, referencingAssemblies);
  public IReadOnlySet<Assembly> GetReferencedAssemblies(params Assembly[] referencingAssemblies) => GetReferencedAssemblies(null,null,null,referencingAssemblies);
  public IReadOnlySet<Assembly> GetReferencedAssemblies(Func<AssemblyName, bool>? includeFilter = null,Func<AssemblyName,bool>? excludeFilter = null, bool? loadUnloadedAssemblies = null,params Assembly[] referencingAssemblies) => GetReferencedAssemblies(referencingAssemblies.ToSortedList(x => x.GetName().Name??""),includeFilter,excludeFilter,loadUnloadedAssemblies);
  public IReadOnlySet<Assembly> GetReferencedAssemblies(SortedList<string,Assembly> referencingAssemblies, Func<AssemblyName, bool>? includeFilter = null,Func<AssemblyName,bool>? excludeFilter = null, bool? loadUnloadedAssemblies = null) {
    includeFilter ??= Filter<AssemblyName>.DefaultIncludeFilter;
    excludeFilter ??= Filter<AssemblyName>.DefaultExcludeFilter;
    loadUnloadedAssemblies ??= true;
    HashSet<Assembly> loadedAssemblies = new();
    HashSet<AssemblyName> loadedAssemblyNames = new();
    foreach (Assembly referencingAssembly in referencingAssemblies.Values) {
      DependencyContext? dependencyContext = DependencyContext.Load(referencingAssembly);
      if (dependencyContext == null) {
        _logger.LogInformation("DependencyContext.Load(...) returned null. Skipping {ReferencingAssembly}", referencingAssemblies);
        continue;
      }

      var location = referencingAssembly.Location;
      var rt = dependencyContext.RuntimeLibraries.ToArray();

      AssemblyLoadContext loadContext = AssemblyLoadContext.GetLoadContext(referencingAssembly) ?? AssemblyLoadContext.Default ?? new AssemblyLoadContext(referencingAssembly.ToString());
      loadedAssemblies.Add(loadContext.Assemblies);

      if (loadUnloadedAssemblies.Value) {
        AssemblyName[] unloadedAssemblyNames = dependencyContext
                                              .RuntimeLibraries
                                              .Select(x => new AssemblyName(x.Name))
                                              .Where(x => includeFilter(x) || !excludeFilter(x))
                                              .Where(x => !loadedAssemblyNames.Contains(x))
                                              .ToArray();
        foreach (AssemblyName assemblyName in unloadedAssemblyNames) {
          loadedAssemblyNames.Add(assemblyName);
          var assembly = loadContext.LoadFromAssemblyName(assemblyName);
          loadedAssemblies.Add(assembly);
          _logger.LogInformation("Loaded {Assembly} referenced by {ReferencingAssembly}", assembly, referencingAssemblies);
        }
      }
    }
    return loadedAssemblies;
  }
}