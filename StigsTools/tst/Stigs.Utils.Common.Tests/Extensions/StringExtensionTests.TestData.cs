// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Collections;

namespace Stigs.Utils.Common.Tests.Extensions;

public abstract class TestDataBase : IEnumerable<object[]> {
  private readonly List<object?[]> _data = new();
  public virtual TestDataBase AddRow(params object?[] values) {
    _data.Add(values);
    return this;
  }
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
}

public abstract class TestData<T1, T2, T3, T4, T5> : TestDataBase {
  public virtual void Add(T1? t1 = default, T2? t2 = default, T3? t3 = default, T4? t4 = default, T5? t5 = default) {
    AddRow(t1, t2, t3, t4, t5);
  }
  public virtual TestData<T1, T2, T3, T4, T5> With(T1? t1 = default, T2? t2 = default, T3? t3 = default, T4? t4 = default, T5? t5 = default) {
    Add(t1, t2, t3, t4, t5);
    return this;
  }
  protected virtual TestData<T1, T2, T3, T4, T5> With(params (T1?, T2?, T3?, T4?, T5?)[] args) {
    foreach (var x in args) With(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5);
    return this;
  }
}
