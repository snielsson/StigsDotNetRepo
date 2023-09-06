// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Reflection;
using Stigs.Utils.Common;
using Stigs.Utils.Common.Extensions;

namespace StigsTool.Core.Features.TemplateEngine;

public sealed class TemplateEngine {
  private Dictionary<string, string> _parameters = new();
  public static TemplateEngine Execute(string templateName, string argumentsJson = "{}", Assembly? assembly = null, string templateFolder = "Templates") =>
    Execute(templateName, argumentsJson.FromJson<IReadOnlyDictionary<string, string>>(), assembly, templateFolder);
  public static TemplateEngine Execute(string templateName, IReadOnlyDictionary<string, string>? arguments, Assembly? assembly = null, string templateFolder = "Templates") {
    assembly ??= Assembly.GetExecutingAssembly();
    var symbols = BindParameters(arguments, assembly);
    var files = assembly.GetManifestResourceNames().Matching(@$"\.{templateFolder}$");

    // var resources = assembly.GetManifestResourceNames();
    // var templateFiles = resources.Where(x => x.Contains($".{templateFolder}.{templateName}."));
    // var parametersJson = assembly.GetManifestResourceStream()
    //
    // var arguments = System.Text.Json.JsonDocument.Parse()
    //
    //
    return new TemplateEngine();

    //StigsTool.Core.Features.TemplateEngine.Templates.xUnitProjectTemplate.__ProjectFile__.csproj
  }
  private static Dictionary<string, string?>? BindParameters(IReadOnlyDictionary<string, string>? arguments, Assembly assembly) {
    Dictionary<string, string?> parameters = assembly.JsonDeserializeEmbeddedResource<Dictionary<string, string?>>(".template.Parameters.json") ?? new Dictionary<string, string?>();
    foreach (var (key, val) in parameters) {
      if (arguments != null) {
        if (arguments.TryGetValue(key, out var arg)) {
          parameters[key] = arg ?? throw new ArgumentNullException($"Argument '{key}' is null.");
          continue;
        }
        if (val == null) throw new ArgumentNullException($"No value provided for parameter '{key}'.");
      }
    }
    return parameters;
  }
}