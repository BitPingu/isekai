# Isekai

**Version 1.1**

An open world game developed in Unity. Features procedurally generated top down 2D worlds for the player to freely explore. Spiritual successor of [Genshin Chronicles.](https://github.com/BitPingu/genshin-chronicles)

**Currently a work-in-progress.**

## Overview

The player moving around possible generated worlds:

<img src="/Demo/isekai.gif" width="50%" height="50%"/>

**New in Version 1.1:**

Sprite update with new foliage 

<img src="/Demo/v1.1.png" width="50%" height="50%"/>

Title screen, menus, and save system

<img src="/Demo/title.gif" width="50%" height="50%"/>

NPC spawning, villages, and usable world map with fog (for discovery)

<img src="/Demo/map.gif" width="50%" height="50%"/>

Day and night cycle (with overworld music)

<img src="/Demo/day.gif" width="50%" height="50%"/>

## Installation

1. [Download the latest version for Windows (v1.1)](https://github.com/BitPingu/isekai/releases/download/v1.1/Isekai-1.1-windows.zip)
2. Extract the files
3. Run the executable

Game Controls:
- [ASWD] Move Player
- [M] Use Map
- [Esc] Pause Game

## Development

After making Genshin Chronicles, I've wanted to start learning how to use a game engine to bring my dream game to fruition which was limited by being text based. I was initially going to build off of what my previous game had, but because of Unity's capabilities, my vision for this game is on a much bigger scale, so much so that it has become a lifelong project of mine. My goal is to encapsulate all the elements of an rpg game in order to create an immersive fantasy experience that feels as though you are transported or "isekai'd" into the game. 

The main focus during development was procedural world generation. Not only did this allow for replayability, but it made it so that I didn't need to meticulously handcraft every section of the world, saving a lot of development time. I used a mix of algorithms to get this to work including; perlin noise for generating the island, cellular automata for generating the forests, and poisson disc sampling for generating the dungeon entrances (also the same algorithm used for enemy spawning). 

I planned to make the game top down 2D like Genshin Chronicles so I used 16x16 sprites and tilemaps for the graphics of the game. Unity's Rule Tiles helped to make the coastlines and village outlines look more natural. The sprites were personally made by myself using Aseprite.

As for the gameplay, the player does not have a lot of options at the moment other than movement since most of the code right now is focused on building the world. Eventually I plan to add a combat system as well as interaction with villagers and overworld objects.

**Name Origin:** "Isekai" is a genre in Japanese fiction where the protagonist is transported into another world.

## Future Considerations

Obviously this game is still in very early stage and I plan to add much more content to the game once I have the knowledge and/or motivation to. Here is some of the content you could potentially see in a future update:
- Character creation
- Combat system
- Interaction with villagers
- Interaction with overworld (ie. houses, dungeons)
- More overworld structures (ie. Goblin campsites)
- Crafting system
- Quest system
- A storyline
- and more...

## Acknowledgements

- [Brackeys](https://www.youtube.com/channel/UCYbK_tjZ2OrIZFBvU6CCMiA) for general game development tutorials
- [Code 2D](https://code2d.wordpress.com/) for tilemap procedural generation tutorials
- [nextProgram](https://www.youtube.com/channel/UC-MGHRKbmkden1SjxdL8UzA) for inspiration (and having a similar game)
- [Pixel Pete](https://www.youtube.com/channel/UC7OO80qJzGTLOj_6-0dmOiA) for pixel art guides
- [Sebastian Lague](https://www.youtube.com/c/SebastianLague) for all the algorithms used in my game
