export interface Player {
    id: number;
    name: string;
    isDisqualified: boolean;
    penaltyPoints: number;
}

export interface PlayerScore {
    player: Player;
    totalScore: number;
}

export interface RankingResponse {
    rankings: PlayerScore[];
}

export interface ChampionResponse {
    champion: Player | null;
}
