// Copyright © 2023 TradingLens. All Rights Reserved.

namespace CleanArchitectureSolutionTemplate.Application.Features.CommentFeature;

public record struct GetCommentsQuery(long PostId) {
  public static implicit operator GetCommentsQuery(long postId) => new(postId);
};