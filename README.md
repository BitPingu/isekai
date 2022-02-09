# Isekai

**Version 1.0**

An open world game developed using Unity2D. Utilizes procedural generation with tilemaps for world building. Inspired by my own game Genshin Chronicles.  

**Currently a work-in-progress.**

## Overview

The player moving around possible generated worlds:

![demo](https://github.com/BitPingu/isekai/blob/main/Demo/isekai.gif)

## Download

[Download for Windows](https://github.com/BitPingu/isekai/releases/download/v1.0/Isekai-1.0-windows.zip)

## Development

After making Genshin Chronicles, I've wanted to start using a real game engine to realize my dream game to its fullest potential which was limited by being text based.

The game doesn't have a clear end goal yet. I've wanted to first focus on generating a randomized world using procedural generation. I used a mix of algorithms to get this to
work, including perlin noise for generating the island, and cellular automata for generating the forests and grass patches. I planned to make the game top down 2D so I used tiles
for the graphics of the game, with each tile representing an enumerated constant on a grid. Unity's Rule Tiles helped to make the coastline look more natural. The graphics were 
done by me using Aseprite. :)

Afterwords I started making the player which can move around the island, ensuring that it can't go into the water using Unity's colliders, as well as having the camera follow him.
I've also animated his movement and changed his movement speed depending on the terrain. The spawn point is randomly set on any grass tile.

That's all I have done so far. In the future I plan on adding NPCs to make the world feel more lively, as well as generate landmarks such as villages and dungeons, and potentially 
add a story.

**Fun Fact:** The name of the game was inspired by the genre of the same name in Japanese fiction where the protagonist is transported into another world.

## Acknowledgements

- [Brackeys](https://www.youtube.com/channel/UCYbK_tjZ2OrIZFBvU6CCMiA)
- [Code 2D](https://code2d.wordpress.com/)
- [nextProgram](https://www.youtube.com/channel/UC-MGHRKbmkden1SjxdL8UzA)
- [Pixel Pete](https://www.youtube.com/channel/UC7OO80qJzGTLOj_6-0dmOiA)
- [Sebastian Lague](https://www.youtube.com/c/SebastianLague)
