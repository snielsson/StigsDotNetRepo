// Copyright © 2023 TradingLens. All Rights Reserved.

using Xunit;

namespace StigsTool.Core.Tests.Features.TemplateEngine;

public class TemplateEngineTests {
  [Fact(Skip = "temp")]
  public void TemplateEngineWorks() {
    StigsTool.Core.Features.TemplateEngine.TemplateEngine.Execute("xUnitProjectTemplate");
  }
}