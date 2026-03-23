# CMP1140 – Leaderboard starter code (C# / (.NET 9.0.310))

This collection is a starter codebase for the CMP1140 Assessment Item 1.

## Projects

- `Leaderboard.Core` – model + leaderboard logic (this is where most changes should happen)
- `Leaderboard.App` – minimal console UI (not assessed, and should only require minimal modification)
- `Leaderboard.Tests` – xUnit tests for required behaviour

## Data

- `data/players_small.csv` – small dataset for debugging
- `data/players_large.csv` – 50,000 players (to make inefficiencies visible)

CSV format (with header): `id,displayName,score`

## Getting started

From the collection root (requires .NET SDK 9.0.310):

```bash
dotnet restore
dotnet test
dotnet run --project Leaderboard.App
```

In the console app, try:

- `load data/players_small.csv`
- `top 10`
- `rank echo`
- `add newplayer New_Player 10`
- `score newplayer 50`

## Inefficiencies (by design)

The starter implementation is intentionally inefficient:

- Player lookup by id is a linear search.
- Top-K and player rank sort all players on each call.

Your task is to refactor the internals so these operations behave well under the stated constraints.
See the assessment brief and CRG for more details.

## Stretch objectives

See `STRETCH.md`.


