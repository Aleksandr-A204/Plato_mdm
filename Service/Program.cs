using Newtonsoft.Json.Serialization;
using Plato.MDM.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMdmService();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

if (builder.Environment.IsProduction())
    builder.Services.AddSpaStaticFiles(config => config.RootPath = "wwwroot");

// JSON Serializer
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

// Enable CORS
builder.Services.AddCors(option => option.AddPolicy(name: "myAllowSpecificOrigins", builder =>
    builder.AllowAnyOrigin()
        //.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

if (app.Environment.IsProduction())
{
    app.UseSpaStaticFiles();
    app.UseSpa(_ => { });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
