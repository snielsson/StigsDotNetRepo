// Copyright © 2023 TradingLens. All Rights Reserved.

namespace Stigs.Utils.Common;

public interface IHandler<in T> {
  Task<IResult> Handle(T command);
}

public interface IHandler<in T, TResult> {
  Task<IResult<TResult>> Handle(T command);
}
