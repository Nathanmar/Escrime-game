using EscrimeGame.Api.Data;
using EscrimeGame;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<EscrimeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=escrime.db"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Register domain services
builder.Services.AddScoped<ScoreCalculator>();
builder.Services.AddScoped<TournamentRanking>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

// Initialisation automatique de la base de données
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EscrimeDbContext>();
    
    // Applique automatiquement les migrations (crée la BDD si elle n'existe pas)
    context.Database.Migrate();

    // Seed automatique s'il n'y a pas de joueurs
    if (!context.Players.Any())
    {
        var heroes = new List<Player>
        {
            new Player { Name = "Sir Galahad", CharacterClass = "Paladin", Icon = "🛡️" },
            new Player { Name = "Dame Morgane", CharacterClass = "Mage", Icon = "🧙‍♀️" },
            new Player { Name = "Chevalier Noir", CharacterClass = "Chevalier Obscur", Icon = "🗡️" },
            new Player { Name = "Lancelot", CharacterClass = "Chevalier", Icon = "⚔️" },
            new Player { Name = "Merlin", CharacterClass = "Enchanteur", Icon = "🔮" }
        };
        context.Players.AddRange(heroes);
        context.SaveChanges();
    }
}

app.Run();
