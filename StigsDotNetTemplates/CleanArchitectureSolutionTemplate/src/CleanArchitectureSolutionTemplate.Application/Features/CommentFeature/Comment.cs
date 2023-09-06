// Copyright © 2023 TradingLens. All Rights Reserved.

using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;

namespace CleanArchitectureSolutionTemplate.Application.Features.CommentFeature;
public record Comment(long Id, long PostId, string Name, string Email, string Body);


[ConfigKey("Features:Comment")]
public class CommentOptions {
  public int DefaultPageSize { get; set; }
}