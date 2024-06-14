# Headless Prometheus Exporter

A [Prometheus](https://prometheus.io) exporter for the [Resonite](https://resonite.com) headless software.

## Installation

1. Download and install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader)
1. Download the [latest version of this mod](https://g.j4.lc/general-stuff/resonite/headless-prometheus-exporter/-/releases)
1. Move the mod dll into the `rml_mods` folder
1. Set your Prometheus scrape config to `localhost:9000` or `headless_container:9000`
1. Enjoy

## Data

Generic data:
- `totalPlayers` - Total players in all the sessions
- `totalWorlds` - Total worlds opened
- `engineFps` - FPS of the server

Per world:
- `world_users{label="S-<sessionId>"}` - Users in current world
- `world_maxusers{label="<sessionId>"}` - Maximum users allowed in world

Those names will not change in the future to ensure backwards compatibility of metrics.

