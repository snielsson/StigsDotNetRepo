// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Text.Json;
using CleanArchitectureSolutionTemplate.Domain;
using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CleanArchitectureSolutionTemplate.Infrastructure.System;

[Singleton]
internal sealed class HttpClientService : IHttpClientService {
  private readonly HttpClient _httpClient;
  public const int DefaultTimeoutMs = 120000;
  public HttpClientService(IConfiguration configuration) {
    if (!TimeSpan.TryParse(configuration["HttpClientService:ConnectionLifeTime"], out TimeSpan timeout)) {
      timeout = TimeSpan.FromMilliseconds(DefaultTimeoutMs);
    }
    var handler = new SocketsHttpHandler {
      PooledConnectionLifetime = timeout
    };
    _httpClient = new HttpClient(handler);
  }

  public JsonSerializerOptions JsonSerializerOptions { get; set; } = new() {
    PropertyNameCaseInsensitive = true
  };
  private T? JsonDeserialize<T>(string? json) => json == null ? default : JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
  public async Task<T?> Get<T>(string uri) => JsonDeserialize<T>(await _httpClient.GetStringAsync(uri));

  public void Dispose() {
    _httpClient.Dispose();
  }
}