# Stretch objectives

These are optional stretch objectives intended to support marks above 80.

You do not need to complete all of these. One well-implemented and well-justified stretch objective is normally sufficient.

## 1) Players near me

Add a feature that, given a player id and a number `n`, returns the `n` players immediately above and below that player in rank.

Hints:
- Deterministic ordering must match the existing tie-break rule.
- Handle edge cases (player near top/bottom, missing id, n < 0).

## 2) Performance evaluation

Provide a brief, reproducible performance comparison between:
- the starter (naïve) approach, and
- your improved approach,

using `data/players_large.csv`.

Hints:
- Report timings for a small set of operations (e.g. 100 lookups, 100 score updates, 100 top-10 requests).
- Discussion the efficiency of your implementation using Big-O notation.

## 3) Robust ordering under frequent updates

Implement a coherent strategy for maintaining ordering when scores change frequently.

Hints:
- If you use an ordered structure, you must ensure updates do not violate ordering.
- If you use caching, you must ensure it cannot become stale.
