using EscrimeGame.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscrimeGame.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentController : ControllerBase
{
    private readonly EscrimeDbContext _context;
    private readonly TournamentRanking _ranking;

    public TournamentController(EscrimeDbContext context, TournamentRanking ranking)
    {
        _context = context;
        _ranking = ranking;
    }

    [HttpGet("ranking")]
    public async Task<IActionResult> GetRanking()
    {
        var players = await _context.Players.Include(p => p.Matches).ToListAsync();
        var ranked = _ranking.GetRanking(players);
        var calculator = new ScoreCalculator();

        var result = ranked.Select(p => new {
            player = p,
            totalScore = calculator.CalculateScore(p.Matches, p.IsDisqualified, p.PenaltyPoints)
        });

        return Ok(result);
    }

    [HttpGet("champion")]
    public async Task<ActionResult<Player>> GetChampion()
    {
        var players = await _context.Players.Include(p => p.Matches).ToListAsync();
        var champion = _ranking.GetChampion(players);

        if (champion == null)
        {
            return NotFound("No champion found. The tournament might be empty.");
        }

        return Ok(champion);
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        // Clear existing data
        _context.Matches.RemoveRange(_context.Matches);
        _context.Players.RemoveRange(_context.Players);
        await _context.SaveChangesAsync();

        // Add default heroes
        var heroes = new List<Player>
        {
            new Player { Name = "Sir Galahad", CharacterClass = "Paladin", Icon = "🛡️" },
            new Player { Name = "Dame Morgane", CharacterClass = "Mage", Icon = "🧙‍♀️" },
            new Player { Name = "Chevalier Noir", CharacterClass = "Chevalier Obscur", Icon = "🗡️" },
            new Player { Name = "Lancelot", CharacterClass = "Chevalier", Icon = "⚔️" },
            new Player { Name = "Merlin", CharacterClass = "Enchanteur", Icon = "🔮" }
        };

        _context.Players.AddRange(heroes);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Arène initialisée avec succès !" });
    }

    [HttpPost("simulate-match")]
    public async Task<IActionResult> SimulateMatch()
    {
        var players = await _context.Players.ToListAsync();
        if (players.Count < 2)
        {
            return BadRequest("Pas assez de joueurs pour faire un combat.");
        }

        var random = new Random();
        
        // Tirer deux joueurs au hasard
        int index1 = random.Next(players.Count);
        int index2;
        do
        {
            index2 = random.Next(players.Count);
        } while (index1 == index2);

        var p1 = players[index1];
        var p2 = players[index2];

        // Tirer un résultat au hasard
        var outcome1 = (MatchResult.Result)random.Next(3);
        MatchResult.Result outcome2;

        if (outcome1 == MatchResult.Result.Win) outcome2 = MatchResult.Result.Loss;
        else if (outcome1 == MatchResult.Result.Loss) outcome2 = MatchResult.Result.Win;
        else outcome2 = MatchResult.Result.Draw;

        _context.Matches.Add(new MatchResult { PlayerId = p1.Id, Outcome = outcome1 });
        _context.Matches.Add(new MatchResult { PlayerId = p2.Id, Outcome = outcome2 });

        await _context.SaveChangesAsync();

        string resultMessage = outcome1 == MatchResult.Result.Draw
            ? $"{p1.Name} a fait match nul contre {p2.Name}."
            : (outcome1 == MatchResult.Result.Win ? $"{p1.Name} a vaincu {p2.Name} !" : $"{p2.Name} a vaincu {p1.Name} !");

        return Ok(new { message = resultMessage, match = new { p1 = p1.Name, p2 = p2.Name, p1Result = outcome1.ToString() } });
    }

    [HttpPost("play-match")]
    public async Task<IActionResult> PlayMatch([FromBody] PlayMatchRequest request)
    {
        var p1 = await _context.Players.FindAsync(request.Player1Id);
        var p2 = await _context.Players.FindAsync(request.Player2Id);

        if (p1 == null || p2 == null)
            return BadRequest("Un ou plusieurs joueurs introuvables.");

        if (p1.Id == p2.Id)
            return BadRequest("Un joueur ne peut pas se battre contre lui-même.");

        var random = new Random();
        var outcome1 = (MatchResult.Result)random.Next(3);
        MatchResult.Result outcome2;

        if (outcome1 == MatchResult.Result.Win) outcome2 = MatchResult.Result.Loss;
        else if (outcome1 == MatchResult.Result.Loss) outcome2 = MatchResult.Result.Win;
        else outcome2 = MatchResult.Result.Draw;

        _context.Matches.Add(new MatchResult { PlayerId = p1.Id, Outcome = outcome1 });
        _context.Matches.Add(new MatchResult { PlayerId = p2.Id, Outcome = outcome2 });

        await _context.SaveChangesAsync();

        return Ok(new {
            player1 = p1,
            player2 = p2,
            outcome1 = outcome1.ToString(),
            outcome2 = outcome2.ToString(),
            message = outcome1 == MatchResult.Result.Draw
                ? $"{p1.Name} a fait match nul contre {p2.Name}."
                : (outcome1 == MatchResult.Result.Win ? $"{p1.Name} a vaincu {p2.Name} !" : $"{p2.Name} a vaincu {p1.Name} !")
        });
    }
}

public record PlayMatchRequest(int Player1Id, int Player2Id);
