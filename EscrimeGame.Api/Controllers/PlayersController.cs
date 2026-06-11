using EscrimeGame.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscrimeGame.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly EscrimeDbContext _context;

    public PlayersController(EscrimeDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        return await _context.Players.Include(p => p.Matches).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _context.Players.Include(p => p.Matches).FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            return NotFound();
        }

        return player;
    }

    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }

    [HttpPost("{id}/matches")]
    public async Task<IActionResult> AddMatch(int id, MatchResult matchResult)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound("Player not found");
        }

        matchResult.PlayerId = id;
        _context.Matches.Add(matchResult);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}/penalty")]
    public async Task<IActionResult> UpdatePenalty(int id, int penaltyPoints, bool isDisqualified)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        player.PenaltyPoints = penaltyPoints;
        player.IsDisqualified = isDisqualified;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
