using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Plato.MDM.DataAccess.Postgres.Mappers;
using Plato.MDM.DataAccess.Postgres.Services;
using Plato.MDM.Storage.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStorageService();

string config = builder.Configuration["ConnectionString:DefaultConnection"]
    ?? throw new Exception("Нет строки подключения к БД");
builder.Services.AddDbContext<MdmDbContext>(options => options.UseNpgsql(config, 
    x => x.MigrationsHistoryTable("__MyMigrationsHistory", "public")));

// Add services to the container.
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options => 
    options.ListenAnyIP(5087, o => o.Protocols = HttpProtocols.Http2));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DirectoryGrpcService>();
app.MapGrpcService<DirectoryVersionGrpcService>();
app.MapGrpcService<DirectoryLevelGrpcService>();
app.MapGrpcService<DirectoryDomainGrpcService>();
app.MapGrpcService<DirectoryDataGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

using (var scope = app.Services.CreateScope())
using (var ctx = scope.ServiceProvider.GetRequiredService<MdmDbContext>())
    await ctx.Database.MigrateAsync();

app.Run();
