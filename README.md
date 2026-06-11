# ⚔️ EscrimeGame

> Moteur de jeu d'escrime en .NET 9 avec API ASP.NET Core et interface Vite/TypeScript.

## Structure du projet

| Dossier | Description |
|---|---|
| `EscrimeGame/` | Bibliothèque principale (moteur de jeu) |
| `EscrimeGame.Api/` | API REST ASP.NET Core + SQLite |
| `EscrimeGame.Tests/` | Tests NUnit avec couverture de code |
| `escrime-web/` | Interface web Vite + TypeScript |

## CI / Intégration continue

La pipeline GitHub Actions s'exécute à **chaque push et pull request** et effectue :

- ✅ **Build** de la solution .NET 9 en mode Release
- 🧪 **Tests NUnit** avec rapport `.trx` publié dans l'UI GitHub
- 📊 **Couverture de code** collectée via `coverlet`, générée avec `ReportGenerator`
  - Résumé Markdown visible dans chaque **Job Summary** GitHub Actions
  - Rapport HTML complet archivé comme **artifact** (conservé 14 jours)
  - Commentaire automatique sur chaque **Pull Request**
- 🔷 **TypeScript check** et **build Vite** du frontend

### Voir la couverture

Après chaque run CI, ouvrez l'onglet **Actions** → sélectionnez un workflow run → descendez jusqu'à **Job Summary** pour voir le tableau de couverture.

Le rapport HTML complet est téléchargeable via les **Artifacts** du run.

## Développement local

```bash
# Backend
dotnet restore
dotnet build
dotnet test --collect:"XPlat Code Coverage"

# Frontend
cd escrime-web
npm install
npm run dev
```
