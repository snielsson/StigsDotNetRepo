// Copyright © 2023 Stig Schmidt Nielsson. All Rights Reserved. This file is open source and distributed under the MIT license.

using Stig.Utils.Xunit;
using Xunit.Abstractions;

namespace Stig.Utils.Parsers.Tests;

public class CSharpParserTests : TestBase {
  public CSharpParserTests(ITestOutputHelper output) : base(output) { }

  //   [Fact]
  //   public void CSharpParserWorks() {
  //     //language=c#
  //     var src = """
  //       using Stigs.Utils.Xunit;
  //       using Xunit.Abstractions;
  //
  //       namespace Stigs.Utils.Parsers.Tests;
  //
  //       public class CSharpParserTests : TestBase {
  //         public CSharpParserTests(ITestOutputHelper output) : base(output) { }
  //       }
  //     """;
  //     src.ParseAsCSharp();
  //   }
}