using System;
using System.Collections.Generic;

namespace EscrimeGame;

public class ScoreCalculator
{
    /// <summary>
    /// Calcule le score final d'un joueur selon les règles du tournoi
    /// </summary>
    /// <param name="matches">Liste des résultats de combat dans l'ordre chronologique</param>
    /// <param name="isDisqualified">True si le joueur est disqualifié</param>
    /// <param name="penaltyPoints">Points de pénalité (nombre positif)</param>
    /// <returns>Score final (jamais négatif)</returns>
    public int CalculateScore(List<MatchResult> matches, bool isDisqualified = false, int penaltyPoints = 0)
    {
        if (matches == null)
        {
            throw new ArgumentNullException(nameof(matches), "Matches list cannot be null.");
        }

        if (penaltyPoints < 0)
        {
            throw new ArgumentException("Penalty points cannot be negative.", nameof(penaltyPoints));
        }

        if (isDisqualified)
        {
            return 0;
        }

        int score = 0;
        int consecutiveWins = 0;

        foreach (var match in matches)
        {
            if (match.Outcome == MatchResult.Result.Win)
            {
                score += 3;
                consecutiveWins++;
                if (consecutiveWins == 3)
                {
                    score += 5;
                }
            }
            else
            {
                if (match.Outcome == MatchResult.Result.Draw)
                {
                    score += 1;
                }
                consecutiveWins = 0;
            }
        }

        score -= penaltyPoints;

        if (score < 0)
        {
            score = 0;
        }

        return score;
    }
}