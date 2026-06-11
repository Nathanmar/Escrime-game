# PLAN DE TEST

EscrimeGame - Système de notation pour un tournoi d'escrime fantastique

**Version :** 1.0

**Date :** 2026-06-10

**Statut : En cours**

**Méthodologie :** TDD (Red - Green - Refactor)

**Framework :** xUnit 2.6.1 + FluentAssertions 6.12.0 · .NET 10

# 1. Identification

| **Projet**  | EscrimeGame - Système de notation pour un tournoi d'escrime fantastique |
| ----------- | ----------------------------------------------------------------------- |
| **Version** | 1.0                                                                     |
| **Auteur**  | [Votre nom]                                                             |
| **Date**    | 2026-06-10                                                              |
| **Statut**  | **En cours**                                                            |

# 2. Périmètre

## 2.1 In Scope

- Classe `ScoreCalculator` — méthode publique : `CalculateScore`
- Classe `MatchResult` et son énumération `Result` (Win, Draw, Loss)
- Règles de calcul des points de base (victoire, nul, défaite)
- Règle du bonus de série (3 victoires consécutives ou plus → +5 points)
- Règle de disqualification (score total = 0)
- Règle des pénalités (soustraites du score, plancher à 0)
- Gestion des cas limites (liste vide, liste null, pénalités négatives)
- **[BONUS] Classe `TournamentRanking`** — méthodes : `GetRanking`, `GetChampion`
- **[BONUS] Classement des joueurs par score décroissant**
- **[BONUS] Gestion des égalités de score**

## 2.2 Out of Scope

- Persistance en base de données
- Interface utilisateur (IHM / front React)
- API REST / exposition HTTP
- Authentification et gestion des utilisateurs
- Tests de performance et de charge
- Tests de sécurité

# 3. Stratégie de test

## 3.1 Méthodologie

La campagne suit strictement le cycle TDD (Test-Driven Development) :

- **RED** : écriture du test avant toute ligne de code de production — le test doit être rouge (échec).
- **GREEN** : implémentation du code minimal pour faire passer le test au vert.
- **REFACTOR** : amélioration du code sans casser les tests existants.

Chaque exigence donne lieu à au minimum un commit « test rouge » suivi d'un commit « code vert ». L'historique Git doit refléter ce cycle de manière traçable.

## 3.2 Types de tests prévus

- Tests unitaires (xUnit + FluentAssertions) : couverture des 18 exigences fonctionnelles.
- Tests nominaux (happy path) : comportement attendu avec des données valides.
- Tests alternatifs : variantes valides (ex : bonus avec 4 victoires, bonus multiples).
- Tests d'erreur (sad path) : entrées invalides, liste null, pénalités négatives.
- Tests paramétrés `[Theory]` : pour couvrir plusieurs combinaisons de résultats en un seul test.

## 3.3 Traçabilité dans le code

Chaque test est annoté avec `[Trait("Requirement", "REQ-E-XXX")]` pour permettre le filtrage :

```
dotnet test --filter "Requirement=REQ-E-005"
```

# 4. Critères d'entrée

- Spécifications métier validées (sujet TP escrime fantastique).
- Squelette de classes fourni : `MatchResult`, `ScoreCalculator` (avec `NotImplementedException`).
- Solution .NET 10 initialisée avec deux projets : `EscrimeGame.Core` et `EscrimeGame.Tests`.
- Packages NuGet installés : xUnit 2.6.1, FluentAssertions 6.12.0, coverlet.collector.
- Environnement de développement opérationnel (Rider / VS, CLI dotnet).

# 5. Critères de sortie

- 100 % des **18 exigences** (REQ-E-001 à REQ-E-015 + REQ-E-016, REQ-E-017, REQ-E-018) couvertes par au moins un cas de test.
- Les exigences bonus REQ-E-016, REQ-E-017 et REQ-E-018 respectent le cycle TDD (commit rouge + commit vert documentés).
- Tous les tests passent au vert (0 échec, 0 test ignoré).
- Couverture de lignes ≥ 95 % sur la classe `ScoreCalculator`.
- Couverture de branches ≥ 90 % sur `ScoreCalculator`.
- Aucun bug bloquant ou critique non résolu.
- Historique Git : au moins un commit rouge + un commit vert par exigence.
- Rapport de test (`TEST_REPORT.md`) rédigé avec les métriques réelles d'exécution.

# 6. Environnement

