# Puck-Ball

## The Game

*Disclaimer: This project is a learning resource to me, so do not expect much of it.*

It is nice to provide a meaningful name to a game; this game is essentially an online multiplayer hunting of collectibles with spheric characters; this is loosely similar to Pac-Man; hence the name.

The gameplay is physics-based: control your character using the axis (or WASD); a force will be applied to the ball in the inputted direction; don't confuse this force with instant acceleration, velocity or even impulse/momentum, it is mass-dependent; this creates an interesting and realistic challenge for braking or changing directions.

There will be only one match for players to join, this is intentional; this match is created at the server side.

Authentication is required; besides being helpful for the online experience, this will be used in the next versions for keeping track of collected items so far and player coordinates in the match; the intention is to provide MMO-like experience.

## Development

Authentication with email and password isn't the better way, it causes too much friction for the first contact with the game; automatic device id authentication would be enough for sure, however the former facilitates testing multiple instances of the game in the same device for development purposes at this first moment.

There is a lot of improvements to be done: unit testing, 3D models, server-client communication, UIs etc.; as this is a academic development, I'll be focused on working on learning-rich features so a amateur general experience is expected.
