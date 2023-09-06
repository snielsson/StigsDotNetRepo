// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Stigs.Utils.SourceGenerators;

// [Generator]
// public class DependencyInjectionGenerator : IIncrementalGenerator {
//   public void Initialize(IncrementalGeneratorInitializationContext context) {
//
//     Dictionary<string, string> symbols = new Dictionary<string, string>();
//     var dependencyInjectionAttributesSource = Templates.DependencyInjectionAttributes.Substitute(symbols);
//     context.RegisterPostInitializationOutput(
//       ctx => ctx.AddSource("DependencyInjectionAttributes.g.cs", SourceText.From(dependencyInjectionAttributesSource, Encoding.UTF8)));
//
//   }
// }