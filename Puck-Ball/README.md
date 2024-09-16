# Puck-Ball

## The Game Client

Players must collect as most items as they can and, most important, more than the other potentially thousands of players in the match; that's the goal! All the collected counts are persisted throughout the player sessions, also state in the world.

## Development

This client project was started from "Core/3D (Built-In Render Pipeline)" Unity template; it seemed like the most "from scratch" option for this learning-focused project.

Nakama SDK (for game server integration) was imported into the project from Assets Store; ideally, it should have been added as a dependency from a external registry for proper package management (a way would be to add it directly to `manifest.json`, including its registry information).

Singletons are very useful for sharing variables like a socket connection; it might present a problem though when we try to track code dependencies; I tried my best to make good usage of Singletons, pursuing a good balance between convenience and maintainability by using singletons only for shared variables that shouldn't be re-initialized all the time, those are initialized a single time in authoritative/predictable places of the code.

Empty game objects for code components of specific purposes, like spawning players or update adversaries positions are placed in some scenes where necessary; those are managers, e.g. `GameManager`.
