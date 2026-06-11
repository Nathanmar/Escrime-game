import { useEffect, useState } from 'react'
import type { RankingResponse, ChampionResponse, PlayerScore, Player } from './types'

function App() {
  const [ranking, setRanking] = useState<PlayerScore[]>([]);
  const [champion, setChampion] = useState<Player | null>(null);
  const [loading, setLoading] = useState(true);
  
  // Nouveaux états pour le système de combat
  const [player1Id, setPlayer1Id] = useState<string>("");
  const [player2Id, setPlayer2Id] = useState<string>("");
  const [isFighting, setIsFighting] = useState(false);
  const [fightResult, setFightResult] = useState<any>(null);
  const [showResult, setShowResult] = useState(false); // pour gérer la fin de l'animation

  const API_URL = 'http://localhost:5136'; // Port du backend ASP.NET Core API

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const rankingRes = await fetch(`${API_URL}/api/tournament/ranking`);
      if (rankingRes.ok) {
        const rankingData: PlayerScore[] = await rankingRes.json();
        setRanking(rankingData);
        // Sélection par défaut pour les listes
        if (rankingData.length >= 2 && !player1Id) {
          setPlayer1Id(rankingData[0].player.id.toString());
          setPlayer2Id(rankingData[1].player.id.toString());
        }
      }

      const champRes = await fetch(`${API_URL}/api/tournament/champion`);
      if (champRes.ok) {
        const champData: Player = await champRes.json();
        setChampion(champData);
      }
    } catch (error) {
      console.error("Erreur lors de la récupération des données : ", error);
    } finally {
      setLoading(false);
    }
  };

  const seedDatabase = async () => {
    try {
      await fetch(`${API_URL}/api/tournament/seed`, { method: 'POST' });
      fetchData();
    } catch (error) {
      console.error(error);
    }
  };

  const playMatch = async () => {
    if (player1Id === player2Id) {
      alert("Un joueur ne peut pas se battre contre lui-même !");
      return;
    }

    try {
      const res = await fetch(`${API_URL}/api/tournament/play-match`, { 
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ player1Id: parseInt(player1Id), player2Id: parseInt(player2Id) })
      });

      if (res.ok) {
        const data = await res.json();
        
        // Démarrer l'animation
        setFightResult(data);
        setIsFighting(true);
        setShowResult(false);

        // L'animation dure 3 secondes
        setTimeout(() => {
          setShowResult(true); // Affiche qui gagne et qui perd
          // Après 2 secondes d'affichage du résultat, on retourne au classement
          setTimeout(() => {
            setIsFighting(false);
            fetchData(); // On met à jour le classement avec le nouveau résultat !
          }, 2500);
        }, 3000);

      } else {
        alert("Erreur lors de l'appel au combat.");
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <>
      <h1>Escrime Fantastique</h1>
      
      {loading && !isFighting ? (
        <div className="loading">Chargement de l'arène...</div>
      ) : isFighting && fightResult ? (
        <div className="arena-view">
          <h2 style={{ textAlign: 'center', marginBottom: '2rem' }}>Combat en cours !</h2>
          <div className="arena">
            {/* Joueur 1 */}
            <div className={`fighter ${!showResult ? 'clashing-left' : (fightResult.outcome1 === 'Win' ? 'winner' : (fightResult.outcome1 === 'Loss' ? 'loser' : ''))}`}>
              <div style={{ fontSize: '5rem' }}>{fightResult.player1.icon}</div>
              <h3>{fightResult.player1.name}</h3>
            </div>

            <div className="vs-text">VS</div>

            {/* Joueur 2 */}
            <div className={`fighter ${!showResult ? 'clashing-right' : (fightResult.outcome2 === 'Win' ? 'winner' : (fightResult.outcome2 === 'Loss' ? 'loser' : ''))}`}>
              <div style={{ fontSize: '5rem' }}>{fightResult.player2.icon}</div>
              <h3>{fightResult.player2.name}</h3>
            </div>
          </div>
          {showResult && (
            <div style={{ textAlign: 'center', fontSize: '1.5rem', marginTop: '2rem', color: '#00ffcc' }}>
              <strong>{fightResult.message}</strong>
            </div>
          )}
        </div>
      ) : (
        <div className="container">
          <div style={{ textAlign: 'center', marginBottom: '2rem', display: 'flex', flexDirection: 'column', gap: '1rem', alignItems: 'center' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
              <select className="select-hero" value={player1Id} onChange={(e) => setPlayer1Id(e.target.value)}>
                {ranking.map(r => <option key={`p1-${r.player.id}`} value={r.player.id}>{r.player.icon} {r.player.name}</option>)}
              </select>
              <span style={{ fontWeight: 'bold' }}>CONTRE</span>
              <select className="select-hero" value={player2Id} onChange={(e) => setPlayer2Id(e.target.value)}>
                {ranking.map(r => <option key={`p2-${r.player.id}`} value={r.player.id}>{r.player.icon} {r.player.name}</option>)}
              </select>
            </div>
            
            <div>
              <button onClick={playMatch} style={{ backgroundColor: '#ff007f', fontSize: '1.2rem', padding: '1rem 2rem' }}>⚔️ Lancer le Combat !</button>
            </div>
            <button onClick={seedDatabase} style={{ backgroundColor: 'transparent', border: '1px solid #7928ca', color: '#7928ca', marginTop: '1rem' }}>Réinitialiser les joueurs par défaut</button>
          </div>

          <div className="card champion-card">
            <h2>🏆 Champion Actuel 🏆</h2>
            {champion ? (
              <>
                <div style={{ fontSize: '4rem' }}>{champion.icon}</div>
                <div className="champion-name">{champion.name}</div>
                <p style={{ fontStyle: 'italic', color: '#ccc' }}>{champion.characterClass}</p>
              </>
            ) : (
              <p>Aucun champion défini pour le moment.</p>
            )}
            <button onClick={fetchData} style={{ marginTop: '1rem' }}>Actualiser</button>
          </div>

          <div className="card">
            <h2>Classement du Tournoi</h2>
            {ranking.length > 0 ? (
              <ul className="player-list">
                {ranking.map((score, index) => (
                  <li key={score.player.id} className="player-item">
                    <span className="player-name">
                      {index + 1}. {score.player.icon} {score.player.name}
                      <span style={{ fontSize: '0.8rem', color: '#aaa', marginLeft: '10px' }}>({score.player.characterClass})</span>
                    </span>
                    <span className="player-score">{score.totalScore} pts</span>
                  </li>
                ))}
              </ul>
            ) : (
              <p>L'arène est vide. Cliquez sur "Réinitialiser les joueurs par défaut".</p>
            )}
          </div>
        </div>
      )}
    </>
  )
}

export default App
