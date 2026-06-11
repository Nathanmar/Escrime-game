using System.Collections.Generic;

namespace EscrimeGame;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MatchResult> Matches { get; set; } = new();
    public bool IsDisqualified { get; set; }
    public int PenaltyPoints { get; set; }
}
