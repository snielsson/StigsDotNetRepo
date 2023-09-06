// Copyright © 2023 TradingLens. All Rights Reserved.
namespace Stigs.Utils.SourceGenerators;

public static partial class Src {

  //language=c#
  public static string DependencyAttributes => """
    // Generated code. Any manual modifications will be overwritten!
    [AttributeUsage(AttributeTargets.Class)]
    public partial class TransientAttribute : Attribute{}

    [AttributeUsage(AttributeTargets.Class)]
    public partial class ScopedAttribute : Attribute{}

    [AttributeUsage(AttributeTargets.Class)]
    public partial class SingletonAttribute : Attribute{}
    """;

}