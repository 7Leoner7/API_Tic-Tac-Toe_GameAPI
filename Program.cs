using API_Tic_Tac_Toe_Game.AppContext;
using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IDbProvider<GameRoomBase>, DbProvider<GameRoomBase>>();
builder.Services.AddTransient<IDbProvider<Player>, DbProvider<Player>>();
builder.Services.AddTransient<IDbProvider<GameMetaData>, DbProvider<GameMetaData>>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tic-Tac-Toe Game API",
        Description = "API для того, чтобы играть в крестики-нолики",
    });

    var xmlPath = builder.Environment.WebRootPath + "/xml/Tic-Tac-Toe_API.xml";
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

void HandleId(IApplicationBuilder app)
{
    app.Run(req => 
    { 
        req.Response.Redirect("/swagger/index.html"); 
        return req.Response.WriteAsync(""); 
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            c.InjectStylesheet("/swagger/ui/custom-swagger-ui.css");
        });

    app.MapWhen(context =>
    {
        return context.Request.Path == new PathString("/");
    }, HandleId);
}

app.MapControllers();

app.Run();
