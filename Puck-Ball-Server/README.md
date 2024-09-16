# Puck-Ball Server

## The Server

It takes care of online multiplayer including accounts, matches and progression tracking; it is an authoritative dedicated server for matches, meaning it will have the final word over the gameplay, avoiding cheating and holding gameplay state.

Gameplay logic only relays players positions for now; next versions will have collectibles feature and some anti-cheating validations.

## Development

Nakama for Game Server Framework; it takes care of recurrent features/implementations like authentication, accounts, matches, connections/sockets etc.; it is extended with a Go module containing gameplay rules.

Tickrate was set for 30-60 because the game requires rapid feedback to players as being a fast-paced realtime MMO-like action game.
