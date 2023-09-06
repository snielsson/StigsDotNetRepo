// Copyright © 2023 TradingLens. All Rights Reserved.

using CleanArchitectureSolutionTemplate.Domain;
using CleanArchitectureSolutionTemplate.Domain.Utils;
using CleanArchitectureSolutionTemplate.Domain.Utils.CQS;
using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureSolutionTemplate.Application.Features.CommentFeature;

public interface IGetCommentsHandler : IHandler<GetCommentsQuery, Comment[]> { }
public interface IGetCommentsHandlerOther : IHandler<GetCommentsQuery, Comment[]> { }

[Singleton]
internal sealed class GetCommentsHandler : CommandQueryHandlerBase<GetCommentsQuery,Comment[]>, IGetCommentsHandler {
  private readonly IHttpClientService _http;
  public GetCommentsHandler(ILogger<GetCommentsHandler> logger, IHttpClientService http):base(logger)  {
    _http = http;
  }
  protected override async Task<Result<Comment[]?>> OnExecute(GetCommentsQuery query) {
    return await _http.Get<Comment[]>($"https://jsonplaceholder.typicode.com/posts/{query.PostId}/comments");
  }
}

[Singleton]
internal sealed class GetCommentsHandlerOther : CommandQueryHandlerBase<GetCommentsQuery,Comment[]>, IGetCommentsHandlerOther {
  private readonly IHttpClientService _http;
  public GetCommentsHandlerOther(ILogger<GetCommentsHandler> logger, IHttpClientService http):base(logger)  {
    _http = http;
  }
  protected override async Task<Result<Comment[]?>> OnExecute(GetCommentsQuery query) {
    return await _http.Get<Comment[]>($"https://jsonplaceholder.typicode.com/posts/{query.PostId}/comments");
  }
}