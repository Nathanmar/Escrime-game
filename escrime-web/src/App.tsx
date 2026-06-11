import { useEffect, useState } from 'react'
import { RankingResponse, ChampionResponse, PlayerScore, Player } from './types'

function App() {
  const [ranking, setRanking] = useState<PlayerScore[]>([]);
  const [champion, setChampion] = useState<Player | null>(null);
  const [loading, setLoading] = useState(true);

  const API_URL = 'http://localhost:5136'; // Port du backend ASP.NET Core API

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      // On fetch le ranking
      const rankingRes = await fetch(`${API_URL}/api/tournament/ranking`);
      if (rankingRes.ok) {
        const rankingData: RankingResponse = await rankingRes.json();
        setRanking(rankingData.rankings);
      }

      // On fetch le champion
      const champRes = await fetch(`${API_URL}/api/tournament/champion`);
      if (champRes.ok) {
        const champData: ChampionResponse = await champRes.json();
        setChampion(champData.champion);
      }
    } catch (error) {
      console.error("Erreur lors de la récupération des données : ", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <h1>Escrime Fantastique</h1>
      
      {loading ? (
        <div className="loading">Chargement des données de l'arène...</div>
      ) : (
        <div className="container">
          {/* Section Champion */}
          <div className="card champion-card">
            <h2>🏆 Champion Actuel 🏆</h2>
            {champion ? (
              <>
                <div className="champion-name">{champion.name}</div>
                <p>{champion.isDisqualified ? 'Disqualifié' : 'En lice'}</p>
              </>
            ) : (
              <p>Aucun champion défini pour le moment.</p>
            )}
            <button onClick={fetchData} style={{ marginTop: '1rem' }}>Actualiser l'Arène</button>
          </div>

          {/* Section Classement */}
          <div className="card">
            <h2>Classement du Tournoi</h2>
            {ranking.length > 0 ? (
              <ul className="player-list">
                {ranking.map((score, index) => (
                  <li key={score.player.id} className="player-item">
                    <span className="player-name">
                      {index + 1}. {score.player.name} {score.player.isDisqualified && '❌'}
                    </span>
                    <span className="player-score">{score.totalScore} pts</span>
                  </li>
                ))}
              </ul>
            ) : (
              <p>Aucun joueur dans le classement.</p>
            )}
          </div>
        </div>
      )}
    </>
  )
}

export default App
