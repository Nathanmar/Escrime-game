using System.Collections.Generic;
using System.Linq;

namespace EscrimeGame;

public class TournamentRanking
{
    private readonly ScoreCalculator _scoreCalculator;

    public TournamentRanking(ScoreCalculator scoreCalculator)
    {
        _scoreCalculator = scoreCalculator;
    }

    /// <summary>
    /// Classe les joueurs par score décroissant
    /// </summary>
    public List<Player> GetRanking(List<Player> players)
    {
        if (players == null)
            return new List<Player>();

        return players
            .OrderByDescending(p => _scoreCalculator.CalculateScore(p.Matches, p.IsDisqualified, p.PenaltyPoints))
            .ToList();
    }

    /// <summary>
    /// Trouve le champion (joueur avec le meilleur score)
    /// </summary>
    public Player? GetChampion(List<Player> players)
    {
        if (players == null || !players.Any())
            return null;

        var ranked = GetRanking(players);
        return ranked.FirstOrDefault();
    }
}