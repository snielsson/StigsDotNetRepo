// Copyright © 2023 TradingLens. All Rights Reserved.

using System.Text.Json;

namespace CleanArchitectureSolutionTemplate.Domain;

public interface IHttpClientService : IDisposable {
  Task<T?> Get<T>(string uri);
  JsonSerializerOptions JsonSerializerOptions { get; set; }
}