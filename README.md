# Unity Turn-Based Strategy Demo

A grid-based, turn-driven tactics prototype built in Unity. The player commands units on a tile grid, spends action points on abilities, and fights enemy AI while interacting with destructible objects, doors, and a simple economy layer.

**Author:** Nate Courchesne  
**Contact:** natecourchesne@gmail.com · [LinkedIn](https://www.linkedin.com/in/natecourchesne/) 

[![Gameplay Demo](Recordings/demo/thumbnail.jpg)](https://nateyboy12.github.io/Unity-Turn-Based-Strategy-Demo/)

**[Watch gameplay demo](https://nateyboy12.github.io/Unity-Turn-Based-Strategy-Demo/)**

---

## Overview

This project demonstrates core Unity gameplay engineering: custom grid logic, turn flow, action selection, A\* pathfinding, AI decision-making, UI tied to game state, and camera/input polish. It is intended as a **readable, playable sample** for technical interviews—open the scene, press Play, and inspect how systems connect in code.

> **Note:** This repository is based on [Code Monkey’s Turn-Based Strategy course](https://www.youtube.com/c/CodeMonkeyUnity). I completed and extended the project as part of my Unity learning path.

---

## Features

### Core gameplay

- **Turn system** — Alternating player / enemy turns with turn counter and events (`TurnSystem`).
- **Grid & positioning** — Generic `GridSystem<T>` with `GridPosition`, `LevelGrid`, and visual feedback for valid tiles.
- **Unit actions** — Extensible `BaseAction` pattern: Move, Shoot, Sword, Grenade, Spin, Interact, each with validation, AP cost, and completion callbacks.
- **Action points & busy state** — `Unit` + `UnitActionSystem` gate input while actions run; UI reflects selection and busy state.
- **Pathfinding** — A on the grid with straight/diagonal move costs and obstacle height checks (`Pathfinding`, `PathNode`).
- **Enemy AI** — State machine picks best-scoring `EnemyAIAction` per valid tile when it is the enemy turn (`EnemyAI`, `BaseAction.GetBestEnemyAIAction`).

### World & interactions

- **Health & combat** — `HealthSystem`, projectiles (`BulletProjectile`, `GrenadeProjectile`), ragdoll on death.
- **Interactables** — `IInteractable` implemented by doors, barrels, crates, interact spheres.
- **Level scripting** — `LevelScripting`, destructibles, currency pickups (`CurrencySystem` + UI).

### Presentation & feel

- **Cinemachine** camera management and **CameraController** (pan, rotate, zoom).
- **Unity Input System** — centralized `InputManager` with legacy `#else` fallbacks.
- **UI (UGUI + TextMesh Pro)** — action buttons, turn display, unit world UI, busy overlay.
- **Polish** — screen shake, unit animators, selection visuals.

---

## Tech stack

|                 |                                              |
| --------------- | -------------------------------------------- |
| **Engine**      | Unity **2022.1** (LTS family)                |
| **Rendering**   | Universal Render Pipeline (URP)              |
| **Input**       | Unity Input System (`com.unity.inputsystem`) |
| **Camera**      | Cinemachine                                  |
| **Level tools** | ProBuilder (package in manifest)             |
| **UI**          | uGUI, TextMesh Pro                           |
| **Language**    | C# (.NET Standard / Unity scripting)         |

---

## Getting started

### Requirements

- **Unity Hub** with editor **2022.1.x** (match `ProjectSettings/ProjectVersion.txt` if present after clone).
- Git LFS (large assets: textures, fonts, etc.) — install [Git LFS](https://git-lfs.com/) then `git lfs pull` after clone.

### Run the game

1. Clone the repository and open the **project folder** (the directory that contains `Assets/` and `ProjectSettings/`) in Unity Hub—not a duplicate copy elsewhere on disk.
2. Open scene: `Assets/Scenes/GameScene.unity` (the `.unity` file, not the `GameScene/` folder).
3. Press **Play**.

## Controls

| Input                               | Action                                 |
| ----------------------------------- | -------------------------------------- |
| **WASD**                            | Move camera                            |
| **Q / E**                           | Rotate camera                          |
| **Mouse wheel**                     | Zoom                                   |
| **Left click on grid**              | Select unit / confirm grid action      |
| **Left click on Action UI buttons** | Choose Move, Shoot, etc.               |
| **Left click on End turn**          | Pass to enemy (via UI / flow in scene) |

Exact bindings live in `PlayerInputActions` and `InputManager`.
