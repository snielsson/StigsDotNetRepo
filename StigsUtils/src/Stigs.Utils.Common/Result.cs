// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Runtime.Serialization;

namespace Stigs.Utils.Common;

public interface IResult {
  public Task Task { get; }
}

public interface IResult<T> : IResult  {
  public Task<T?> Value { get; }
}

public abstract class ResultBase {
  private ITimeService? _timeService;
  public static ITimeService? DefaultTimeService { get; set; }
  public ITimeService? TimeService {
    get => _timeService ?? Sys.TimeServiceOrNull ?? DefaultTimeService;
    set => _timeService = value;
  }
}

public abstract class ResultBase<T> : ResultBase, IResult {
  public DateTime? StartingTime { get; protected set; }
  public DateTime? EndTime { get; protected set; }
  public TimeSpan? Elapsed => EndTime - StartingTime;
  public string? Message { get; protected set; } = null;
  public object? Data { get; protected set; } = null;
  protected ResultBase() {
    StartingTime = TimeService?.UtcNow;
  }
  protected readonly TaskCompletionSource<T?> _tcs = new();
  public Task Task => _tcs.Task;


  protected void SetResult(TaskStatus status, DateTime? endTime, T? value, Exception? exception, CancellationToken? cancellationToken, string? message = null) {
    EndTime = TimeService?.UtcNow;
    switch (status) {
      case TaskStatus.RanToCompletion:
        if(exception != null) _tcs.SetException(exception);
        else _tcs.SetResult(value);
        break;
      case TaskStatus.Faulted:
        _tcs.SetException(exception ?? new ResultException<T>(value,message));
        break;
      case TaskStatus.Canceled:
        if(cancellationToken == null) _tcs.SetCanceled();
        else _tcs.SetCanceled(cancellationToken.Value);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(status), status, null);
    }
    Status = status;
  }
  public TaskStatus Status { get; private set; }

}
public class Result<T> : ResultBase<T>, IResult<T> {
  public Task<T?> Value => _tcs.Task;
  public Result<T> SetOk(T? value, string? message = null) {
    SetResult(TaskStatus.RanToCompletion, TimeService?.UtcNow, value, null,null,message);
    return this;
  }
  public Result<T> SetCancelled(CancellationToken? cancellationToken, string? message = null) {
    SetResult(TaskStatus.Canceled, TimeService?.UtcNow, default, null, cancellationToken,message);
    return this;
  }
  public Result<T> SetCancelled(string message) {
    SetResult(TaskStatus.Canceled, TimeService?.UtcNow, default, null, null,message);
    return this;
  }
  public Result<T> SetError
    (string message) {
    SetResult(TaskStatus.Faulted, TimeService?.UtcNow, default, null, null,message);
    return this;
  }
  public Result<T> SetError(Exception ex, string? message = null) {
    SetResult(TaskStatus.Faulted, TimeService?.UtcNow, default, ex, null,message);
    return this;
  }
  public Result<T> SetError(object? error, string? message = null) {
    SetResult(TaskStatus.Faulted, TimeService?.UtcNow, default, null, null,message??error?.ToString());
    Data = error;
    return this;
  }
  public static Result<T> Error(string message) => new Result<T>().SetError(message);
  public static Result<T> Error(object data, string? message = null) => new Result<T>().SetError(data, message);
}

public class Result : ResultBase<object?> {
  public static Result Error(string message) => new Result().SetError(message);
  public static Result Error(object data, string? message = null) => new Result().SetError(data, message);

  public Result SetOk(string? message = null) {
    SetResult(TaskStatus.RanToCompletion, TimeService?.UtcNow, null, null,null,message);
    return this;
  }
  public Result SetCancelled(CancellationToken? cancellationToken, string? message = null) {
    SetResult(TaskStatus.Canceled, TimeService?.UtcNow, default, null, cancellationToken,message);
    return this;
  }
  public Result SetCancelled(string message) {
    SetResult(TaskStatus.Canceled, TimeService?.UtcNow, default, null, null,message);
    return this;
  }
  public Result SetError(string message) {
    SetResult(TaskStatus.Faulted, TimeService?.UtcNow, default, null, null,message);
    return this;
  }
  public Result SetError(Exception ex, string? message = null) {
    SetResult(TaskStatus.Faulted, TimeService?.UtcNow, default, ex, null,message);
    return this;
  }
  public Result SetError(object? error, string? message = null) {
    SetResult(TaskStatus.Faulted, TimeService?.UtcNow, default, null, null,message??error?.ToString());
    Data = error;
    return this;
  }
}

public class ResultException<T> : Exception {
  protected ResultException(SerializationInfo info, StreamingContext context, T? value = default) : base(info, context) {
    Value = value;
  }
  public ResultException(string? message = null, Exception? innerException = null) : this(default, message, innerException) { }
  public ResultException(T? value = default, string? message = null, Exception? innerException = null) : base(message, innerException) {
    Value = value;
  }
  public T? Value { get; }
}
