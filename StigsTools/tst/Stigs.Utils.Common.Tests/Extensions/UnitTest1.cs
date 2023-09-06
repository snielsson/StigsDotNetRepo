// Copyright © 2023 TradingLens. All Rights Reserved.

using FluentAssertions;
using Stigs.Utils.Common.Extensions;
using Xunit;

namespace Stigs.Utils.Common.Tests.Extensions;

public class StringExtensionTests {

  [Theory]
  [ClassData(typeof(GetItemPositionTestCases))]
  public void GetItemPositionsWorks(string input, (int Start, int Length)[] expected, string? because, string? startDelimiter, string? endDelimiter) {
    input.GetItemPositions(startDelimiter,endDelimiter).ToArray().Should().Equal(expected, because);
  }

  private class GetItemPositionTestCases : TestData<string, (int Start, int Length)[], string, string?, string?> {
    public sealed override void Add(string? InputString = default, (int Start, int Length)[]? Expected = default, string? Because = default, string? StartDelimiter = default, string? EndDelimiter = default)
      => base.Add(InputString, Expected, Because ?? $"(InputString = '{InputString}')", StartDelimiter ?? "___", EndDelimiter ?? "___");

    public GetItemPositionTestCases() {
      Add("this is ___a___ test string",new [] { (8,7) });
      Add("this ___is___ another ___test___ string",new [] { (5,8),(22,10) } );
      Add("this ___is________yet_____ another ___test___ string",new [] { (5,8),(13,11),(35,10) } );
    }
  }
}