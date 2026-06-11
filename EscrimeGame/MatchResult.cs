namespace EscrimeGame;

public class MatchResult
{
    public enum Result
    {
        Win,   // Victoire
        Draw,  // Match nul
        Loss   // Défaite
    }

    public int Id { get; set; }
    public int PlayerId { get; set; }
    
    public Result Outcome { get; set; }

    // Constructeur pour faciliter les tests
    public MatchResult(Result outcome)
    {
        Outcome = outcome;
    }

    // Constructeur par défaut
    public MatchResult() { }
}