| **Composant**                | **Version / Détail**                     |
| ---------------------------- | ---------------------------------------- |
| **.NET SDK**                 | 9.0                                      |
| **Runtime**                  | .NET 9.0                                 |
| **Framework de test**        | xUnit 2.6.1                              |
| **Assertions**               | FluentAssertions 6.12.0                  |
| **Couverture de code**       | coverlet.collector (XPlat Code Coverage) |
| **Rapport HTML**             | dotnet-reportgenerator-globaltool        |
| **OS cible**                 | Windows / Linux / macOS (cross-platform) |
| **IDE recommandé**           | Rider / Visual Studio                    |
| **Gestionnaire de packages** | NuGet                                    |
| **CI/CD (optionnel)**        | GitHub Actions                           |

# 7. Cas de test prévus

Chaque cas de test couvre une exigence métier identifiée. Les tests sont classés par thème, avec indication du type (nominal, alternatif, erreur) et de l'exigence couverte.

| **ID**     | **Titre**                                                              | **Données d'entrée**                          | **Résultat attendu**           | **Exigence** |
| ---------- | ---------------------------------------------------------------------- | --------------------------------------------- | ------------------------------ | ------------ |
| **TC-001** | Calcul simple : Win, Draw, Loss                                        | [Win, Draw, Loss]                             | 4 points (3+1+0)               | REQ-E-001    |
| **TC-002** | Que des victoires : Win, Win                                           | [Win, Win]                                    | 6 points                       | REQ-E-001    |
| **TC-003** | Que des nuls : Draw, Draw, Draw                                        | [Draw, Draw, Draw]                            | 3 points                       | REQ-E-002    |
| **TC-004** | Que des défaites : Loss, Loss                                          | [Loss, Loss]                                  | 0 point                        | REQ-E-003    |
| **TC-005** | Bonus minimum : 3 victoires consécutives                               | [Win, Win, Win]                               | 14 points (9+5)                | REQ-E-004    |
| **TC-006** | Bonus avec 4 victoires consécutives                                    | [Win, Win, Win, Win]                          | 17 points (12+5)               | REQ-E-004    |
| **TC-007** | Pas de bonus si la série est interrompue                               | [Win, Win, Loss, Win]                         | 6 points                       | REQ-E-004    |
| **TC-008** | Bonus non accordé : Win, Draw, Win, Win                                | [Win, Draw, Win, Win]                         | 10 points                      | REQ-E-004    |
| **TC-009** | Bonus multiple : deux séries distinctes                                | [Win×3, Loss, Win×4]                          | 31 points (21+5+5)             | REQ-E-004    |
| **TC-010** | Disqualifié avec score positif → 0                                     | [Win, Win, Win], isDisqualified=true          | 0 point                        | REQ-E-005    |
| **TC-011** | Disqualifié sans aucun combat → 0                                      | [], isDisqualified=true                       | 0 point                        | REQ-E-005    |
| **TC-012** | Pénalités normales soustraites du score                                | [Win, Win, Draw], penaltyPoints=3             | 4 points (7-3)                 | REQ-E-006    |
| **TC-013** | Pénalités supérieures au score → plancher à 0                          | [Win, Draw], penaltyPoints=10                 | 0 point                        | REQ-E-006    |
| **TC-014** | Pénalités égales au score → 0                                          | [Win, Win, Win], penaltyPoints=14             | 0 point                        | REQ-E-006    |
| **TC-015** | Liste vide : aucun combat → 0 point                                    | []                                            | 0 point                        | REQ-E-007    |
| **TC-016** | Liste null → ArgumentNullException                                     | null                                          | ArgumentNullException "matches"| REQ-E-008    |
| **TC-017** | Pénalités négatives → ArgumentException                                | penaltyPoints=-5                              | ArgumentException              | REQ-E-009    |
| **TC-018** | Theory paramétrée : plusieurs scénarios combinés                       | InlineData multiples                          | Résultats variés               | REQ-E-001 à REQ-E-004 |
| **TC-019** | ★ Classement correct par score décroissant (3 joueurs)                | 3 joueurs avec scores différents              | Liste triée décroissante       | REQ-E-016    |
| **TC-020** | ★ Gestion des égalités de score                                       | 2 joueurs avec score identique                | Ordre stable conservé          | REQ-E-017    |
| **TC-021** | ★ Champion = joueur avec le score le plus élevé                       | 3 joueurs, scores 14 / 7 / 4                  | Joueur avec 14 points          | REQ-E-016    |
| **TC-022** | ★ Tous les joueurs disqualifiés → champion à 0 point                  | 3 joueurs tous disqualifiés                   | Score du champion = 0          | REQ-E-018    |

# 8. Matrice de traçabilité

Chaque exigence métier est reliée à au moins un cas de test. Cette matrice prouve l'exhaustivité de la couverture.

