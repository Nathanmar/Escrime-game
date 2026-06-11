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
    public async Task<ActionResult<IEnumerable<Player>>> GetRanking()
    {
        var players = await _context.Players.Include(p => p.Matches).ToListAsync();
        return Ok(_ranking.GetRanking(players));
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
}
