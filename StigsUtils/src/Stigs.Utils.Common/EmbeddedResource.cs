// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Reflection;
using System.Text;
using System.Text.Json;
using Stigs.Utils.Common.Extensions;

namespace Stigs.Utils.Common;

public static class EmbeddedResource {
  public static string? Find(string resourcePath, Assembly? assembly = null, bool requireExactPath = true, bool throwIfNotFound = false)
  {
    assembly ??= Assembly.GetCallingAssembly();
    var assemblyName = assembly.GetName().Name!;
    var fullPath = resourcePath;
    if (!fullPath.StartsWith(assemblyName))
      fullPath = $"{assemblyName}.{resourcePath.TrimStart('.')}";
    string[] resources = assembly.GetManifestResourceNames();
    if (resources.Contains(fullPath))
      return fullPath;
    if (requireExactPath) {
      return throwIfNotFound ? throw new ArgumentException($"Embedded resource '{fullPath}' in {assembly} not found.") : null;
    }
    string[] matches = resources.Where(x => x.Contains(resourcePath)).ToArray();
    return matches.Length switch
    {
      0 => throwIfNotFound ? throw new ArgumentException($"Embedded resource '{fullPath}' in {assembly} not found.") : null,
      1 => matches[0],
      _ => throw new InvalidOperationException($"More than 1 embedded resource name matches '{resourcePath}' in '{assembly}' :\n{matches.ToJson()}")
    };
  }

  public static string FindRequired(string resourcePath, Assembly? assembly = null, bool requireExactPath = true) => Find(resourcePath, assembly, requireExactPath, true)!;

  public static Stream? GetManifestResourceStream(string? path, Assembly assembly) =>
    path == null ? null : assembly.GetManifestResourceStream(path) ?? throw new InvalidOperationException($"Could not get embedded resource '{path}' from {assembly}.");

  public static Stream? OpenFromEmbeddedResourceAsStream(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false, bool throwIfNotFound = false)
  {
    assembly ??= Assembly.GetCallingAssembly();
    string? path = Find(resourcePath, assembly, requireExactPath, throwIfNotFound);
    return GetManifestResourceStream(path, assembly);
  }

  public static StreamReader? OpenFromEmbeddedResourceAsStreamReader(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false, bool throwIfNotFound = false,Encoding? encoding = null) {
    var stream = resourcePath.OpenFromEmbeddedResourceAsStream(assembly, requireExactPath,throwIfNotFound);
    return stream == null ? null : new StreamReader(stream,encoding ?? DefaultEncoding);
  }

  public static string? Read(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false, bool throwIfNotFound = false, Encoding? encoding = null)
  {
    using Stream? stream = resourcePath.OpenFromEmbeddedResourceAsStream(assembly ?? Assembly.GetCallingAssembly(), requireExactPath, throwIfNotFound);
    if (stream != null) {
      using var reader = new StreamReader(stream,encoding??DefaultEncoding);
      return reader.ReadToEnd();
    }
    return null;
  }

  public static string ReadEmbeddedResource(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false, Encoding? encoding = null)
    => (assembly??Assembly.GetExecutingAssembly()).ReadRequired(resourcePath, requireExactPath, encoding);

  public static string ReadRequired(this Assembly assembly, string resourcePath, bool requireExactPath = false, Encoding? encoding = null)
    => resourcePath.Read(assembly, requireExactPath, true, encoding)!;

  public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

  public static JsonDocument? ReadJsonDocument(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false, bool throwIfNotFound = false)
  {
    using Stream? stream = resourcePath.OpenFromEmbeddedResourceAsStream(assembly ?? Assembly.GetCallingAssembly(), requireExactPath, throwIfNotFound);
    return stream == null ? null : JsonDocument.Parse(stream);
  }

  public static JsonDocument ReadJsonDocumentRequired(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false) {
    return resourcePath.ReadJsonDocument(assembly, requireExactPath, true)!;
  }

  public static T? JsonDeserializeEmbeddedResource<T>(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false, bool throwIfNotFound = false) {
    using Stream? stream = resourcePath.OpenFromEmbeddedResourceAsStream(assembly ?? Assembly.GetCallingAssembly(), requireExactPath, throwIfNotFound);
    return stream == null ? default : stream.FromJson<T>();
  }

  public static T? JsonDeserializeEmbeddedResource<T>(this Assembly assembly, string resourcePath, bool requireExactPath = false, bool throwIfNotFound = false) {
    using Stream? stream = resourcePath.OpenFromEmbeddedResourceAsStream(assembly, requireExactPath, throwIfNotFound);
    return stream == null ? default : stream.FromJson<T>();
  }

  public static T JsonDeserializEmbeddedResourceRequired<T>(this string resourcePath, Assembly? assembly = null, bool requireExactPath = false)
    => resourcePath.JsonDeserializeEmbeddedResource<T>(assembly, requireExactPath, true)!;

  public static T JsonDeserializEmbeddedResourceRequired<T>(this Assembly assembly, string resourcePath, bool requireExactPath = false)
    => resourcePath.JsonDeserializeEmbeddedResource<T>(assembly, requireExactPath, true)!;
}