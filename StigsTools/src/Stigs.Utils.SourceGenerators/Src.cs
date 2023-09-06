// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Reflection;
using System.Text;

namespace Stigs.Utils.SourceGenerators;

public static partial class Src {
  public static string Read(string path, Assembly? assembly = null, Encoding? encoding = null) {
    assembly ??= Assembly.GetExecutingAssembly();
    var ns = "Stigs.Utils.SourceGenerators.Templates.";
    if (!path.StartsWith(ns)) path = ns + path;

    //Stigs.Utils.SourceGenerators.Templates.
    using var stream = assembly.GetManifestResourceStream(path) ?? throw new ArgumentException();
    using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
    return reader.ReadToEnd();
  }

}