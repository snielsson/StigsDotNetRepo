// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Stigs.Utils.Common;

public abstract class CommandHandlerBase<TCommand> : CommandHandlerBase<TCommand, object?>, IHandler<TCommand> {
  async Task<IResult> IHandler<TCommand>.Handle(TCommand command) => await base.InternalHandle(command);
  /// <summary>
  /// Returns a null if validation succeeded.
  /// Returns a non-null object (validation error data) if validation failed.
  /// Defaults to a successful non operation if not overridden.///
  /// </summary>
  protected new virtual Task OnValidate(TCommand command) => Task.CompletedTask;

  /// <summary>
  /// Returns a null if authorization succeeded.
  /// Returns a non-null object (authorization error data) if validation failed.
  /// Defaults to a successful non operation if not overridden.
  /// </summary>
  protected new virtual Task OnAuthorize(TCommand command) => Task.CompletedTask;

  protected new abstract Task OnExecute(TCommand command);

}

public abstract class CommandHandlerBase<TCommand,TResult> : IHandler<TCommand,TResult> {
  protected readonly ILogger _logger;
  public CommandHandlerBase(ILogger? logger = null) {
    _logger = logger ?? Sys.Resolve<ILoggerFactory>().CreateLogger(GetType().Name);
  }
  Task<IResult<TResult>> IHandler<TCommand,TResult>.Handle(TCommand command) => InternalHandle(command);

  /// <summary>
  /// Handle implements a template method (or pipeline) of
  /// the following order of steps to execute when a command is handled:
  ///
  /// - OnAuditLog : Logs who did what when.
  /// - OnAuthorize: Check if the calling identity is authorized to execute.
  /// - OnValidate : Validate that the command input is valid.
  /// - OnExecute  : Executes the logic of the command.
  /// </summary>
  /// <param name="command"></param>
  /// <returns></returns>
  protected virtual async Task<IResult<TResult>> InternalHandle(TCommand command) {
    if (command == null) throw new ArgumentNullException(nameof(command));
    await OnAuditLog(command);
    object? authorizationResult = await OnAuthorize(command);
    if (authorizationResult != null) return Result<TResult>.Error(authorizationResult, "Authorization Error");
    object? validationResult = await OnValidate(command);
    if (validationResult != null) return Result<TResult>.Error(validationResult, "Validation Error");
    TResult? resultValue = await OnExecute(command);
    return new Result<TResult>().SetOk(resultValue);
  }

  protected virtual Task OnAuditLog([DisallowNull] TCommand command) {
    if (command == null) throw new ArgumentNullException(nameof(command));
    _logger.Log(AuditLogLevel, "{CommandName} called by {UserId}", command.GetType().Name, Sys.UserId);
    return Task.CompletedTask;
  }
  public LogLevel AuditLogLevel { get; set; } = LogLevel.Information;

  /// <summary>
  /// Returns a null if validation succeeded.
  /// Returns a non-null object (validation error data) if validation failed.
  /// Defaults to a successful non operation if not overridden.///
  /// </summary>
  protected virtual Task<object?> OnValidate(TCommand command) => Task.FromResult((object?)null);

  /// <summary>
  /// Returns a null if authorization succeeded.
  /// Returns a non-null object (authorization error data) if validation failed.
  /// Defaults to a successful non operation if not overridden.
  /// </summary>
  protected virtual Task<object?> OnAuthorize(TCommand command) => Task.FromResult((object?)null);

  protected virtual Task<TResult?> OnExecute(TCommand command) => Task.FromResult(default(TResult?));
}