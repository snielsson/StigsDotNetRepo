using CleanArchitectureSolutionTemplate.Application.Features.CommentFeature;
using CleanArchitectureSolutionTemplate.Domain;
using CleanArchitectureSolutionTemplate.Domain.Utils.DependencyInjection;
using Microsoft.Extensions.Options;
using static Microsoft.AspNetCore.Http.Results;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AssemblyScan().Services
       .AddControllers().Services
       .AddEndpointsApiExplorer()
       .AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}
//app.UseHttpsRedirection();
app.UseAuthorization();
app.Use(async (context, next) => {
  Sys.Initialize(app.Services);
  await next(context);
});

app.MapGet("/", (IEnumerable<IAssemblyInfo> modules) => new {
  Status = "Running - server time is " + Sys.TimeService.UtcNow.ToString("u"),
  Modules = modules
});

app.MapGet("/comments/{postId}", async (long postId, IGetCommentsHandler getCommentsHandler)
             => await getCommentsHandler.Execute(postId) is var result ? Ok(result) : NotFound());

var options = app.Services.GetRequiredService<IOptions<CommentOptions>>();
await app.RunAsync();
