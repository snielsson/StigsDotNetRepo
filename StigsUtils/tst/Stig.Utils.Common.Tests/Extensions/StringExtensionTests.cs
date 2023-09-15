// Copyright © 2023 Stig Schmidt Nielsson. All Rights Reserved. This file is open source and distributed under the MIT license.

using FluentAssertions;
using Stig.Utils.Common.Extensions;
using Xunit;

namespace Stig.Utils.Common.Tests.Extensions;

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

  [Theory]
  [InlineData("abcde",null,null,"abcde")]
  [InlineData("abcde","ab",null,"cde")]
  [InlineData("abcde",null,"de","abc")]
  [InlineData("abcde","ab","de","c")]
  [InlineData("abcde","abx","de","abc")]
  [InlineData("abcde","abx","de","abc",false)]
  [InlineData("abcde","abx","de","c",true)]
  [InlineData("abcde","ab","dex","cde")]
  [InlineData("abcde","ab","xde","cde",null,false)]
  [InlineData("abcde","ab","xde","c",null,true)]
  [InlineData("abcde","abc","cde","de",null,null)]
  [InlineData("abcde","abc","cde","",null,true)]
  [InlineData("abcde","abcdef",null,"abcde")]
  [InlineData("abcde","abcdef",null,"",true)]
  public void TrimStringWorks(string input, string? trimStart, string? trimEnd, string expected, bool? allowPartialTrimStart = null, bool? allowPartialTrimEnd = null) {
    input.TrimString(trimStart, trimEnd, allowPartialTrimStart, allowPartialTrimEnd).Should().Be(expected);
  }
}