| **ID Exigence**    | **Description**                                              | **Cas de test**              | **Méthode couverte**          | **Statut prévu** |
| ------------------ | ------------------------------------------------------------ | ---------------------------- | ----------------------------- | ---------------- |
| **REQ-E-001**      | Victoire = +3 points, Nul = +1 point                         | TC-001, TC-002, TC-003       | CalculateScore                | À faire          |
| **REQ-E-002**      | Défaite = 0 point                                            | TC-004                       | CalculateScore                | À faire          |
| **REQ-E-003**      | Calcul cumulatif de tous les combats                         | TC-001, TC-002               | CalculateScore                | À faire          |
| **REQ-E-004**      | Bonus de série : +5 points pour 3 victoires consécutives     | TC-005, TC-006, TC-007, TC-008, TC-009 | CalculateScore      | À faire          |
| **REQ-E-005**      | Disqualification : score total = 0                           | TC-010, TC-011               | CalculateScore                | À faire          |
| **REQ-E-006**      | Pénalités soustraites, score plancher à 0                    | TC-012, TC-013, TC-014       | CalculateScore                | À faire          |
| **REQ-E-007**      | Liste vide retourne 0 point                                  | TC-015                       | CalculateScore                | À faire          |
| **REQ-E-008**      | Liste null lève ArgumentNullException                        | TC-016                       | CalculateScore                | À faire          |
| **REQ-E-009**      | Pénalités négatives lèvent ArgumentException                 | TC-017                       | CalculateScore                | À faire          |
| **REQ-E-010**      | Tests paramétrés [Theory] couvrant plusieurs scénarios       | TC-018                       | CalculateScore                | À faire          |
| **REQ-E-016 ★**    | Classement des joueurs par score décroissant                 | TC-019, TC-021               | GetRanking, GetChampion       | À faire          |
| **REQ-E-017 ★**    | Gestion des égalités de score dans le classement             | TC-020                       | GetRanking                    | À faire          |
| **REQ-E-018 ★**    | Tous les joueurs disqualifiés → champion à 0                 | TC-022                       | GetChampion                   | À faire          |

# 9. Risques identifiés et mitigations

| **Risque**                                                                                          | **Probabilité** | **Impact** | **Mitigation**                                                                                                                         |
| --------------------------------------------------------------------------------------------------- | --------------- | ---------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| Mauvaise interprétation du bonus : accordé une seule fois même pour 4+ victoires consécutives       | Haute           | Élevé      | Relire attentivement les exemples 2 et 3 du sujet. Écrire TC-006 avant d'implémenter la détection de série.                           |
| Confusion sur le bonus multiple : deux séries séparées doivent chacune accorder +5                  | Haute           | Élevé      | Créer TC-009 en priorité et le valider manuellement avant implémentation.                                                              |
| Score négatif non bloqué : oubli du plancher à 0 après soustraction des pénalités                   | Moyenne         | Élevé      | TC-013 et TC-014 couvrent les cas limites. Vérifier que `Math.Max(0, score - penalties)` est utilisé.                                 |
| Disqualification ignorée si traitée après les pénalités                                             | Moyenne         | Élevé      | Appliquer la disqualification en dernier dans le pipeline de calcul. TC-010 doit être écrit avant toute implémentation.                |
| Implémentation anticipée (coder avant le test rouge) — violation du cycle TDD                       | Haute           | Moyen      | Committer le test rouge avant d'écrire le moindre code de production. Vérifier l'historique Git.                                      |
| Test faux positif : NotImplementedException masquée fait passer un test en erreur plutôt qu'en échec | Faible         | Élevé      | Exécuter `dotnet test` après chaque ajout de test et vérifier que le statut est bien FAILED avant de coder.                           |
| Bonus de série : détection sur fenêtre glissante vs reset au Loss                                   | Moyenne         | Élevé      | TC-007 et TC-008 couvrent ce cas. Implémenter un compteur de victoires consécutives remis à zéro à chaque non-victoire.               |
| Tests non déterministes (état partagé entre tests)                                                  | Faible          | Moyen      | Chaque test instancie son propre `ScoreCalculator`. Pas de state partagé entre tests.                                                  |
| ★ REQ-E-017 : GetRanking doit être stable sur les égalités                                         | Moyenne         | Faible     | Utiliser `OrderByDescending` avec un tri stable. Ajouter TC-020 pour valider l'ordre conservé en cas d'égalité.                       |

# 10. Responsabilités

| **Rôle**                  | **Responsable**       | **Activités**                                                             |
| ------------------------- | --------------------- | ------------------------------------------------------------------------- |
| **Développeur / Testeur** | [Votre nom]           | Rédaction du plan, écriture des tests, implémentation TDD, rapport final  |
| **Formateur / Valideur**  | Kake Abdoulaye        | Validation du plan de test, revue de la matrice de traçabilité, notation  |

_Ce plan de test doit être validé avant toute ligne de code de production (exigence TDD stricte)._