# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.1] - 2026-May-24

### Fixed
- Adjusted the names of some goto locations and fixed some invalid / broken gotos

## [1.1.0] - 2026-May-23

### Added
- `/warp [area] [goto]`, acts as a `/warp [area]` followed by `/goto [goto]` once the player loads into the
  new map
- `/dungeon [area] [difficulty]`, allows warping to given map and set the dungeon difficulty to one of
  `EASY`, `NORMAL`, or `HARD`

### Changed
- The area and location names in commands now use partial matching and will warp directly if the match is unambiguous
  - `/warp arc` will usually warp you directly to Arcwood Pass, `/warp trial` to Trial of the Stars, etc.
  - `/goto way` will usually match `Waypoint` and go there, etc.
- Updated the names of goto locations

### Fixed
- Changed loading log message so that it is no longer a Warn
- Goto no longer messes with underwater state, shaders, map region triggers, etc.
- Commands should now work correctly in multiplayer

## [1.0.1] - 2026-May-15

### Fixed

- Updated to work with 12025.a3

## [1.0.0] - 2025-July-30

### Changed

**Initial mod release**