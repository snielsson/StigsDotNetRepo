// Copyright © 2023 TradingLens. All Rights Reserved.

namespace Stigs.Utils.Common;

public abstract class CommandBase<T> where T:CommandBase<T> {
  public Task<IResult> Handle() => Sys.Resolve<IHandler<T>>().Handle((T)this);
}

public abstract class CommandBase<TCommand,TResult> where TCommand:CommandBase<TCommand,TResult> {
  public Task<IResult<TResult>> Handle() => Sys.Resolve<IHandler<TCommand,TResult>>().Handle((TCommand)this);
}